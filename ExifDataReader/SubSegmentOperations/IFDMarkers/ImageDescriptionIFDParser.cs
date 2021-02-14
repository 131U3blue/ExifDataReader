using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Buffers.Binary;

namespace ExifDataReader.Markers.IFDMarkers {
    class ImageDescriptionIFDParser : SubSegmentParser {
        protected override byte[] ExpectedMarkerBigEndian { get; } = { 0x01, 0x0e };
        public override object ParseSegment(IByteReader byteReader, Span<byte> tagBytes) {
            /*Parses data assuming that the data is not offset further into the array. If the data IS offset,
            returns an object with all of the necessary information to parse the information later when the whole array
            is available. */
            short format = byteReader.ReadShort(tagBytes[2..4]);
            int numComponents = byteReader.ReadInt(tagBytes[4..9]);
            int offset = byteReader.ReadInt(tagBytes[9..13]);
            int totalDataLength = numComponents * offset;
            object data;
            if (totalDataLength <= 4) {
                data = IFDDataFormatter.GetData(byteReader, format, tagBytes[9..13]);
                return new ImageDescriptionData(format, numComponents, data);
            }
            else {
                data = new OffsetDataFinder(format, numComponents, offset, totalDataLength);
                return new ImageDescriptionData(format, numComponents, data); //When parsing the offset data, check if data is OffsetDescriptionFinder
            }
        }
    }
    class ImageDescriptionData {
        public short FormatIndicator { get; }
        public int NumberOfComponents { get; }
        public object DataOrOffsetValue { get; }

        public ImageDescriptionData(short format, int numComponents, object dataOrDataOffset)
        {
            FormatIndicator = format;
            NumberOfComponents = numComponents;
            DataOrOffsetValue = dataOrDataOffset;
        }
    }
}
