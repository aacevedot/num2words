using Xunit;

namespace Server.Test
{
    public class UnitTest1
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
        public void Test_FromNumber(long input, string expected)
        {
            var words = WordsEncoder.FromNumber(input);
            Assert.Equal(words, expected);
        }
    }
}