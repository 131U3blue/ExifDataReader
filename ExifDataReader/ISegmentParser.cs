using System;
using System.Collections.Generic;
using System.Text;

namespace ExifDataReader
{
    interface ISegmentParser
    {
        bool MatchesMarker(byte[] potentialMarker);
        bool ExifValidator(byte[] potentialExifHeader);
        int GetSegmentLength(byte[] segmentLength);
        object ParseSegment(byte[] fullByteArray);

    }

}
