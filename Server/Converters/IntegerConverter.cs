using System;
using System.Collections.Generic;

namespace Server.Converters
{
    /// <summary>
    /// Integer conversion functionalities
    /// </summary>
    public static class IntegerConverter
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

        /// <summary>
        /// Transform a given integer to words
        /// </summary>
        /// <param name="integer">Integer to be converted</param>
        /// <returns>The integer representation as words</returns>
        public static string IntegerToWords(long integer)
        {
            var words = string.Empty;

            // abstracted from the method below
            // because the unnecessary checking 
            // during the recursive calls
            if (integer < 0)
            {
                words += "minus ";
                integer = Math.Abs(integer);
            }

            words += ToWords(integer);
            return words;
        }

        private static string ToWords(long integer)
        {
            if (SingularNumbers.TryGetValue((int)integer, out var asWords))
            {
                return asWords;
            }

            var str = integer.ToString();
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
                    throw new ArgumentOutOfRangeException(nameof(integer), "the number is too long");
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