using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace ExifDataReader
{
    class LittleEndianByteReader : IByteReader
    {
        public int ReadInt(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadInt32LittleEndian(byteSpan);
        public uint ReadUInt(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadUInt32LittleEndian(byteSpan);
        public string ReadString(ReadOnlySpan<byte> byteSpan) => Encoding.ASCII.GetString(byteSpan);
        public short ReadShort(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadInt16LittleEndian(byteSpan);
        public ushort ReadUShort(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadUInt16LittleEndian(byteSpan);

        public long ReadLong(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadInt64LittleEndian(byteSpan);

        public ulong ReadULong(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadUInt64LittleEndian(byteSpan);

        public float ReadFloat(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadSingleLittleEndian(byteSpan);

        public double ReadDouble(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadDoubleLittleEndian(byteSpan);
        public byte ReadUByteFromSpan(ReadOnlySpan<byte> byteSpan) => byteSpan[0];
        public sbyte ReadByteFromSpan(ReadOnlySpan<byte> byteSpan) => Convert.ToSByte(byteSpan[0]);
        public URational ReadURational(ReadOnlySpan<byte> byteSpan)  {
            return new URational(BinaryPrimitives.ReadUInt32LittleEndian(byteSpan[0..4]), BinaryPrimitives.ReadUInt32LittleEndian(byteSpan[4..(byteSpan.Length)]));
        }
        public Rational ReadRational(ReadOnlySpan<byte> byteSpan) {
            return new Rational(BinaryPrimitives.ReadInt32LittleEndian(byteSpan[0..4]), BinaryPrimitives.ReadInt32LittleEndian(byteSpan[4..(byteSpan.Length)]));
        }
        public bool MatchesBigEndianByteString(ReadOnlySpan<byte> bigEndiandTargetString, ReadOnlySpan<byte> byteSpan)
        {
            if (bigEndiandTargetString.Length != byteSpan.Length) { return false; }
            for (int i = 0; i < byteSpan.Length; i++) {
                if (byteSpan[byteSpan.Length - (i + 1)] != bigEndiandTargetString[i]) {return false; }
            }
            return true;
        }
    }
}
