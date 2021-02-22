using ExifDataReader.Parsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ExifDataReader.Markers.APPnMarkers {
    static class IFDFunctions {
        public const int DirectoryLength = 12;                                                              //Directory length is ALWAYS 12 bytes

        public static List<byte[]> CreateIFDSegmentList(int directories, Span<byte> iFDSegment) {           //
            List<byte[]> directoryList = new List<byte[]>();                                                //
            for (int i = 0; i < directories; i++) {                                                         //
                directoryList.Add(iFDSegment.ToArray()[(i * DirectoryLength)..((i + 1) * DirectoryLength)]);//Looping through whole IFD SubSegment to add each directory to the list.
            }
            return directoryList;
        }
        public static byte[] IFDSegmentTag(bool isBigEndian, byte firstByte, byte secondByte) {
            byte[] bigEndianTag = { firstByte, secondByte };
            byte[] littleEndianTag = { secondByte, firstByte };
            if (isBigEndian) return bigEndianTag;
            else return littleEndianTag;
        }
        public static IEnumerable<object> CheckTagMarkers(IByteReader byteReader, Span<byte> iFDSegmentSpan, PossibleIFDTagList listOfTags) {
            var iFDSegmentList = new List<object>();
            for (int i = 0; i < iFDSegmentSpan.Length - 3; i++) {
                foreach (ISubSegmentParser tag in listOfTags.InstantiatedList) {
                    if (tag.MatchesMarker(byteReader, iFDSegmentSpan[i..(i + 2)])) {
                        iFDSegmentList.Add(tag.ParseSegment(byteReader, iFDSegmentSpan));
                    }
                }
            }
            return iFDSegmentList;
        }
    }
    class IFDOverview {
        public int AmountOfDirectories { get; }
        public List<byte[]> ListOfDirectories { get; }
        public IFDOverview(int amountOfDirectories, List<byte[]> directoryList) {
            AmountOfDirectories = amountOfDirectories;
            ListOfDirectories = directoryList;
        }
    }
    class IFDTagParser {
        private byte[] SubIFDTag = { 0x87, 0x69 };
        public byte[] DirectoryTagNum { get; }
        public short DataFormatIndicator { get; }
        public int ComponentSize { get; }
        public int NumberOfComponents { get; }
        public bool IsOffset = false;
        public int DataValue { get; }
        public object ParsedData { get; }
        public List<IFDTagParser> SubIFDData { get; } = new List<IFDTagParser>();
        public IFDTagParser(IByteReader byteReader, Span<byte> thisIFD, Span<byte> fullApp1Span) {
            DirectoryTagNum = thisIFD[0..2].ToArray();
            DataFormatIndicator = byteReader.ReadShort(thisIFD[2..4]);
            NumberOfComponents = byteReader.ReadInt(thisIFD[4..8]);
            ComponentSize = GetComponentSize(DataFormatIndicator);
            var offsetIndicator = NumberOfComponents * ComponentSize;
            if (offsetIndicator > 4) {
                IsOffset = true;
            }
            var offsetVal = byteReader.ReadInt(thisIFD[8..12]);
            ParsedData = !IsOffset ? ParseData(DataFormatIndicator, thisIFD[8..12], byteReader) : ParseData(DataFormatIndicator, fullApp1Span[offsetVal..(offsetVal + offsetIndicator)], byteReader);
            if (byteReader.MatchesBigEndianByteString(SubIFDTag, DirectoryTagNum)) {
                var subIFDList = new List<SubIFDParser>();
                var offset = (int)(uint)ParsedData;
                var numSubComponents = byteReader.ReadShort(fullApp1Span[offset..(offset + 2)]);
                var relevantSpan = fullApp1Span[(offset + 2)..((offset + 2) + (numSubComponents * 12))];
                for (int i = 0; i < numSubComponents; i++) {
                    var subIfDData = new IFDTagParser(byteReader, relevantSpan[(i * 12)..((i * 12) + 12)], fullApp1Span);
                    SubIFDData.Add(subIfDData);
                }
            }

            static int GetComponentSize(int dataFormat) {
                if (dataFormat == 1 || dataFormat == 2 || dataFormat == 6 || dataFormat == 7) return 1;
                else if (dataFormat == 3 || dataFormat == 8) return 2;
                else if (dataFormat == 4 || dataFormat == 9 || dataFormat == 11) return 4;
                else if (dataFormat == 5 || dataFormat == 10 || dataFormat == 12) return 8;
                else return 0;
            }

            static object ParseData(int formatIndicator, Span<byte> releventSpan, IByteReader byteReader) {
                return formatIndicator switch {
                    1 => byteReader.ReadUByteFromSpan(releventSpan),
                    2 => byteReader.ReadString(releventSpan),
                    3 => byteReader.ReadUShort(releventSpan),
                    4 => byteReader.ReadUInt(releventSpan),
                    5 => byteReader.ReadURational(releventSpan),
                    6 => byteReader.ReadByteFromSpan(releventSpan),
                    7 => byteReader.ReadUByteFromSpan(releventSpan),
                    8 => byteReader.ReadShort(releventSpan),
                    9 => byteReader.ReadInt(releventSpan),
                    10 => byteReader.ReadRational(releventSpan),
                    11 => byteReader.ReadFloat(releventSpan),
                    12 => byteReader.ReadDouble(releventSpan),
                    _ => "error",
                };
            }
        }
        public class SubIFDParser
        {
            
        }
    }
}
