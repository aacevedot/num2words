namespace Server.Converters
{
    /// <summary>
    /// Representation of a currency
    /// </summary>
    /// <remarks>
    /// Eventually, this is meant to handle other currencies in the future
    /// besides dollars 
    /// </remarks>
    public struct Currency
    {
        /// <summary>
        /// Amount naming as singular
        /// </summary>
        public string AmountSingular { get; init; }

        /// <summary>
        /// Amount naming as plural
        /// </summary>
        public string AmountPlural { get; init; }

        /// <summary>
        /// Coins naming as singular
        /// </summary>
        public string CoinsSingular { get; init; }

        /// <summary>
        /// Coins naming as plural
        /// </summary>
        public string CoinsPlural { get; init; }

        /// <summary>
        /// Currency symbol
        /// </summary>
        public string Symbol { get; init; }
    }
}