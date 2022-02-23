using System;
using System.Windows;
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

        public MainWindow()
        {
            InitializeComponent();
            _client = new Client();
            SetTextServerInput(_client.CurrentServerEndpoint());
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

            var response = await _client.ConvertNumber(number);

            SetTextMainLabels(response, TextLabels.CurrentTime());
        }

        private void ChangeServerButton_Click(object sender, RoutedEventArgs e)
        {
            var current = _client.CurrentServerEndpoint();
            if (current.Equals(ServerInput.Text))
            {
                SetTextServerLabel(TextLabels.ServerAddressUnchanged);
                return;
            }

            Uri newServerEndpoint;
            try
            {
                newServerEndpoint = new Uri(ServerInput.Text);
            }
            catch (Exception ex)
            {
                SetTextServerLabel(ex.Message);
                return;
            }

            _client.SetServerEndpoint(newServerEndpoint);
            
            SetTextServerLabel(TextLabels.ServerAddressUpdated);
        }
    }
}