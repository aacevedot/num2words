using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Moq;
using num2words;
using Xunit;

namespace WpfClient.Test
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

    public class ClientTests
    {
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
                    null,
                    null,
                    CancellationToken.None
                )).Returns(mockedResponse);

            var client = new Client(mockedClient.Object);
            var words = await client.ConvertNumber(42);
            Assert.Equal(TextLabels.CurrencyResponse("Forty-two"), words);
        }

        [Theory]
        [InlineData(StatusCode.Internal, TextLabels.ServerInternalError)]
        [InlineData(StatusCode.Unavailable, TextLabels.ServerUnavailableError)]
        [InlineData(StatusCode.DeadlineExceeded, TextLabels.ServerDeadlineError)]
        [InlineData(StatusCode.FailedPrecondition, TextLabels.ServerArgumentError)]
        [InlineData(StatusCode.InvalidArgument, TextLabels.ServerArgumentError)]
        [InlineData(StatusCode.Unknown, null)]
        public async Task TestClient_ConvertNumber_Errors(StatusCode status, string expectedOutput)
        {
            var mockedResponse = ResponseTestHelpers.ErrorResponse<WordsResponse>(status);
            var mockedClient = new Mock<Parser.ParserClient>();
            mockedClient
                .Setup(m => m.FromNumberToWordsAsync(
                    It.IsAny<NumberRequest>(),
                    null,
                    null,
                    CancellationToken.None
                )).Returns(mockedResponse);

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