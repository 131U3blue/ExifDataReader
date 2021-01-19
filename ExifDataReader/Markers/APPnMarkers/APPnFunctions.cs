using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExifDataReader.Markers.APPnMarkers
{
    static class APPnFunctions
    {
        public static bool IsBigEndian(byte firstByte, byte secondByte)
        {
            byte[] littleEndian = { 0x49, 0x49 }; //AKA Intel or Logical Order
            byte[] bigEndian = { 0x4d, 0x4d }; //AKA Motorolla or Reverse Order
            byte[] endianSignature = { firstByte, secondByte };
            if (endianSignature.SequenceEqual(littleEndian)) return false;
            else if (endianSignature.SequenceEqual(bigEndian)) return true;
            else return false;
        }
        public static int IFD0StartOffsetValue(bool isBigEndian, byte firstByte, byte secondByte, byte thirdByte, byte fourthByte)
        {
            if (isBigEndian) return (int)((firstByte * Math.Pow(256, 3)) + (secondByte * Math.Pow(256, 2)) + (thirdByte * 256) + (fourthByte));
            else return (int)((fourthByte * Math.Pow(256, 3)) + (thirdByte * Math.Pow(256, 2)) + (secondByte * 256) + (firstByte));
        }
        public static int GetAmountOfIFDDirectories(bool isBigEndian, byte firstByte, byte secondByte)
        {
            if (isBigEndian) return (firstByte * 256) + secondByte;
            else return (secondByte * 256) + firstByte;
        }
        //public static T CheckEndianness<T>()
        //{
        //    return IFDSegmentFunctions;
        //}


        public static (bool isIFD, int IFDNumber) GetIFDInfo()
        {
            return (true, 1);
        }
    }
    static class IFDSegmentFunctions
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
    }
}
