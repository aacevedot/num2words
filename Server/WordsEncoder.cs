using System;
using System.Collections.Generic;
using System.Globalization;

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

        public static string DoubleToCurrency(double number)
        {
            var words = DoubleToWords(number);
            if (words == null)
            {
                return $"'{number}' cannot be converted to currency";
            }

            // TODO: Refactor suffixes
            var suffix = words[0] == "one" ? "dollar" : "dollars";
            var asCurrency = words[0] + " " + suffix;
            if (words.Length < 2) return asCurrency;

            suffix = words[1] == "one" ? "cent" : "cents";
            asCurrency += " and " + words[1] + " " + suffix;
            return asCurrency;
        }

        public static string[] DoubleToWords(double number)
        {
            // TODO: Check if this rounding approach makes sense
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
            if (SingularCases.TryGetValue((int)number, out var asWords))
            {
                return asWords;
            }

            var str = number.ToString();
            var digits = str.Length;
            int offset;
            string suffix;

            switch (digits)
            {
                case 2:
                    offset = digits - 1;
                    suffix = "tens";
                    break;
                case 3:
                    offset = digits - 2;
                    suffix = "hundred";
                    break;
                case >= 4 and <= 6:
                    offset = digits - 3;
                    suffix = "thousand";
                    break;
                case >= 7 and <= 9:
                    offset = digits - 6;
                    suffix = "million";
                    break;
                case >= 10 and <= 12:
                    offset = digits - 9;
                    suffix = "billion";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(number), "the number is too long");
            }

            var head = Convert.ToInt32(str[..offset]);
            var tail = Convert.ToInt32(str[offset..]);
            if (suffix.Equals("tens"))
            {
                if (TensCases.TryGetValue(head, out var ty))
                {
                    asWords = tail > 0 ? ty + "-" + SingularCases[tail] : ty;
                    return asWords;
                }

                asWords = SingularCases[tail] + "teen";
                return asWords;
            }

            asWords = IntegerToWords(head) + " " + suffix;
            if (tail == 0) return asWords;

            asWords += " " + IntegerToWords(tail);
            return asWords;
        }
    }
}