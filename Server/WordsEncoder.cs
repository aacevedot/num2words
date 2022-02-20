using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static string DoubleToCurrency(double number)
        {
            var words = DoubleToWords(number);
            if (words == null)
            {
                return $"'{number}' cannot be converted to currency";
            }

            var suffix = words[0] == "one" ? "dollar" : "dollars";
            var asCurrency = words[0] + " " + suffix;
            if (words.Length < 2) return asCurrency;

            suffix = words[1] == "one" ? "cent" : "cents";
            asCurrency += " and " + words[1] + " " + suffix;
            return asCurrency;
        }

        public static string[] DoubleToWords(double number)
        {
            number = Math.Round(number, 2, MidpointRounding.ToZero);
            var str = number.ToString(CultureInfo.InvariantCulture);
            var numberSegments = str.Split('.');
            var totalSegments = numberSegments.Length;

            if (totalSegments == 0) return null;

            var converted = new List<string>();

            var numberBeforeComma = Convert.ToInt64(numberSegments[0]);
            var numberAsWordsBeforeComma = "";
            if (numberBeforeComma < 0)
            {
                numberAsWordsBeforeComma += "minus ";
                numberBeforeComma = Math.Abs(numberBeforeComma);
            }
            numberAsWordsBeforeComma += IntegerToWords(numberBeforeComma);

            converted.Add(numberAsWordsBeforeComma);

            if (totalSegments < 2) return converted.ToArray();
            
            var numberAfterComma = Convert.ToInt64(numberSegments[1]);

            if (!numberSegments[1].StartsWith("0") && numberAfterComma < 10)
            {
                numberAfterComma *= 10;
            }

            var numberAsWordsAfterComma = IntegerToWords(numberAfterComma);
            converted.Add(numberAsWordsAfterComma);

            return converted.ToArray();
        }

        public static string IntegerToWords(long number)
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
                    converted = IntegerToWords(head) + " hundred";
                    tail = Convert.ToInt32(str[digits..]);
                    if (tail == 0) break;
                    converted += " " + IntegerToWords(tail);
                    break;
                case 4:
                case 5:
                case 6:
                    digits = len - 3;
                    head = Convert.ToInt32(str[..digits]);
                    converted = IntegerToWords(head) + " thousand";
                    tail = Convert.ToInt32(str[digits..]);
                    if (tail == 0) break;
                    converted += " " + IntegerToWords(tail);
                    break;
                case 7:
                case 8:
                case 9:
                    digits = len - 6;
                    head = Convert.ToInt32(str[..digits]);
                    converted = IntegerToWords(head) + " million";
                    tail = Convert.ToInt32(str[digits..]);
                    if (tail == 0) break;
                    converted += " " + IntegerToWords(tail);
                    break;
            }

            return converted;
        }
    }
}