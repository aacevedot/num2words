using System;
using Xunit;

namespace Server.Test
{
    public class WordsEncoderTests
    {
        [Theory]
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
        [InlineData(999999999.99, "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]
        public void Test_DoubleToCurrency(double input, string expected)
        {
            var words = WordsEncoder.DoubleToCurrency(input);
            Assert.Equal(expected, words);
        }
        
        [Theory]
        [InlineData(12.12, new[] { "twelve", "twelve" })]
        public void Test_DoubleToWords(double input, string[] expected)
        {
            var words = WordsEncoder.DoubleToWords(input);
            Assert.Equal(expected, words);
        }

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "one")]
        [InlineData(10, "ten")]
        [InlineData(11, "eleven")]
        [InlineData(12, "twelve")]
        [InlineData(19, "nineteen")]
        [InlineData(17, "seventeen")]
        [InlineData(16, "sixteen")]
        [InlineData(25, "twenty-five")]
        [InlineData(42, "forty-two")]
        [InlineData(80, "eighty")]
        [InlineData(50, "fifty")]
        [InlineData(109, "one hundred nine")]
        [InlineData(118, "one hundred eighteen")]
        [InlineData(100, "one hundred")]
        [InlineData(120, "one hundred twenty")]
        [InlineData(152, "one hundred fifty-two")]
        [InlineData(1259, "one thousand two hundred fifty-nine")]
        [InlineData(45100, "forty-five thousand one hundred")]
        [InlineData(100000, "one hundred thousand")]
        [InlineData(112000, "one hundred twelve thousand")]
        [InlineData(711010, "seven hundred eleven thousand ten")]
        [InlineData(356137, "three hundred fifty-six thousand one hundred thirty-seven")]
        [InlineData(1000000, "one million")]
        [InlineData(1010012, "one million ten thousand twelve")]
        [InlineData(1234987, "one million two hundred thirty-four thousand nine hundred eighty-seven")]
        [InlineData(12345678, "twelve million three hundred forty-five thousand six hundred seventy-eight")]
        [InlineData(13089050, "thirteen million eighty-nine thousand fifty")]
        [InlineData(123456789,
            "one hundred twenty-three million four hundred fifty-six thousand seven hundred eighty-nine")]
        [InlineData(999999999,
            "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine")]
        public void Test_IntegerToWords(long input, string expected)
        {
            var words = WordsEncoder.IntegerToWords(input);
            Assert.Equal(expected, words);
        }
    }
}