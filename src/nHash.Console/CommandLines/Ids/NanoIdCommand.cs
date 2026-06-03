using System;
using System.CommandLine;
using System.Collections.Generic;
using nHash.Application.Ids;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Ids;

public class NanoIdCommand(INanoIdService nanoidService, IOutputProvider outputProvider) : INanoIdCommand
{
    public BaseCommand Command => GetFeatureCommand();

    private readonly Option<int> _lengthOption = new("--length", "-l") { Description = "Length of the NanoID", DefaultValueFactory = _ => 21 };
    private readonly Option<string> _alphabetOption = new("--alphabet", "-a") { Description = "Alphabet to choose characters from", DefaultValueFactory = _ => string.Empty };
    private readonly Option<int> _countOption = new("--count", "-c") { Description = "Number of NanoIDs to generate", DefaultValueFactory = _ => 1 };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("nanoid", "Tiny, secure, URL-friendly unique string ID generator", GetExamples());
        command.Options.Add(_lengthOption);
        command.Options.Add(_alphabetOption);
        command.Options.Add(_countOption);

        command.SetAction(parseResult =>
        {
            var length = parseResult.GetValue(_lengthOption);
            var alphabet = parseResult.GetValue(_alphabetOption) ?? string.Empty;
            var count = parseResult.GetValue(_countOption);

            var res = nanoidService.Generate(length, alphabet, count);
            if (!res.Success)
            {
                outputProvider.AppendLine(res.ErrorMessage);
                return;
            }

            foreach (var nanoid in res.NanoIds)
            {
                outputProvider.AppendLine(nanoid);
            }
        });

        command.Aliases.Add("nid");
        return command;
    }

    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Generate one NanoID (length 21)", "nhash id nanoid"),
            new("Generate 3 NanoIDs of length 12", "nhash id nanoid -l 12 -c 3"),
            new("Generate with custom alphabet", "nhash id nanoid -a \"abcdef123456\"")
        ];
}
