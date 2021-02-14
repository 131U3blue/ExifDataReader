using System;
using System.Collections.Generic;
using System.Text;

namespace ExifDataReader {
    //DO I NEED THIS?
    interface ISubSegmentParser {
        bool MatchesMarker(IByteReader byteReader, Span<byte> potentialMarker);
        object ParseSegment(IByteReader byteReader, Span<byte> fullSegment);
    }
}
