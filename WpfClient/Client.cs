using num2words;
using System.Net.Http;
using Grpc.Net.Client;

namespace WpfClient
{
    public class Client
    {
        public Parser.ParserClient ParserClient { get; }
        private const string DefaultServerAddress = "https://localhost:5001";

        public Client(string serverEndpoint)
        {
            var serverEndpoint1 = string.IsNullOrEmpty(serverEndpoint)
                ? DefaultServerAddress
                : serverEndpoint;

            var httpClientHandler = new HttpClientHandler
            {
                // NOTE: Only for dev purposes
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var channel = GrpcChannel.ForAddress(serverEndpoint1, new GrpcChannelOptions
            {
                HttpHandler = httpClientHandler
            });

            ParserClient = new Parser.ParserClient(channel);
        }
    }
}