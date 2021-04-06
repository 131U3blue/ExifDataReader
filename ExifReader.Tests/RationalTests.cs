using NUnit.Framework;
using ExifDataReader;

namespace ExifReader.Tests
{
    public class RationalTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        [TestCase(1, 3, 2, 6, true)]
        [TestCase(1, 3, 2, 5, false)]
        public void CompareRationals_EqualityCheck_ReturnsBoolean(int Numerator1, int Denominator1, int Numerator2, int Denominator2, bool expected)
        {
            var first = new Rational(Numerator1, Denominator1);
            var second = new Rational(Numerator2, Denominator2);
            Assert.AreEqual(Equals(first, second), expected);
        }
        [Test]
        [TestCase(2, 3, 1, 3, false)]
        public void CompareRationals_GreaterThanCheck_ReturnsBoolean(int Numerator1, int Denominator1, int Numerator2, int Denominator2, bool expected)
        {
            var first = new Rational(Numerator1, Denominator1);
            var second = new Rational(Numerator2, Denominator2);
            Assert.AreEqual((first > second), expected);
        }
    }
}