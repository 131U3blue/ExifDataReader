using System;
using System.Collections.Generic;
using System.Text;

namespace ExifDataReader {
    interface IMainSegmentParser {
        bool MatchesMarker(Span<byte> potentialMarker);
        bool ExifValidator(Span<byte> potentialExifHeader);
        int GetSegmentLength(Span<byte> segmentLength);
        object ParseSegment(Span<byte> fullByteArray);

    }

}
