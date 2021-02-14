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
        public uint[] ReadURational(ReadOnlySpan<byte> byteSpan) 
        {
            uint numerator = BinaryPrimitives.ReadUInt32LittleEndian(byteSpan[0..4]);
            uint denominator = BinaryPrimitives.ReadUInt32LittleEndian(byteSpan[4..(byteSpan.Length)]);
            uint[] uRational = { numerator, denominator };
            return uRational;
        }
        public int[] ReadRational(ReadOnlySpan<byte> byteSpan)
        {
            int numerator = BinaryPrimitives.ReadInt32LittleEndian(byteSpan[0..4]);
            int denominator = BinaryPrimitives.ReadInt32LittleEndian(byteSpan[4..(byteSpan.Length)]);
            int[] Rational = { numerator, denominator };
            return Rational;
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
