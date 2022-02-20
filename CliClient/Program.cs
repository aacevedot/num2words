using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using num2words;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CliClient
{
    class Program
    {
        private static readonly Regex Cleaner = new Regex(@"\s+");
        
        public static async Task Main(string[] args)
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
            var httpClient = new HttpClient(clientHandler);

            const string server = "https://localhost:5001";
            var channel = GrpcChannel.ForAddress(server, new GrpcChannelOptions
            {
                HttpClient = httpClient
            });
            var client = new Parser.ParserClient(channel);

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
                    var response = await client.FromNumberToWordsAsync(request);
                    Console.WriteLine(response.Words);
                }
                catch
                {
                    Console.WriteLine("error while communicating with the server, try again!");
                }
            }
        }
    }
}