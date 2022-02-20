using Grpc.Net.Client;
using num2words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Parser.ParserClient? _client;

        public MainWindow()
        {
            InitializeComponent();
            Connect();
        }

        private void Connect()
        {
            const string server = "https://localhost:5001";
            TextHolder.Text = $"Connecting to: {server}";

            try
            {
                var clientHandler = new HttpClientHandler();
                clientHandler.UseProxy = false;
                clientHandler.ServerCertificateCustomValidationCallback =
                    (sender, cert, chain, sslPolicyErrors) => true;
                var httpClient = new HttpClient(clientHandler);

                var channel = GrpcChannel.ForAddress(server, new GrpcChannelOptions
                {
                    HttpClient = httpClient
                });
                _client = new Parser.ParserClient(channel);
                TextHolder.Text = $"Connected to: {server}";
            }
            catch (Exception e)
            {
                TextHolder.Text = e.Message;
            }
        }

        private async void convertButton_Click(object sender, RoutedEventArgs e)
        {
            if (_client == null)
            {
                TextHolder.Text = "Not connected!";
                return;
            }

            var number = Convert.ToDouble(InputField.Text);

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
            catch (Exception error)
            {
                message = error.Message;
            }

            TextHolder.Text = message;
        }
    }
}