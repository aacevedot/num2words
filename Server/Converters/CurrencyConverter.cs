using System;
using System.Globalization;

namespace Server.Converters
{
    public static class CurrencyConverter
    {
        private static readonly Currency Currency = new()
        {
            AmountSingular = "dollar",
            AmountPlural = "dollars",
            CoinsSingular = "cent",
            CoinsPlural = "cents",
            Symbol = "$"
        };

        public static string FromDoubleToCurrency(double number)
        {
            // TODO: Check if this rounding approach makes sense
            number = Math.Round(number, 2, MidpointRounding.ToZero);

            var segments = SplitNumberInSegments(number);
            var totalSegments = segments.Length;

            // amount segment
            var segment = segments[0];
            var amount = Convert.ToInt64(segment);
            var amountAsWords = IntegerConverter.IntegerToWords(amount);

            var suffix = amountAsWords.Equals("one") ? Currency.AmountSingular : Currency.AmountPlural;
            var asCurrency = amountAsWords + " " + suffix;

            if (totalSegments < 2)
            {
                return asCurrency;
            }

            // coins segment
            segment = segments[1];
            var coins = Convert.ToInt64(segment);
            coins = FixTens(segment, coins);
            var coinsAsWords = IntegerConverter.IntegerToWords(coins);

            suffix = coinsAsWords.Equals("one") ? Currency.CoinsSingular : Currency.CoinsPlural;
            asCurrency += " and " + coinsAsWords + " " + suffix;

            return asCurrency;
        }

        private static string[] SplitNumberInSegments(double number)
        {
            var str = number.ToString(CultureInfo.InvariantCulture);
            var separator = CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
            var segments = str.Split(separator);
            return segments;
        }

        private static long FixTens(string segment, long number)
        {
            // sanity check for numbers lower than 10 after separator
            return !segment.StartsWith("0") && number < 10 ? number *= 10 : number;
        }
    }
}