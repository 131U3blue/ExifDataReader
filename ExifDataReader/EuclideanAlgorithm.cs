using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExifDataReader
{
    class EuclideanAlgorithm {
        public static int HighestCommonFactor(int a, int b) {
            while (b != 0) {
                int i = b;
                b = a % b;
                a = i;
                }
            return a;
        }
        public static Rational SimplestRational(Rational rational) {
            int simpleNum = rational.Numerator / rational.HighestCommonFactor;
            int simpleDen = rational.Denominator / rational.HighestCommonFactor;
            return new Rational(simpleNum, simpleDen);
        }
    }
}
