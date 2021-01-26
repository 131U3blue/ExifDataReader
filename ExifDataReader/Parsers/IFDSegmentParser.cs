using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExifDataReader.Markers.IFDMarkers
{
    abstract class IFDSegmentParser : IIFDSegmentParser
    {
        protected abstract byte[] ExpectedMarkerBigEndian { get; }
        public virtual bool MatchesMarker(bool isBigEndian, Span<byte> potentialMarker)
        {
            var potentialMarkerArray = potentialMarker.ToArray();
            if (isBigEndian) return potentialMarkerArray.SequenceEqual(ExpectedMarkerBigEndian);
            else return potentialMarkerArray.SequenceEqual(ExpectedMarkerBigEndian.Reverse());
        }
        public abstract object ParseSegment(Span<byte> fullSegment);
    }
}
