using System;
using System.Globalization;

namespace WpfClient
{
    /// <summary>
    /// Text label messages
    /// </summary>
    public static class TextLabels
    {
        /// <summary>
        /// Serves as an "empty" message
        /// </summary>
        public const string None = "🍃";

        /// <summary>
        /// Serves as the main presentation message 
        /// </summary>
        public const string LetsConvert = "Let's convert your number to currency! 💸";

        /// <summary>
        /// Serves as the main guide sentence
        /// </summary>
        public const string InputNumber = "Input a number in the form below 👇";

        /// <summary>
        /// Serves as a warning message in case an input was not provided
        /// </summary>
        public const string EmptyInput = "Your input cannot be empty! 😒";

        /// <summary>
        /// Serves as a warning message in case an invalid input was provided
        /// </summary>
        public const string InvalidInput = "Invalid input! 😵";

        /// <summary>
        /// Serves as a guide message for providing a valid input
        /// </summary>
        public const string EnterValid = "Please, provide a valid number! 👇";

        /// <summary>
        /// Serves as a warning message in case a too large input was provided
        /// </summary>
        public const string InputTooLong = "Number too long! 😥";

        /// <summary>
        /// Serves as a guide message for providing a realistic input
        /// </summary>
        public const string RealisticAmount = "Try again with a realistic amount! 😜";

        /// <summary>
        /// Serves as the main message when sending a request
        /// </summary>
        public const string SendingRequest = "Sending request... 📡";

        /// <summary>
        /// Serves as an error message when an unexpected server error is detected
        /// </summary>
        public const string ServerInternalError = "An internal server error was detected! 🤯";

        /// <summary>
        /// Serves as an error message when the unavailability of the server is detected
        /// </summary>
        public const string ServerUnavailableError = "Server unavailable 😓";

        /// <summary>
        /// Serves as an error message when the server does not respond in a predefined timeframe
        /// </summary>
        public const string ServerDeadlineError = "It took too long, the server is not responding! 💀";

        /// <summary>
        /// Serves as an error message when the server rejects the request
        /// </summary>
        public const string ServerArgumentError = "The server rejected your input! 😶";

        /// <summary>
        /// Serves as a message for informing that the default server address is in use
        /// </summary>
        public const string ServerAddressDefault = "Using default server address:";

        /// <summary>
        /// Serves as a message for informing that the server endpoint address was updated
        /// </summary>
        public const string ServerAddressUpdated = "Server address updated!";

        /// <summary>
        /// Serves as a message for informing that a new server address matches the current endpoint
        /// </summary>
        public const string ServerAddressUnchanged = "Already using this server address!";

        /// <summary>
        /// Serves as a method for enriching the current time as text
        /// </summary>
        /// <returns></returns>
        public static string CurrentTime()
        {
            return $"⏲ {DateTime.Now.ToString(CultureInfo.InvariantCulture)}";
        }

        /// <summary>
        /// Serves as a method for enriching a given currency in words
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static string CurrencyResponse(string currency)
        {
            return string.IsNullOrEmpty(currency) ? None : $"{currency} 💵";
        }
    }
}