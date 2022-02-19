using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public static class WordsEncoder
    {
        private static readonly Dictionary<int, string> SingularCases = new()
        {
            [0] = "zero",
            [1] = "one",
            [2] = "two",
            [3] = "three",
            [4] = "four",
            [5] = "five",
            [6] = "six",
            [7] = "seven",
            [8] = "eight",
            [9] = "nine",
            [10] = "ten",
            [11] = "eleven",
            [12] = "twelve",
            [13] = "thirteen",
            [15] = "fifteen",
            [18] = "eighteen",
        };

        private static readonly Dictionary<int, string> TensCases = new()
        {
            [2] = "twenty",
            [3] = "thirty",
            [4] = "forty",
            [5] = "fifty",
            [6] = "sixty",
            [7] = "seventy",
            [8] = "eighty",
            [9] = "ninety",
        };

        public static string FromNumber(long number)
        {
            var integer = (int)number;
            if (SingularCases.TryGetValue(integer, out var converted))
            {
                return converted;
            }

            var str = number.ToString();
            int first;
            int second;
            var len = str.Length;
            int digits;

            switch (len)
            {
                case 2:
                    first = Convert.ToInt32(str[..1]);
                    second = Convert.ToInt32(str[1..]);
                    if (!TensCases.TryGetValue(first, out var tens))
                    {
                        converted = SingularCases[second] + "teen";
                    }
                    else
                    {
                        converted = second > 0
                            ? tens + "-" + SingularCases[second]
                            : tens;
                    }

                    break;
                case 3:
                    first = Convert.ToInt32(str[..1]);
                    converted = SingularCases[first] + " hundred";
                    second = Convert.ToInt32(str[1..]);
                    if (second == 0) break;
                    converted += " " + FromNumber(second);
                    break;
                case 4:
                case 5:
                case 6:
                    digits = len - 3;
                    first = Convert.ToInt32(str[..digits]);
                    converted = FromNumber(first) + " thousand";
                    second = Convert.ToInt32(str[digits..]);
                    if (second == 0) break;
                    converted += " " + FromNumber(second);
                    break;
                case 7:
                case 8:
                case 9:
                    digits = len - 6;
                    first = Convert.ToInt32(str[..digits]);
                    converted = FromNumber(first) + " million";
                    second = Convert.ToInt32(str[digits..]);
                    if (second == 0) break;
                    converted += " " + FromNumber(second);
                    break;
            }


            return converted;
        }
    }
}