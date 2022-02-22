using System;
using System.Collections.Generic;
using System.Globalization;

namespace Server.Converters
{
    public static class CurrencyConverter
    {
        private static readonly Dictionary<int, string> SingularNumbers = new()
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
            [18] = "eighteen"
        };

        private static readonly Dictionary<int, string> TyNumbers = new()
        {
            [2] = "twenty",
            [3] = "thirty",
            [4] = "forty",
            [5] = "fifty",
            [6] = "sixty",
            [7] = "seventy",
            [8] = "eighty",
            [9] = "ninety"
        };

        public static string FromDoubleToCurrency(double number)
        {
            var converted = DoubleToWords(number);

            var dollars = converted[0];
            var suffix = dollars == "one" ? "dollar" : "dollars";
            var asCurrency = dollars + " " + suffix;

            if (converted.Length < 2)
            {
                return asCurrency;
            }

            var cents = converted[1];
            suffix = cents == "one" ? "cent" : "cents";
            asCurrency += " and " + cents + " " + suffix;

            return asCurrency;
        }

        public static string[] DoubleToWords(double number)
        {
            // TODO: Check if this rounding approach makes sense
            number = Math.Round(number, 2, MidpointRounding.ToZero);

            var str = number.ToString(CultureInfo.InvariantCulture);
            var segments = str.Split('.');
            var totalSegments = segments.Length;

            // if a "micro-optimization" is required; then, refactor this
            // for using an array of primitives, e.g., new string[len]
            var converted = new List<string>();
            var words = "";

            var segment = segments[0];
            var asInteger = Convert.ToInt64(segment);

            // sanity check for negative numbers
            if (asInteger < 0)
            {
                words += "minus ";
                asInteger = Math.Abs(asInteger);
            }

            words += IntegerToWords(asInteger);

            converted.Add(words);

            if (totalSegments < 2)
            {
                return converted.ToArray();
            }

            segment = segments[1];
            asInteger = Convert.ToInt64(segment);

            // sanity check for numbers lower than 10 after separator
            if (!segment.StartsWith("0") && asInteger < 10)
            {
                asInteger *= 10;
            }

            words = IntegerToWords(asInteger);
            converted.Add(words);

            return converted.ToArray();
        }

        public static string IntegerToWords(long number)
        {
            if (SingularNumbers.TryGetValue((int)number, out var asWords))
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
                if (TyNumbers.TryGetValue(head, out var ty))
                {
                    asWords = tail > 0 ? ty + "-" + SingularNumbers[tail] : ty;
                    return asWords;
                }

                asWords = SingularNumbers[tail] + "teen";
                return asWords;
            }

            asWords = IntegerToWords(head) + " " + suffix;
            if (tail == 0) return asWords;

            asWords += " " + IntegerToWords(tail);
            return asWords;
        }
    }
}