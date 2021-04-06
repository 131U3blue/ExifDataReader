using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExifDataReader.Markers.APPnMarkers {
    static class APPnFunctions {
        public static bool IsBigEndian(byte firstByte, byte secondByte) {
            byte[] littleEndian = { 0x49, 0x49 }; //AKA Intel or Reverse Order
            byte[] bigEndian = { 0x4d, 0x4d }; //AKA Motorolla or Human Readable Order
            byte[] endianSignature = { firstByte, secondByte };
            if (endianSignature.SequenceEqual(littleEndian)) return false;
            else if (endianSignature.SequenceEqual(bigEndian)) return true;
            else return false;
        }
        public static (bool isIFD, int IFDNumber) GetIFDInfo() {
            return (true, 1);
        }
    }
    
}
