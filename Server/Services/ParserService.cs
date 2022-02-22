using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using num2words;

namespace Server.Services
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
            string encoded;
            try
            {
                encoded = WordsEncoder.DoubleToCurrency(request.Number);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var status = new Status(StatusCode.Internal, "An error occurred when converting the number to words");
                throw new RpcException(status);
            }

            var response = new WordsResponse
            {
                Words = encoded
            };

            return Task.FromResult(response);
        }
    }
}