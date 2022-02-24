using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace CliClient
{
    public class CliClientOptions : BaseAttribute
    {
        [Option('s', "server", Required = true,
            HelpText = "Server endpoint (example: https://127.0.0.1:9001)")]
        public Uri ServerAddress { get; set; }
    }

    public static class Arguments
    {
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

                Environment.Exit(1);
            });
            return options;
        }
    }
}