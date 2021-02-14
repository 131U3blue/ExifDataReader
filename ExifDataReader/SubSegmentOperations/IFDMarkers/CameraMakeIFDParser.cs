using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExifDataReader.Markers.IFDMarkers
{
    class CameraMakeIFDParser : ISubSegmentParser
    {
        protected byte[] ExpectedMarkerBigEndian { get; } = { 0x01, 0x0f };
        public virtual bool MatchesMarker(IByteReader byteReader, Span<byte> potentialMarker)
        {
            return byteReader.MatchesBigEndianByteString(ExpectedMarkerBigEndian, potentialMarker);
        }
        public virtual object ParseSegment(IByteReader byteReader, Span<byte> tagBytes) // <-- (!)tagBytes is the full IFD section(!)
        {
            /*Parses data assuming that the data is not offset further into the array. If the data IS offset,
            returns an object with all of the necessary information to parse the information later when the whole array
            is available. */
            short format = byteReader.ReadShort(tagBytes[2..4]);//<-- (!) bytes 2 and 3 of the FULL IFD SECTION, not the relevant IFD Segment (!)
            int numComponents = byteReader.ReadInt(tagBytes[4..8]);
            int offset = byteReader.ReadInt(tagBytes[8..12]);
            int totalDataLength = numComponents * offset;
            object data;
            if (totalDataLength <= 4) {
                data = IFDDataFormatter.GetData(byteReader, format, tagBytes[8..12]);
                return new CameraMakeData(format, numComponents, data);
            }
            else {
                data = new OffsetDataFinder(format, numComponents, offset, totalDataLength);
                return new CameraMakeData(format, numComponents, data); //When parsing the offset data, check if data is OffsetDescriptionFinder
            }
        }
    }
    class CameraMakeData {
        public short FormatIndicator { get; }
        public int NumberOfComponents { get; }
        public object DataOrOffsetValue { get; }

        public CameraMakeData(short format, int numComponents, object dataOrOffset)
        {
            FormatIndicator = format;
            NumberOfComponents = numComponents;
            DataOrOffsetValue = dataOrOffset;
        }
    }
}
