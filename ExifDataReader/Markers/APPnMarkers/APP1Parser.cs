using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ExifDataReader.Markers.APPnMarkers;

namespace ExifDataReader
{
    class APP1Parser : SegmentParser
    {
        protected override byte[] ExpectedMarker { get; } = { 0xff, 0xe1 };
        protected override byte[] ExifHeader { get; } = { 0x45, 0x78, 0x69, 0x66 };
        public override object ParseSegment(byte[] segmentByteArray)
        {
            bool isBigEndian = APPnFunctions.IsBigEndian(segmentByteArray[10], segmentByteArray[11]);
            int iFDHeaderOffset = 10;
            int iFDDirectoriesOffset = APPnFunctions.IFD0StartOffsetValue(
                isBigEndian, segmentByteArray[14], segmentByteArray[15], segmentByteArray[16], segmentByteArray[17]
            ); // Adding ten to start from the correct point
            int amountOfDirectories = APPnFunctions.GetAmountOfIFDDirectories(
                isBigEndian, segmentByteArray[iFDHeaderOffset + iFDDirectoriesOffset], segmentByteArray[(iFDHeaderOffset + iFDDirectoriesOffset) + 1]
            );
            byte[] iFDSegments = segmentByteArray[(iFDDirectoriesOffset + 2)..(iFDDirectoriesOffset + 2 + (amountOfDirectories * IFDSegmentFunctions.DirectoryLengthBytes))];
            List<byte[]> iFDDirectoryList = IFDSegmentFunctions.CreateIFDSegmentList(amountOfDirectories, iFDSegments);
            
            var creationDate = DateTime.Now;
            int dpi = 3;

            var iFDData = new IFDData1(amountOfDirectories, iFDDirectoryList, creationDate, dpi);
            var iFDParser = new IFDSegmentParser(isBigEndian, );
            var parsedIFDList = new List<IFDSegmentParser>();
            foreach(var segment in iFDDirectoryList)
            //var iFDSegment = new IFDSegment(segmentByteArray[20..])

            return new APP1Data(isBigEndian, iFDDirectoriesOffset, iFDData);
        }
    }

    class APP1Data
    {
        public bool IsBigEndian { get; }
        public int Offset { get; }
        public IFDData1 IFDData { get; }
        public IFDSegmentParser IFDSegment { get; }
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
    class IFDSegmentParser
    {
        public bool IsBigEndian { get; }
        public byte[] DirectoryTagNum { get; } // 2 Bytes
        public int DataFormat { get; } /* 2 Bytes:
                                        * 1 = unsigned byte, 2 = ascii string, 3 =  unsigned short, 4 = unsigned long
                                        * 5 = unsigned rational, 6 = signed byte, 7 = UNDEFINED, 8 = signed short
                                        * 9 = signed long, 10 = signed rational, 11 = single float, 12 = double float */
        public int NumberOfComponents { get; } // 4 Bytes
        public int DataValue { get; }

        public IFDSegmentParser(bool isBigEndian, byte[] fullIFDSegment)
        {
            IsBigEndian = isBigEndian; 
            DirectoryTagNum = IFDSegmentFunctions.IFDSegmentTag(IsBigEndian, fullIFDSegment[0], fullIFDSegment[1]);
        }

    }
}
