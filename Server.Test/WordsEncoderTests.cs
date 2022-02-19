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
        [InlineData(123456789, "one hundred twenty-three million four hundred fifty-six thousand seven hundred eighty-nine")]
        [InlineData(999999999,
            "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine")]
        public void Test_FromNumber(long input, string expected)
        {
            var words = WordsEncoder.FromNumber(input);
            Assert.Equal(words, expected);
        }
    }
}