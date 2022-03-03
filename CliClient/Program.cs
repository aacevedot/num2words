using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

            var options = Arguments.Parse(args);
            if (options == null)
            {
                Environment.Exit(1);
            }

            var client = new Client(options.ServerAddress);

            while (true)
            {
                Console.Write("Input a number [exit: enter]: ");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) break;

                double number;
                try
                {
                    input = Cleaner.Replace(input, "");
                    number = Convert.ToDouble(input);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input, try again!");
                    continue;
                }

                var response = await client.ConvertNumber(number);
                Console.WriteLine(response);
            }

            Environment.Exit(0);
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