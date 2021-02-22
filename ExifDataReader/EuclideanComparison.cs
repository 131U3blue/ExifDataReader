using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExifDataReader
{
    class EuclideanComparison
    {
        public static int HighestCommonFactor(int a, int b)
        {
            while (b != 0) {
                int i = b;
                b = a % b;
                a = i;
                }
            return a;
        }

    }
}
