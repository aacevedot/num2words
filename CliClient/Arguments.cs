using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace CliClient
{
    public class CliClientOptions : BaseAttribute
    {
        [Option('s', "server", Required = true, Default = "https://localhost:9001",
            HelpText = "Server endpoint (format: https://IP:PORT)")]
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