using System;
using Xunit;

namespace CliClient.Test
{
    /// <summary>
    /// Arguments test cases
    /// </summary>
    public class ArgumentsTests
    {
        /// <summary>
        /// Tests non-existing (null) arguments
        /// </summary>
        [Fact]
        public void TestParse_NullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => _ = Arguments.Parse(null));
        }

        /// <summary>
        /// Tests missing required arguments 
        /// </summary>
        [Fact]
        public void TestParse_MissingRequired()
        {
            var args = new[] { "--fake", "argument" };
            var parsed = Arguments.Parse(args);
            Assert.Null(parsed);
        }

        /// <summary>
        /// Tests invalid arguments' values
        /// </summary>
        [Fact]
        public void TestParse_InvalidArgumentValue()
        {
            var args = new[] { "--server", "#+ä!" };
            var parsed = Arguments.Parse(args);
            Assert.Null(parsed);
        }

        /// <summary>
        /// Tests successful arguments parsing
        /// </summary>
        [Fact]
        public void TestParse_ValidArguments()
        {
            var args = new[] { "--server", "https://127.0.0.1:9001" };
            var parsed = Arguments.Parse(args);
            Assert.NotNull(parsed);
            var expected = new Uri("https://127.0.0.1:9001");
            Assert.Equal(expected, parsed.ServerAddress);
        }
    }
}