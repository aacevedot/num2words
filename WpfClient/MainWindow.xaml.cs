using Grpc.Net.Client;
using num2words;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Mime;
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
        private readonly Client _client;
        private static readonly Regex Cleaner = new(@"\s+");
        private const string ServerEndpoint = "https://localhost:5001";

        public MainWindow()
        {
            InitializeComponent();
            _client = new Client(ServerEndpoint);
            SetLabels(TextLabels.LetsConvert, TextLabels.InputNumber);
        }

        private void SetLabels(string primary, string secondary)
        {
            PrimaryText.Text = primary;
            SecondaryText.Text = secondary;
        }

        private async void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            var input = InputField.Text;
            if (string.IsNullOrEmpty(input))
            {
                SetLabels(TextLabels.EmptyInput, TextLabels.EnterValid);
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
                SetLabels(TextLabels.InvalidInput, TextLabels.EnterValid);
                return;
            }

            SetLabels(TextLabels.SendingRequest, TextLabels.CurrentTime());

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

            SetLabels(message, TextLabels.CurrentTime());
        }
    }
}