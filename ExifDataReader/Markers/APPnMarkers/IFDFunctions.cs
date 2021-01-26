using ExifDataReader.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExifDataReader.Markers.APPnMarkers
{
    static class IFDFunctions
    {
        public const int DirectoryLength = 12;
        public static List<byte[]> CreateIFDSegmentList(int directories, Span<byte> iFDSegment)
        {
            List<byte[]> directoryList = new List<byte[]>();
            for (int i = 0; i < directories; i++) {
                directoryList.Add(iFDSegment.ToArray()[(i * DirectoryLength)..((i + 1) * DirectoryLength)]);
            }
            return directoryList;
        }
        public static byte[] IFDSegmentTag(bool isBigEndian, byte firstByte, byte secondByte)
        {
            byte[] bigEndianTag = { firstByte, secondByte };
            byte[] littleEndianTag = { secondByte, firstByte };
            if (isBigEndian) return bigEndianTag;
            else return littleEndianTag;
        }
        public static IEnumerable<object> CheckTagMarkers(bool isBigEndian, Span<byte> iFDSegmentSpan, PossibleIFDTagList listOfTags) //Change acronyms to lower case
        {
            var iFDSegmentList = new List<object>();
            for (int i = 0; i < iFDSegmentSpan.Length - 3; i++) {
                foreach (IIFDSegmentParser tag in listOfTags.InstantiatedList) {
                    if (tag.MatchesMarker(isBigEndian, iFDSegmentSpan[i..(i + 2)])) {
                        iFDSegmentList.Add(tag.ParseSegment(iFDSegmentSpan));
                    }
                }
            }
            return iFDSegmentList;
        }
    }
}
