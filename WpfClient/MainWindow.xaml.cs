using Grpc.Net.Client;
using num2words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                PrimaryText.Text = "Let's convert your number to currency!";
                SecondaryText.Text = "Input a number in the form below";
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
                PrimaryText.Text = "Connection failed!";
                return;
            }

            var input = InputField.Text;
            if (string.IsNullOrEmpty(input))
            {
                PrimaryText.Text = "Your input cannot be empty!";
                SecondaryText.Text = "Please, provide a valid number!";
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
                PrimaryText.Text = "Invalid input!";
                SecondaryText.Text = "Please, provide a valid number!";
                return;
            }

            PrimaryText.Text = "Sending request...";
            SecondaryText.Text = DateTime.Now.ToString();

            var req = new NumberRequest
            {
                Number = number
            };

            string message;
            try
            {
                var res = await _client.FromNumberToWordsAsync(req);
                message = res.Words;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Internal)
            {
                message = "An internal server error was detected. Please, try again!";
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                message = "The server is unavailable. Please, try again later!";
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
            {
                message = "It took too long, the server is not responding. Please, try again!";
            }

            PrimaryText.Text = message;
            SecondaryText.Text = DateTime.Now.ToString();
        }
    }
}