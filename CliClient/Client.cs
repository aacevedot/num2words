using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using num2words;

namespace CliClient
{
    /// <summary>
    /// Custom client for controlling gRPC communication
    /// </summary>
    public class Client
    {
        private readonly Parser.ParserClient _client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server">Server endpoint</param>
        public Client(Uri server)
        {
            var httpClientHandler = new HttpClientHandler();
            // NOTE: Only for dev purposes. Prod applications should use valid certs.
            httpClientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress(server, new GrpcChannelOptions
            {
                HttpHandler = httpClientHandler
            });

            _client = new Parser.ParserClient(channel);
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
            string output;

            try
            {
                var request = new NumberRequest { Number = number };
                var response = await _client.FromNumberToWordsAsync(request, new CallOptions().WithWaitForReady());
                output = response.Words;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Internal)
            {
                output = "internal server error, please try again!";
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                output = "server unavailable, please try again later!";
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
            {
                output = "server timeout, please try again!";
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition)
            {
                output = "server rejected the request, please try again!";
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
            {
                output = "server rejected the request, please try again!";
            }

            return output;
        }
    }
}