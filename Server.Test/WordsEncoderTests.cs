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
        [InlineData(25, "twenty-five")]
        [InlineData(451000, "forty-five thousand one hundred dollars")]
        [InlineData(999999999,
            "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine")]
        public void Test_FromNumber(long input, string expected)
        {
            var words = WordsEncoder.FromNumber(input);
            Assert.Equal(words, expected);
        }
    }
}