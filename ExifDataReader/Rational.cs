using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExifDataReader
{
    public readonly struct Rational : IEquatable<Rational>
    {
        public int Numerator { get; }
        public int Denominator { get; }
        public Rational(int numerator, int denominator)
        {
            if (denominator == 0) {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }
            Numerator = numerator;
            Denominator = denominator;
        }
        public override bool Equals(object obj)
        {
            if (obj is Rational) return Equals(obj);
            else return false;
        }
        public bool Equals(Rational other)
            => EuclideanComparison.HighestCommonFactor(other.Numerator, other.Denominator) == EuclideanComparison.HighestCommonFactor(this.Numerator, this.Denominator);
        public override int GetHashCode() => EuclideanComparison.HighestCommonFactor(Numerator, Denominator);
    }
    public readonly struct URational : IEquatable<URational>
    {
        public uint Numerator { get; }
        public uint Denominator { get; }
        public URational(uint numerator, uint denominator)
        {
            if (denominator == 0) {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }
            Numerator = numerator;
            Denominator = denominator;
        }
        public override bool Equals(object obj)
        {
            if (obj is URational) return Equals(obj);
            else return false;
        }
        public bool Equals(URational other)
            => EuclideanComparison.HighestCommonFactor((int)other.Numerator, (int)other.Denominator) == EuclideanComparison.HighestCommonFactor((int)this.Numerator, (int)this.Denominator);
        public override int GetHashCode() => EuclideanComparison.HighestCommonFactor((int)Numerator, (int)Denominator);

        public static bool operator ==(URational left, URational right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(URational left, URational right)
        {
            return !(left.Equals(right));
        }

        public 
    }

}
