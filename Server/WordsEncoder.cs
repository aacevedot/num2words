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
            var isSingular = SingularCases.TryGetValue((int)number, out var converted);
            if (isSingular)
            {
                return converted;
            }

            converted = "";

            var str = number.ToString();
            var numbers = str.ToArray().Select(c => Convert.ToInt32(c) - 48).ToArray();
            int first;
            int second;

            switch (numbers.Length)
            {
                case 2:
                    first = numbers[0];
                    second = numbers[1];
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
                    first = numbers[0];
                    converted = SingularCases[first] + " hundred";
                    if (number % 100 == 0) break;
                    converted += " " + FromNumber(number % 100);
                    break;
                case 4:
                    first = numbers[0];
                    converted = SingularCases[first] + " thousand";
                    if (number % 1000 == 0) break;
                    converted += " " + FromNumber(number % 1000);
                    break;
                case 5:
                    var two = str[..2];
                    var portion2 = Convert.ToInt64(two);
                    converted = FromNumber(portion2) + " thousand";
                    if (number % 1000 == 0) break;
                    converted += " " + FromNumber(number % 1000);
                    break;
                case 6:
                    var three = str[..3];
                    var portion3 = Convert.ToInt64(three);
                    converted = FromNumber(portion3) + " thousand";
                    if (number % 1000 == 0) break;
                    converted += " " + FromNumber(number % 1000);
                    break;
                case 7:
                    first = numbers[0];
                    converted = SingularCases[first] + " million";
                    if (number % Math.Pow(10, 6) == 0) break;
                    var rest1 = Convert.ToInt64(str[1..]);
                    converted += " " + FromNumber(rest1);
                    break;
                case 8:
                    first = Convert.ToInt32(str[..2]);
                    converted = FromNumber(first) + " million";
                    if (number % Math.Pow(10, 6) == 0) break;
                    var rest2 = Convert.ToInt64(str[2..]);
                    converted += " " + FromNumber(rest2);
                    break;
                case 9:
                    first = Convert.ToInt32(str[..3]);
                    converted = FromNumber(first) + " million";
                    if (number % Math.Pow(10, 6) == 0) break;
                    var rest3 = Convert.ToInt64(str[3..]);
                    converted += " " + FromNumber(rest3);
                    break;
            }


            return converted;
        }
    }
}