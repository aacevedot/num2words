using Grpc.Net.Client;
using num2words;
using System;
using System.Globalization;
using System.Net.Http;
using System.Windows;
using Grpc.Core;
using System.Text.RegularExpressions;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Parser.ParserClient? _client;
        private static readonly Regex Cleaner = new(@"\s+");

        public MainWindow()
        {
            InitializeComponent();
            // TODO: Launch the connection async
            Connect();
        }

        private void Connect()
        {
            // TODO: Check if the address should be provided by the user
            const string server = "https://localhost:5001";

            var httpClientHandler = new HttpClientHandler();
            // NOTE: Only for dev purposes. Prod applications should use valid certs.
            httpClientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            try
            {
                var channel = GrpcChannel.ForAddress(server, new GrpcChannelOptions
                {
                    HttpHandler = httpClientHandler
                });
                _client = new Parser.ParserClient(channel);
                PrimaryText.Text = "Let's convert your number to currency! 💸";
                SecondaryText.Text = "Input a number in the form below 👇";
            }
            catch (Exception e)
            {
                // TODO: This should not be visible!
                PrimaryText.Text = e.Message;
            }
        }

        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            if (_client == null)
            {
                // TODO: Check if this makes sense! (Would the client ever be null???)
                // TODO: If yes, then trying to reconnect here might be good idea
                PrimaryText.Text = "Connection failed! 😱";
                return;
            }

            var input = InputField.Text;
            if (string.IsNullOrEmpty(input))
            {
                PrimaryText.Text = "Your input cannot be empty! 😒";
                SecondaryText.Text = "Please, provide a valid number! 👇";
                return;
            }

            double number;
            try
            {
                input = Cleaner.Replace(input, "");
                number = Convert.ToDouble(input);
            }
            catch (FormatException)
            {
                PrimaryText.Text = "Invalid input! 😵";
                SecondaryText.Text = "Please, provide a valid number! 👇";
                return;
            }

            if (number >= 1E+12)
            {
                PrimaryText.Text = "Number too long! 😥";
                SecondaryText.Text = "Come on! You do not have that much money. Try again with a realistic amount! 😜";
                return;
            }

            PrimaryText.Text = "Sending request... 📡";
            SecondaryText.Text = $"⏲ {DateTime.Now.ToString(CultureInfo.InvariantCulture)}";

            var req = new NumberRequest
            {
                Number = number
            };

            // TODO: There is still an unhandled exception when the number is too big!

            string message;
            try
            {
                var res = await _client.FromNumberToWordsAsync(req);
                message = $"{res.Words} 🤑";
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Internal)
            {
                message = "An internal server error was detected! 🤯 \n Please, try again later!";
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                message = "The server is unavailable 😓 \n Please, try again later!";
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
            {
                message = "It took too long, the server is not responding! 💀 \nPlease, try again!";
            }

            PrimaryText.Text = message;
            SecondaryText.Text = $"⏲ {DateTime.Now.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}