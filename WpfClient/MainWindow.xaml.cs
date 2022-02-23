using num2words;
using System;
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
        private Client _client;
        private static readonly Regex Cleaner = new(@"\s+");
        private string _serverEndpoint = "https://127.0.0.1:9001";

        public MainWindow()
        {
            InitializeComponent();
            _client = new Client(_serverEndpoint);
            SetTextServerInput(_serverEndpoint);
            SetTextServerLabel(TextLabels.ServerAddressDefault);
            SetTextMainLabels(TextLabels.LetsConvert, TextLabels.InputNumber);
        }

        private void SetTextMainLabels(string primary, string secondary)
        {
            PrimaryText.Text = primary;
            SecondaryText.Text = secondary;
        }

        private void SetTextServerInput(string address)
        {
            ServerInput.Text = address;
        }

        private void SetTextServerLabel(string message)
        {
            ServerLabel.Text = message;
        }

        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            var input = InputField.Text;
            if (string.IsNullOrEmpty(input))
            {
                SetTextMainLabels(TextLabels.EmptyInput, TextLabels.EnterValid);
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
                SetTextMainLabels(TextLabels.InvalidInput, TextLabels.EnterValid);
                return;
            }

            SetTextMainLabels(TextLabels.SendingRequest, TextLabels.CurrentTime());

            var request = new NumberRequest { Number = number };
            string message;

            try
            {
                var response = await _client.ParserClient.FromNumberToWordsAsync(request);
                message = response == null ? TextLabels.None : TextLabels.CurrencyResponse(response.Words);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Internal)
            {
                message = TextLabels.ServerInternalError;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
            {
                message = TextLabels.ServerUnavailableError;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
            {
                message = TextLabels.ServerDeadlineError;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.FailedPrecondition)
            {
                message = TextLabels.ServerArgumentError;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
            {
                message = TextLabels.ServerArgumentError;
            }

            SetTextMainLabels(message, TextLabels.CurrentTime());
        }

        private void ChangeServerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_serverEndpoint.Equals(ServerInput.Text))
            {
                SetTextServerLabel(TextLabels.ServerAddressUnchanged);
                return;
            }

            Uri newServer;
            try
            {
                newServer = new Uri(ServerInput.Text);
            }
            catch (Exception ex)
            {
                ServerLabel.Text = ex.Message;
                return;
            }

            _serverEndpoint = newServer.ToString();
            _client = new Client(_serverEndpoint);

            SetTextServerLabel(TextLabels.ServerAddressUpdated);
        }
    }
}