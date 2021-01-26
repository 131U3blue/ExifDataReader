using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExifDataReader
{
    abstract class SegmentParser : ISegmentParser
    {
        protected abstract byte[] ExpectedMarker { get; }
        protected abstract byte[] ExifHeader { get; }
        public virtual bool MatchesMarker(Span<byte> byteIndex0and1) => byteIndex0and1.SequenceEqual(ExpectedMarker);

        public virtual bool ExifValidator(Span<byte> potentialExifHeader) => potentialExifHeader.SequenceEqual(ExifHeader);
        public virtual int GetSegmentLength(Span<byte> segmentLengthSignature) => (segmentLengthSignature[0] * 256) + segmentLengthSignature[1];

        public abstract object ParseSegment(Span<byte> relevantSegment);
    }
}
