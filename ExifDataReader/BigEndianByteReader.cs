using System;
using System.Collections.Generic;
using System.Text;
using System.Buffers.Binary;

namespace ExifDataReader
{
    class BigEndianByteReader : IByteReader
    {
        public int ReadInt(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadInt32BigEndian(byteSpan);
        public uint ReadUInt(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadUInt32BigEndian(byteSpan);
        public string ReadString(ReadOnlySpan<byte> byteSpan) => Encoding.ASCII.GetString(byteSpan);
        public short ReadShort(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadInt16BigEndian(byteSpan);
        public ushort ReadUShort(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadUInt16BigEndian(byteSpan);

        public long ReadLong(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadInt64BigEndian(byteSpan);

        public ulong ReadULong(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadUInt64BigEndian(byteSpan);

        public float ReadFloat(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadSingleBigEndian(byteSpan);

        public double ReadDouble(ReadOnlySpan<byte> byteSpan) => BinaryPrimitives.ReadDoubleBigEndian(byteSpan);
        public byte ReadUByteFromSpan(ReadOnlySpan<byte> byteSpan) => byteSpan[byteSpan.Length - 1];
        public sbyte ReadByteFromSpan(ReadOnlySpan<byte> byteSpan) => Convert.ToSByte(byteSpan[byteSpan.Length - 1]);
        public uint[] ReadURational(ReadOnlySpan<byte> byteSpan)
        {
            uint numerator = BinaryPrimitives.ReadUInt32BigEndian(byteSpan[0..4]);
            uint denominator = BinaryPrimitives.ReadUInt32BigEndian(byteSpan[4..(byteSpan.Length)]);
            uint[] uRational = { numerator, denominator };
            return uRational;
        }
        public int[] ReadRational(ReadOnlySpan<byte> byteSpan)
        {
            int numerator = BinaryPrimitives.ReadInt32BigEndian(byteSpan[0..4]);
            int denominator = BinaryPrimitives.ReadInt32BigEndian(byteSpan[4..(byteSpan.Length)]);
            int[] Rational = { numerator, denominator };
            return Rational;
        }
        public bool MatchesBigEndianByteString(ReadOnlySpan<byte> bigEndiandTargetString, ReadOnlySpan<byte> byteSpan) => bigEndiandTargetString.SequenceEqual(byteSpan);
    }
}
