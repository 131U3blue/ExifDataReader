using System;
using System.Collections.Generic;
using System.Text;

namespace ExifDataReader
{
    //DO I NEED THIS?
    interface IIFDSegmentParser
    {
        bool MatchesMarker(byte[] potentialMarker);
        object ParseSegment(byte[] fullSegment);
    }
}
