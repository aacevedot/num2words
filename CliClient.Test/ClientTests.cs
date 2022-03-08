using System.Threading.Tasks;
using Grpc.Core;
using Moq;
using num2words;
using Xunit;

namespace CliClient.Test
{
    internal static class ResponseTestHelpers
    {
        public static AsyncUnaryCall<TResponse> SuccessResponse<TResponse>(TResponse response)
        {
            return new AsyncUnaryCall<TResponse>(
                Task.FromResult(response),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { });
        }

        public static AsyncUnaryCall<TResponse> ErrorResponse<TResponse>(StatusCode statusCode)
        {
            var status = new Status(statusCode, string.Empty);
            return new AsyncUnaryCall<TResponse>(
                Task.FromException<TResponse>(new RpcException(status)),
                Task.FromResult(new Metadata()),
                () => status,
                () => new Metadata(),
                () => { });
        }
    }

    /// <summary>
    /// Client test cases
    /// </summary>
    public class ClientTests
    {
        /// <summary>
        /// Tests successful number conversion
        /// </summary>
        [Fact]
        public async void TestClient_ConvertNumber_Success()
        {
            var mockedResponse = ResponseTestHelpers.SuccessResponse(new WordsResponse
            {
                Words = "Forty-two"
            });
            var mockedClient = new Mock<Parser.ParserClient>();
            mockedClient
                .Setup(m => m.FromNumberToWordsAsync(
                    It.IsAny<NumberRequest>(),
                    It.IsAny<CallOptions>())
                ).Returns(mockedResponse);

            var client = new Client(mockedClient.Object);
            var words = await client.ConvertNumber(42);
            Assert.Equal("Forty-two", words);
        }

        /// <summary>
        /// Tests erroneous number conversion
        /// </summary>
        /// <param name="status">RPC status</param>
        /// <param name="expectedOutput">Expected output</param>
        /// <remarks>
        /// When the expected output is null or empty it means that a RCPException should be thrown
        /// </remarks>
        [Theory]
        [InlineData(StatusCode.Internal, "internal server error, please try again!")]
        [InlineData(StatusCode.Unavailable, "server unavailable, please try again later!")]
        [InlineData(StatusCode.DeadlineExceeded, "server timeout, please try again!")]
        [InlineData(StatusCode.FailedPrecondition, "server rejected the request, please try again!")]
        [InlineData(StatusCode.InvalidArgument, "server rejected the request, please try again!")]
        [InlineData(StatusCode.Unknown, "")]
        public async Task TestClient_ConvertNumber_Errors(StatusCode status, string expectedOutput)
        {
            var mockedResponse = ResponseTestHelpers.ErrorResponse<WordsResponse>(status);
            var mockedClient = new Mock<Parser.ParserClient>();
            mockedClient
                .Setup(m => m.FromNumberToWordsAsync(
                    It.IsAny<NumberRequest>(),
                    It.IsAny<CallOptions>())
                ).Returns(mockedResponse);

            var client = new Client(mockedClient.Object);

            if (string.IsNullOrEmpty(expectedOutput))
            {
                await Assert.ThrowsAsync<RpcException>(async () => { await client.ConvertNumber(42); });
                return;
            }

            var words = await client.ConvertNumber(42);
            Assert.Equal(expectedOutput, words);
        }
    }
}