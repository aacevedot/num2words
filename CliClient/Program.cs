using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using num2words;
using System.Net.Http;
using System.Text.RegularExpressions;
using Grpc.Core;

namespace CliClient
{
    internal static class Program
    {
        private static readonly Regex Cleaner = new(@"\s+");
        private static bool _sigint;
        private static bool _sigterm;

        public static async Task Main(string[] args)
        {
            Console.CancelKeyPress += HandleCancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += HandleProcessExit;

            var httpClientHandler = new HttpClientHandler();
            // NOTE: Only for dev purposes. Prod applications should use valid certs.
            httpClientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            // TODO: Allow address changes
            const string server = "https://localhost:5001";
            var channel = GrpcChannel.ForAddress(server, new GrpcChannelOptions
            {
                HttpHandler = httpClientHandler
            });
            var client = new Parser.ParserClient(channel);
            var options = new CallOptions().WithWaitForReady();

            while (true)
            {
                Console.Write("Input a number [enter: Exit]: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) break;

                double number;
                try
                {
                    input = Cleaner.Replace(input, "");
                    number = Convert.ToDouble(input);
                }
                catch (Exception e)
                {
                    Console.WriteLine(input);
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Invalid input, try again!");
                    continue;
                }

                try
                {
                    var request = new NumberRequest { Number = number };
                    var response = await client.FromNumberToWordsAsync(request, options);
                    Console.WriteLine(response.Words);
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Internal)
                {
                    Console.WriteLine("internal server error, please try again!");
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
                {
                    Console.WriteLine("server unavailable, please try later!");
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
                {
                    Console.WriteLine("server timeout, please try again!");
                }
            }
        }

        private static void HandleProcessExit(object sender, EventArgs e)
        {
            if (_sigint) return;
            _sigterm = true;
            Environment.ExitCode = 0;
        }

        private static void HandleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (_sigterm) return;
            _sigint = true;
            Environment.Exit(0);
        }
    }
}