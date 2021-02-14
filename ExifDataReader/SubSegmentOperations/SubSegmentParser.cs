using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExifDataReader.Markers.IFDMarkers {
    abstract class SubSegmentParser : ISubSegmentParser {
        protected abstract byte[] ExpectedMarkerBigEndian { get; }
        public virtual bool MatchesMarker(IByteReader byteReader, Span<byte> potentialMarker) {
            return byteReader.MatchesBigEndianByteString(ExpectedMarkerBigEndian, potentialMarker);
        }
        public abstract object ParseSegment(IByteReader byteReader, Span<byte> fullSegment);
    }
}
