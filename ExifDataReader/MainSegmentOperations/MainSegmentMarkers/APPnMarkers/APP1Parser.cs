using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ExifDataReader.Markers.APPnMarkers;
using ExifDataReader.Parsers;

namespace ExifDataReader {
    /*The below class looks a little complicated so I will explain here what each part is doing for ease of understanding:
     * We are defining how the APP1 segments are parsed if they are found and also finding the sub-segments within.
     * isBigEndian defines the endianness of the segment which should remain the same throughout. 
     * We instantiate Big/LittleEndianByte Reader and pass it to subsequent functions.
     * The offset to the start of the actual header's data is always 10.
     * We get an int to represent the offset to the IFD Directories contained within APP1
     * We get a short to find the number of directories
     * We get a new span which starts from after the indicator of the /NUMBER of directories/ up to the end of the directory data.
     * We create a list of all the separated directories
     * START TO FILL OUT IFD DATA
     */
    class APP1Parser : MainSegmentParser {
        protected override byte[] ExpectedMarker { get; } = { 0xff, 0xe1 };
        protected override byte[] ExifHeader { get; } = { 0x45, 0x78, 0x69, 0x66 };
        public override object ParseSegment(Span<byte> aPP1ByteSpan) {
            var startFromTIFFSpan = aPP1ByteSpan[10..aPP1ByteSpan.Length];
            bool isBigEndian = APPnFunctions.IsBigEndian(startFromTIFFSpan[0], startFromTIFFSpan[1]);
            IByteReader dataParser = isBigEndian ? new BigEndianByteReader() : new LittleEndianByteReader();
            int offsetToIFDDataStart = dataParser.ReadInt(startFromTIFFSpan[4..8]);
            var startFromIFDSpan = startFromTIFFSpan[offsetToIFDDataStart..startFromTIFFSpan.Length];
            short numberOfDirectories = dataParser.ReadShort(startFromIFDSpan[0..2]);
            Span<byte> iFDSpan = startFromIFDSpan[2..(2 + (numberOfDirectories * IFDFunctions.DirectoryLength))];
            List<byte[]> iFDDirectoryList = IFDFunctions.CreateIFDSegmentList(numberOfDirectories, iFDSpan);

            IFDOverview iFDData = new IFDOverview(numberOfDirectories, iFDDirectoryList);
            var parsedDirectoryDataList = new List<IFDTagParser2>();
            foreach (byte[] directorySpan in iFDDirectoryList) {
                var directoryData = new IFDTagParser2(dataParser, directorySpan, startFromTIFFSpan);
                parsedDirectoryDataList.Add(directoryData);
            }

            return new APP1Data(isBigEndian, offsetToIFDDataStart, iFDData, parsedDirectoryDataList);
        }
    }
    class APP1Data {
        public bool IsBigEndian { get; }
        public int Offset { get; }
        public IFDOverview IFDOverview { get; }
        public List<IFDTagParser2> IFDData { get; }
        //public IFDDataObjects IFDSegment { get; }
        //public DPIData DPIData { get; }
        public APP1Data(bool isBigEndian, int offset, IFDOverview overview, List<IFDTagParser2> parsedDataList) {
            IsBigEndian = isBigEndian;
            Offset = offset;
            IFDOverview = overview;
            IFDData = parsedDataList;
        }
    }
}
