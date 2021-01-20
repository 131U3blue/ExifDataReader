using ExifDataReader.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExifDataReader.Markers.APPnMarkers
{
    static class IFDFunctions
    {
        public const int DirectoryLengthBytes = 12;
        public static List<byte[]> CreateIFDSegmentList(int directories, byte[] iFDSegments)
        {
            List<byte[]> directoryList = new List<byte[]>();
            for (int i = 0; i < directories; i++)
            {
                directoryList.Add(iFDSegments[(i * DirectoryLengthBytes)..((i + 1) * DirectoryLengthBytes)]);
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
        public static IEnumerable<object> CheckTagMarkers(bool isBigEndian, byte[] iFDSegmentArray, PossibleIFDTagList listOfTags) //Change acronyms to lower case
        {
            for (int i = 0; i < iFDSegmentArray.Length - 3; i++)
            {
                foreach (IIFDSegmentParser tag in listOfTags.InstantiatedList)
                {
                    if (tag.MatchesMarker(isBigEndian, iFDSegmentArray[i..(i + 2)]))
                    {
                        yield return tag.ParseSegment(iFDSegmentArray);
                    }
                }
            }
        }
    }
}
