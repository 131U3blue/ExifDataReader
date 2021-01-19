using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ExifDataReader.Markers.APPnMarkers;

namespace ExifDataReader
{
    class APP3Parser : SegmentParser
    {
        protected override byte[] ExpectedMarker { get; } = { 0xff, 0xe3 };
        protected override byte[] ExifHeader { get; } = { 0x45, 0x78, 0x69, 0x66 };
        public override object ParseSegment(byte[] segmentByteArray)
        {
            var isBigEndian = APPnFunctions.IsBigEndian(segmentByteArray[10], segmentByteArray[11]);
            var offset = APPnFunctions.IFD0StartOffsetValue(
                isBigEndian, segmentByteArray[14], segmentByteArray[15], segmentByteArray[16], segmentByteArray[17]);
            var amountOfDirectories = APPnFunctions.GetAmountOfIFDDirectories(
                isBigEndian, segmentByteArray[offset], segmentByteArray[(offset + 1)]);
            var creationDate = DateTime.Now;
            var dpi = 3;

            var iFDData = new IFDData(amountOfDirectories, creationDate, dpi);
            return new APP3Data(isBigEndian, offset, iFDData);
        }
    }

    class APP3Data
    {
        public bool IsBigEndian { get; }
        public int Offset { get; }
        public IFDData IFDData { get;}
        //public DPIData DPIData { get; }
        public APP3Data(bool isBigEndian, int offset, IFDData iFDData)
        {
            IsBigEndian = isBigEndian;
            Offset = offset;
            IFDData = iFDData;
        }
    }
    class IFDData
    {
        public int AmountOfDirectories { get; }
        public DateTime CreationDate { get; }
        public int DPI { get; }
        public IFDData(int amountOfDirectories, DateTime creationDate, int dPI)
        {
            AmountOfDirectories = amountOfDirectories;
            CreationDate = creationDate;
            DPI = dPI;
        }
    }

    //class DPIData
    //{
    //    dpiHorizontal
    //    public DPIData()
    //}
}
