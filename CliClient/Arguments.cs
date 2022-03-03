using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace CliClient
{
    /// Custom CLI options
    public class CliClientOptions : BaseAttribute
    {
        /// <summary>
        /// Explicit server endpoint
        /// </summary>
        /// <remarks>
        /// The given input will be parsed into Uri
        /// errors while parsing will be end in the rejection
        /// of the provided value
        /// </remarks>
        [Option('s', "server", Required = true,
            HelpText = "Server endpoint (example: https://127.0.0.1:9001)")]
        public Uri ServerAddress { get; set; }
    }


    /// <summary>
    /// Command line arguments parsing functionalities
    /// </summary>
    public static class Arguments
    {
        /// <summary>
        /// Parse the given args into CliClientOptions
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Custom CLI options</returns>
        public static CliClientOptions Parse(IEnumerable<string> args)
        {
            CliClientOptions options = null;
            var parser = new Parser();
            var arguments = parser.ParseArguments<CliClientOptions>(args);
            arguments.WithParsed(parsed => options = parsed);
            arguments.WithNotParsed(_ =>
            {
                var help = new HelpText
                {
                    AutoVersion = false,
                    AddDashesToOption = true
                };
                help = HelpText.DefaultParsingErrorsHandler(arguments, help);
                help.AddOptions(arguments);
                Console.Error.Write(help);
            });
            return options;
        }
    }
}