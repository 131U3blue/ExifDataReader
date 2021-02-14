using ExifDataReader.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

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
    class IFDTagParser2
    {
        public byte[] DirectoryTagNum { get; }
        public short DataFormatIndicator { get; }
        public int ComponentSize { get; }
        public int NumberOfComponents { get; }
        public bool IsOffset = false;
        public int DataValue { get; }
        public object ParsedData { get; }

        public IFDTagParser2(IByteReader byteReader, Span<byte> thisIFD, Span<byte> fullApp1Span)
        {
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


            static int GetComponentSize(int dataFormat)
            {
                if (dataFormat == 1 || dataFormat == 2 || dataFormat == 6 || dataFormat == 7)
                    return 1;
                else if (dataFormat == 3 || dataFormat == 8)
                    return 2;
                else if (dataFormat == 4 || dataFormat == 9 || dataFormat == 11)
                    return 4;
                else if (dataFormat == 5 || dataFormat == 10 || dataFormat == 12)
                    return 8;
                else return 0;
            }

            static object ParseData(int formatIndicator, Span<byte> releventSpan, IByteReader byteReader)
            {
                switch (formatIndicator) {
                    case 1:
                        return byteReader.ReadUByteFromSpan(releventSpan);
                    case 2:
                        return byteReader.ReadString(releventSpan);
                    case 3:
                        return byteReader.ReadUShort(releventSpan);
                    case 4:
                        return byteReader.ReadUInt(releventSpan);
                    case 5:
                        return byteReader.ReadURational(releventSpan);
                    case 6:
                        return byteReader.ReadByteFromSpan(releventSpan);
                    case 7:
                        return byteReader.ReadUByteFromSpan(releventSpan);
                    case 8:
                        return byteReader.ReadShort(releventSpan);
                    case 9:
                        return byteReader.ReadInt(releventSpan);
                    case 10:
                        return byteReader.ReadRational(releventSpan);
                    case 11:
                        return byteReader.ReadFloat(releventSpan);
                    case 12:
                        return byteReader.ReadDouble(releventSpan);
                    default:
                        return "error";
                }
            }
        }
    }
    class IFDDataObjects {
        public bool IsBigEndian { get; }
        public byte[] DirectoryTagNum { get; }  // 2 Bytes
        public int DataFormat { get; }          /* 2 Bytes:
                                                    * 1 = unsigned byte, 2 = ascii string, 3 =  unsigned short, 4 = unsigned long
                                                    * 5 = unsigned rational, 6 = signed byte, 7 = UNDEFINED, 8 = signed short
                                                    * 9 = signed long, 10 = signed rational, 11 = single float, 12 = double float */
        public int NumberOfComponents { get; }  // 4 Bytes
        public int DataValue { get; }

        public IFDDataObjects(IByteReader byteReader, Span<byte> fullIFDSegment) {
            var tagList = new PossibleIFDTagList();
            IEnumerable<object> parsedIFDObjectList = IFDFunctions.CheckTagMarkers(byteReader, fullIFDSegment, tagList);
            foreach (object IFDObject in parsedIFDObjectList) {
                Console.WriteLine(IFDObject.ToString());
                Console.WriteLine(IFDObject.GetType());
                object obj = GetIFDType(IFDObject);
                //IFDMarkers.DisplayCameraMakeData.GetData(obj);
            }

            static object GetIFDType(object obj) {
                switch(obj) {
                    case IFDMarkers.CameraMakeData cameraMake:
                        if (cameraMake.DataOrOffsetValue is OffsetDataFinder) {
                            object offset = cameraMake.DataOrOffsetValue;
                            
                        }
                        return cameraMake;
                    case IFDMarkers.ImageDescriptionData imageDesc:
                        return imageDesc;
                    default: return "hello";
                }    
            }
        }
    }

}
