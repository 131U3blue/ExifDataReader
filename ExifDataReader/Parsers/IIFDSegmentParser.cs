using System;
using System.Collections.Generic;
using System.Text;

namespace ExifDataReader
{
    //DO I NEED THIS?
    interface IIFDSegmentParser
    {
        bool MatchesMarker(bool isBigEndian, byte[] potentialMarker);
        object ParseSegment(byte[] fullSegment);
    }
}
