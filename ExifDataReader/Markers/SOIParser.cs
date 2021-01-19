using System;
using System.Collections.Generic;
using System.Text;

namespace ExifDataReader
{
    class SOIParser
    {
        public byte[] ExpectedMarker { get; } = { 0xff, 0xd8 };
        public object ParseSegment(byte[] relevantSegment)
        {
            throw new NotImplementedException();
        }

    }
}
