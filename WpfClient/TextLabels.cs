using System;
using System.Globalization;

namespace WpfClient
{
    public static class TextLabels
    {
        public const string None = "🍃";
        public const string LetsConvert = "Let's convert your number to currency! 💸";
        public const string InputNumber = "Input a number in the form below 👇";
        public const string EmptyInput = "Your input cannot be empty! 😒";
        public const string InvalidInput = "Invalid input! 😵";
        public const string EnterValid = "Please, provide a valid number! 👇";
        public const string InputTooLong = "Number too long! 😥";
        public const string RealisticAmount = "Try again with a realistic amount! 😜";
        public const string SendingRequest = "Sending request... 📡";

        public const string ServerInternalError = "An internal server error was detected! 🤯";
        public const string ServerUnavailableError = "Server unavailable 😓";
        public const string ServerDeadlineError = "It took too long, the server is not responding! 💀";
        public const string ServerArgumentError = "The server rejected your input! 😶";

        public static string CurrentTime()
        {
            return $"⏲ {DateTime.Now.ToString(CultureInfo.InvariantCulture)}";
        }

        public static string CurrencyResponse(string currency)
        {
            return string.IsNullOrEmpty(currency) ? None : $"{currency} 💵";
        }
    }
}