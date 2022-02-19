using Xunit;

namespace Server.Test
{
    public class WordsEncoderTests
    {
        // TODO: Rules
        //  * 0-9       : zero, one, two, three, four, five, six, seven, eight, nine
        //  * 11-12     : eleven, twelve
        //  * 10-19     : teen
        //  * 20-29     : twenty
        //  * 30-39     : thirty
        //  * 40-49     : forty
        //  * 50-59     : fifty
        //  * 60-69     : sixty
        //  * 70-79     : seventy
        //  * 80-89     : eighty
        //  * 90-99     : ninety
        //  * 100       : hundred
        //  * 1000      : thousand
        //  * 1000000   : million
        //  * 1000000000: billion

        [Theory]
        [InlineData(0, "zero")]
        [InlineData(1, "one")]
        [InlineData(10, "ten")]
        [InlineData(11, "eleven")]
        [InlineData(12, "twelve")]
        [InlineData(19, "nineteen")]
        [InlineData(25, "twenty-five")]
        [InlineData(42, "forty-two")]
        [InlineData(152, "one hundred fifty-two")]
        [InlineData(1259, "one thousand two hundred fifty-nine")]
        [InlineData(45100, "forty-five thousand one hundred")]
        [InlineData(356137, "three hundred fifty-six thousand one hundred thirty-seven")]
        [InlineData(1234987, "one million two hundred thirty-four thousand nine hundred eighty-seven")]
        [InlineData(999999999,
            "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine")]
        public void Test_FromNumber(long input, string expected)
        {
            var words = WordsEncoder.FromNumber(input);
            Assert.Equal(words, expected);
        }
    }
}