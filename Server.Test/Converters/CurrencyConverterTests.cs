using Server.Converters;
using Xunit;

namespace Server.Test.Converters
{
    /// <summary>
    /// CurrencyConverter unit tests
    /// </summary>
    public class CurrencyConverterTests
    {
        /// <summary>
        /// Test the conversion of a number into currency in words
        /// </summary>
        /// <param name="input">Number to be converted</param>
        /// <param name="expected">Expected currency in words</param>
        [Theory]
        [InlineData(-0.0, "zero dollars")]
        [InlineData(-19, "minus nineteen dollars")]
        [InlineData(-1999.7809, "minus one thousand nine hundred ninety-nine dollars and seventy-eight cents")]
        [InlineData(0, "zero dollars")]
        [InlineData(0.001, "zero dollars")]
        [InlineData(1, "one dollar")]
        [InlineData(25.1, "twenty-five dollars and ten cents")]
        [InlineData(0.01, "zero dollars and one cent")]
        [InlineData(0.02, "zero dollars and two cents")]
        [InlineData(0.09, "zero dollars and nine cents")]
        [InlineData(120.105, "one hundred twenty dollars and ten cents")]
        [InlineData(1.99999, "one dollar and ninety-nine cents")]
        [InlineData(45100, "forty-five thousand one hundred dollars")]
        [InlineData(999999999.99,
            "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]
        public void Test_FromDoubleToCurrency(double input, string expected)
        {
            var words = CurrencyConverter.FromDoubleToCurrency(input);
            Assert.Equal(expected, words);
        }
    }
}