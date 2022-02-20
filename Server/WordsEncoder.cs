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

        private static readonly Dictionary<int, string> Magnitudes = new()
        {
            [1] = "hundred",
            [2] = "thousand",
            [3] = "million",
            [4] = "billion",
        };

        public static string FromNumber(long number)
        {
            var integer = (int)number;
            if (SingularCases.TryGetValue(integer, out var converted))
            {
                return converted;
            }

            var str = number.ToString();
            var len = str.Length;
            int head;
            int tail;
            int digits;

            switch (len)
            {
                case 2:
                    digits = len - 1;
                    head = Convert.ToInt32(str[..digits]);
                    tail = Convert.ToInt32(str[digits..]);
                    if (!TensCases.TryGetValue(head, out var tens))
                    {
                        converted = SingularCases[tail] + "teen";
                    }
                    else
                    {
                        converted = tail > 0
                            ? tens + "-" + SingularCases[tail]
                            : tens;
                    }

                    break;
                case 3:
                    digits = len - 2;
                    head = Convert.ToInt32(str[..digits]);
                    converted = FromNumber(head) + " hundred";
                    tail = Convert.ToInt32(str[digits..]);
                    if (tail == 0) break;
                    converted += " " + FromNumber(tail);
                    break;
                case 4:
                case 5:
                case 6:
                    digits = len - 3;
                    head = Convert.ToInt32(str[..digits]);
                    converted = FromNumber(head) + " thousand";
                    tail = Convert.ToInt32(str[digits..]);
                    if (tail == 0) break;
                    converted += " " + FromNumber(tail);
                    break;
                case 7:
                case 8:
                case 9:
                    digits = len - 6;
                    head = Convert.ToInt32(str[..digits]);
                    converted = FromNumber(head) + " million";
                    tail = Convert.ToInt32(str[digits..]);
                    if (tail == 0) break;
                    converted += " " + FromNumber(tail);
                    break;
            }


            return converted;
        }
    }
}