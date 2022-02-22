using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using num2words;

namespace Server
{
    public class ParserService : Parser.ParserBase
    {
        private readonly ILogger<ParserService> _logger;

        public ParserService(ILogger<ParserService> logger)
        {
            _logger = logger;
        }

        public override Task<WordsResponse> FromNumberToWords(NumberRequest request, ServerCallContext context)
        {
            string converted;
            try
            {
                converted = WordsEncoder.DoubleToCurrency(request.Number);
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex.Message);
                var status = new Status(StatusCode.FailedPrecondition, "number cannot be converted");
                throw new RpcException(status);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError(ex.Message);
                var status = new Status(StatusCode.InvalidArgument, ex.Message);
                throw new RpcException(status);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                var status = new Status(StatusCode.Internal, "critical error while processing the request");
                throw new RpcException(status);
            }

            var response = new WordsResponse
            {
                Words = converted
            };

            return Task.FromResult(response);
        }
    }
}