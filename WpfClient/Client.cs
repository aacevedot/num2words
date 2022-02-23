using System;
using num2words;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;

namespace WpfClient
{
    public class Client
    {
        private Parser.ParserClient _client;
        private Uri _currentServerEndpoint;
        private readonly Uri _defaultServerAddress = new("https://localhost:9001");

        private readonly HttpClientHandler _defaultHttpClientHandler = new HttpClientHandler
        {
            // NOTE: Only for dev purposes
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        public Client()
        {
            var channel = GrpcChannel.ForAddress(_defaultServerAddress, new GrpcChannelOptions
            {
                HttpHandler = _defaultHttpClientHandler
            });
            _client = new Parser.ParserClient(channel);
            _currentServerEndpoint = _defaultServerAddress;
        }

        public string CurrentServerEndpoint()
        {
            return _currentServerEndpoint.ToString();
        }

        public void SetServerEndpoint(Uri server)
        {
            var channel = GrpcChannel.ForAddress(server, new GrpcChannelOptions
            {
                HttpHandler = _defaultHttpClientHandler
            });
            _client = new Parser.ParserClient(channel);
            _currentServerEndpoint = server;
        }

        public async Task<string> ConvertNumber(double number)
        {
            var request = new NumberRequest { Number = number };
            string output;

            try
            {
                var response = await _client.FromNumberToWordsAsync(request);
                output = response == null
                    ? TextLabels.None
                    : TextLabels.CurrencyResponse(response.Words);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Internal)
            {
                output = TextLabels.ServerInternalError;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                output = TextLabels.ServerUnavailableError;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
            {
                output = TextLabels.ServerDeadlineError;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition)
            {
                output = TextLabels.ServerArgumentError;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
            {
                output = TextLabels.ServerArgumentError;
            }

            return output;
        }
    }
}