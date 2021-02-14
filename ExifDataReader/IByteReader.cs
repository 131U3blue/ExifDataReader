using System;
using System.Collections.Generic;
using System.Text;

namespace ExifDataReader
{
    interface IByteReader
    {
        int ReadInt(ReadOnlySpan<byte> byteSpan);
        uint ReadUInt(ReadOnlySpan<byte> byteSpan);
        string ReadString(ReadOnlySpan<byte> byteSpan);
        short ReadShort(ReadOnlySpan<byte> byteSpan);
        ushort ReadUShort(ReadOnlySpan<byte> byteSpan);
        long ReadLong(ReadOnlySpan<byte> byteSpan);
        ulong ReadULong(ReadOnlySpan<byte> byteSpan);
        float ReadFloat(ReadOnlySpan<byte> byteSpan);
        double ReadDouble(ReadOnlySpan<byte> byteSpan);
        byte ReadUByteFromSpan(ReadOnlySpan<byte> byteSpan);
        sbyte ReadByteFromSpan(ReadOnlySpan<byte> byteSpan);
        uint[] ReadURational(ReadOnlySpan<byte> byteSpan);
        int[] ReadRational(ReadOnlySpan<byte> byteSpan);
        bool MatchesBigEndianByteString(ReadOnlySpan<byte> bigEndiandTargetString, ReadOnlySpan<byte> byteSpan);
    }
}
