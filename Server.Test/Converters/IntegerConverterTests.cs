using System;
using Server.Converters;
using Xunit;

namespace Server.Test.Converters
{
    public class IntegerConverterTests
    {
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
        [InlineData(9999999999,
            "nine billion nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine")]
        [InlineData(99999999999,
            "ninety-nine billion nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine")]
        [InlineData(999999999999,
            "nine hundred ninety-nine billion nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine")]
        [InlineData(9999999999999, null)]
        [InlineData(99999999999999, null)]
        [InlineData(999999999999999, null)]
        [InlineData(9223372036854775807, null)]
        public void Test_IntegerToWords(long input, string expected)
        {
            if (string.IsNullOrEmpty(expected))
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => IntegerConverter.IntegerToWords(input));
                return;
            }

            var words = IntegerConverter.IntegerToWords(input);
            Assert.Equal(expected, words);
        }
    }
}