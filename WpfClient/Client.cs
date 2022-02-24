using System;
using num2words;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;

namespace WpfClient
{
    /// <summary>
    /// Custom client for controlling gRPC communication
    /// </summary>
    public class Client
    {
        private Parser.ParserClient _client;
        private Uri _currentServerEndpoint;
        private readonly Uri _defaultServerAddress = new("https://localhost:9001");

        private readonly HttpClientHandler _defaultHttpClientHandler = new()
        {
            // NOTE: Only for dev purposes
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public Client()
        {
            var channel = GrpcChannel.ForAddress(_defaultServerAddress, new GrpcChannelOptions
            {
                HttpHandler = _defaultHttpClientHandler
            });
            _client = new Parser.ParserClient(channel);
            _currentServerEndpoint = _defaultServerAddress;
        }

        /// <summary>
        /// Retrieves the current server endpoint used by the client
        /// </summary>
        /// <returns>Server endpoint address as a string</returns>
        public string CurrentServerEndpoint()
        {
            return _currentServerEndpoint.ToString();
        }

        /// <summary>
        /// Sets a new server endpoint
        /// </summary>
        /// <param name="server">Uri of the new server endpoint</param>
        public void SetServerEndpoint(Uri server)
        {
            var channel = GrpcChannel.ForAddress(server, new GrpcChannelOptions
            {
                HttpHandler = _defaultHttpClientHandler
            });
            _client = new Parser.ParserClient(channel);
            _currentServerEndpoint = server;
        }

        /// <summary>
        /// Calls the underlying gRPC service for converting a given number into words
        /// </summary>
        /// <param name="number">Number to be converted</param>
        /// <returns>
        /// The number as words or the error message in case the number could not be converted
        /// </returns>
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