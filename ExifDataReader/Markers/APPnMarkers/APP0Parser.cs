using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ExifDataReader.Markers.APPnMarkers;

namespace ExifDataReader
{
    class APP0Parser : SegmentParser
    {
        protected override byte[] ExpectedMarker { get; } = { 0xff, 0xe0 };
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

            var iFDData = new IFDData0(amountOfDirectories, creationDate, dpi);
            return new APP0Data(isBigEndian, offset, iFDData);
        }
    }

    class APP0Data
    {
        public bool IsBigEndian { get; }
        public int Offset { get; }
        public IFDData0 IFDData { get; }
        //public DPIData DPIData { get; }
        public APP0Data(bool isBigEndian, int offset, IFDData0 iFDData)
        {
            IsBigEndian = isBigEndian;
            Offset = offset;
            IFDData = iFDData;
        }
    }

    class IFDData0
    {
        public int AmountOfDirectories { get; }
        public DateTime CreationDate { get; }
        public int DPI { get; }
        public IFDData0(int amountOfDirectories, DateTime creationDate, int dPI)
        {
            AmountOfDirectories = amountOfDirectories;
            CreationDate = creationDate;
            DPI = dPI;
        }
    }
}
