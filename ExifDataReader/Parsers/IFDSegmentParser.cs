using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExifDataReader.Markers.IFDMarkers
{
    abstract class IFDSegmentParser : IIFDSegmentParser
    {
        protected abstract byte[] ExpectedMarkerBigEndian { get; }
        public virtual bool MatchesMarker(bool isBigEndian, byte[] potentialMarker)
        {
            if (isBigEndian) return potentialMarker.SequenceEqual(ExpectedMarkerBigEndian);
            else return potentialMarker.SequenceEqual(ExpectedMarkerBigEndian.Reverse());
        }
        public abstract object ParseSegment(byte[] fullSegment);
    }
}
