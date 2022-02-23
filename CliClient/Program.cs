using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using num2words;
using System.Net.Http;
using System.Text.RegularExpressions;
using CommandLine;
using CommandLine.Text;
using Grpc.Core;
using Parser = num2words.Parser;

namespace CliClient
{
    internal class CliClientOptions : BaseAttribute
    {
        [Option('s', "server", Required = true, Default = "https://localhost:9001",
            HelpText = "Server endpoint (format: https://IP:PORT)")]
        public Uri ServerAddress { get; set; }
    }

    internal static class Program
    {
        private static readonly Regex Cleaner = new(@"\s+");
        private static bool _sigint;
        private static bool _sigterm;

        public static async Task Main(string[] args)
        {
            Console.CancelKeyPress += HandleCancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit += HandleProcessExit;

            CliClientOptions options = null;

            var parser = new CommandLine.Parser();

            var arguments = parser.ParseArguments<CliClientOptions>(args);
            arguments.WithParsed(parsed => options = parsed);
            arguments.WithNotParsed(_ =>
            {
                var help = new HelpText
                {
                    AutoVersion = false,
                    AddDashesToOption = true
                };
                help = HelpText.DefaultParsingErrorsHandler(arguments, help);
                help.AddOptions(arguments);
                Console.Error.Write(help);

                Environment.Exit(1);
            });

            var httpClientHandler = new HttpClientHandler();
            // NOTE: Only for dev purposes. Prod applications should use valid certs.
            httpClientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress(options.ServerAddress.ToString(), new GrpcChannelOptions
            {
                HttpHandler = httpClientHandler
            });
            var client = new Parser.ParserClient(channel);
            var requestOptions = new CallOptions().WithWaitForReady();

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
                    var response = await client.FromNumberToWordsAsync(request, requestOptions);
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
                catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition)
                {
                    Console.WriteLine("server rejected the request, please try again!");
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
                {
                    Console.WriteLine("server rejected the request, please try again!");
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