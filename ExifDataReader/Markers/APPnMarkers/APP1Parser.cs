using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ExifDataReader.Markers.APPnMarkers;
using ExifDataReader.Parsers;

namespace ExifDataReader
{
    class APP1Parser : SegmentParser
    {
        protected override byte[] ExpectedMarker { get; } = { 0xff, 0xe1 };
        protected override byte[] ExifHeader { get; } = { 0x45, 0x78, 0x69, 0x66 };
        public override object ParseSegment(byte[] aPP1ByteArray)
        {
            bool isBigEndian = APPnFunctions.IsBigEndian(aPP1ByteArray[10], aPP1ByteArray[11]);
            int iFDHeaderOffset = 10;
            int iFDDirectoriesOffset = APPnFunctions.IFD0StartOffsetValue(
                isBigEndian, aPP1ByteArray[14], aPP1ByteArray[15], aPP1ByteArray[16], aPP1ByteArray[17]
            ); // Adding ten to start from the correct point
            int amountOfDirectories = APPnFunctions.GetAmountOfIFDDirectories(
                isBigEndian, aPP1ByteArray[iFDHeaderOffset + iFDDirectoriesOffset], aPP1ByteArray[(iFDHeaderOffset + iFDDirectoriesOffset) + 1]
            );
            byte[] iFDSegments = aPP1ByteArray[(iFDDirectoriesOffset + 2)..(iFDDirectoriesOffset + 2 + (amountOfDirectories * IFDFunctions.DirectoryLengthBytes))];
            List<byte[]> iFDDirectoryList = IFDFunctions.CreateIFDSegmentList(amountOfDirectories, iFDSegments);
            
            var creationDate = DateTime.Now;
            int dpi = 3;

            var iFDData = new IFDData1(amountOfDirectories, iFDDirectoryList, creationDate, dpi);
            var iFDParser = new IFDTagParser(isBigEndian, iFDSegments);

            //foreach(var segment in iFDDirectoryList)
            //var iFDSegment = new IFDSegment(segmentByteArray[20..])

            return new APP1Data(isBigEndian, iFDDirectoriesOffset, iFDData);
        }
    }

    class APP1Data
    {
        public bool IsBigEndian { get; }
        public int Offset { get; }
        public IFDData1 IFDData { get; }
        public IFDTagParser IFDSegment { get; }
        //public DPIData DPIData { get; }
        public APP1Data(bool isBigEndian, int offset, IFDData1 iFDData)
        {
            IsBigEndian = isBigEndian;
            Offset = offset;
            IFDData = iFDData;
        }
    }

    class IFDData1
    {
        public int AmountOfDirectories { get; }
        public List<byte[]> ListOfDirectories { get; }
        public DateTime CreationDate { get; }
        public int DPI { get; }
        public IFDData1(int amountOfDirectories, List<byte[]> directoryList, DateTime creationDate, int dPI)
        {
            AmountOfDirectories = amountOfDirectories;
            ListOfDirectories = directoryList;
            CreationDate = creationDate;
            DPI = dPI;
        }
    }
    class IFDTagParser
    {
        public bool IsBigEndian { get; }
        public byte[] DirectoryTagNum { get; } // 2 Bytes
        public int DataFormat { get; } /* 2 Bytes:
                                        * 1 = unsigned byte, 2 = ascii string, 3 =  unsigned short, 4 = unsigned long
                                        * 5 = unsigned rational, 6 = signed byte, 7 = UNDEFINED, 8 = signed short
                                        * 9 = signed long, 10 = signed rational, 11 = single float, 12 = double float */
        public int NumberOfComponents { get; } // 4 Bytes
        public int DataValue { get; }

        public IFDTagParser(bool isBigEndian, byte[] fullIFDSegment)
        {
            IsBigEndian = isBigEndian;
            var tagList = new PossibleIFDTagList();
            IEnumerable<object> parsedIFDObjectList = IFDFunctions.CheckTagMarkers(isBigEndian, fullIFDSegment, tagList);

        }

    }
}
