using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExifDataReader
{
    static class IFDDataFormatter
    {
        public static object GetData(IByteReader byteReader, int format, Span<byte> dataSpan)
        {
            switch (format) {
                case 1: return byteReader.ReadUByteFromSpan(dataSpan);
                case 2: return byteReader.ReadString(dataSpan);
                case 3: return byteReader.ReadUShort(dataSpan);
                case 4: return byteReader.ReadULong(dataSpan);
                case 5: return byteReader.ReadRational(dataSpan);
                case 6: return byteReader.ReadByteFromSpan(dataSpan);
                case 7: return dataSpan.ToArray();
                case 8: return byteReader.ReadShort(dataSpan);
                case 9: return byteReader.ReadLong(dataSpan);
                case 10: return byteReader.ReadRational(dataSpan);
                case 11: return byteReader.ReadFloat(dataSpan);
                case 12: return byteReader.ReadDouble(dataSpan);
                default: return "Something went wrong with the Image Description Parser Switch Case...";
            }
        }
    }
    //Finds the start of the actual data if IFD data is offset
    class OffsetDataFinder
    {
        public short FormatIndicator { get; }
        public int NumberOfComponents { get; }
        public int Offset { get; }
        public int DataLength { get; }
        public OffsetDataFinder(short format, int numComponents, int offset, int dataLength)
        {
            FormatIndicator = format;
            NumberOfComponents = numComponents;
            Offset = offset;
            DataLength = dataLength;
        }
    }
}
