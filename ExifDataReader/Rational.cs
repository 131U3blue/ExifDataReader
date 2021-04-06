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
        public int HighestCommonFactor { get; }
        public Rational(int numerator, int denominator)
        {
            if (denominator == 0) {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }
            Numerator = numerator;
            Denominator = denominator;
            HighestCommonFactor = EuclideanAlgorithm.HighestCommonFactor(Numerator, Denominator);

        }
        public override bool Equals(object obj)
        {
            if (obj is Rational) return Equals((Rational)obj);
            else return false;
        }
        public bool Equals(Rational other)
            => EuclideanAlgorithm.SimplestRational(other).Numerator == EuclideanAlgorithm.SimplestRational(this).Numerator && 
            EuclideanAlgorithm.SimplestRational(other).Denominator == EuclideanAlgorithm.SimplestRational(this).Denominator;
        public override int GetHashCode() => this.HighestCommonFactor;
        public static bool operator ==(Rational left, Rational right)
            => Equals(left, right);
        public static bool operator !=(Rational left, Rational right)
            => !Equals(left, right);
        public static bool operator >(Rational left, Rational right)
            => EuclideanAlgorithm.HighestCommonFactor(left.Numerator, left.Denominator) > EuclideanAlgorithm.HighestCommonFactor(right.Numerator, right.Denominator);
        public static bool operator <(Rational left, Rational right)
            => EuclideanAlgorithm.HighestCommonFactor(left.Numerator, left.Denominator) < EuclideanAlgorithm.HighestCommonFactor(right.Numerator, right.Denominator);
    }
}
