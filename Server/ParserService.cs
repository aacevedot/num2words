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
            var transformed = request.Number.ToString();
            var response = new WordsResponse
            {
                Words = $"Your number is: {transformed}"
            };
            return Task.FromResult(response);
        }
    }
}