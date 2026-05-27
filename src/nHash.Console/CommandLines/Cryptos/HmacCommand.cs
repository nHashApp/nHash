using nHash.Application.Cryptos.Hashes;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Cryptos;

public class HmacCommand(IHmacService hmacService, IOutputProvider outputProvider) : IHmacCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument = new("text") { Description = "Text message to sign" };
    private readonly Option<string> _keyOption = new("--key", "-k") { Description = "Secret key for HMAC signature", DefaultValueFactory = _ => string.Empty };
    private readonly Option<string> _algoOption = new("--type", "-t") { Description = "HMAC algorithm (sha256, sha512, md5, sha1)", DefaultValueFactory = _ => "sha256" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("hmac", "Generate Keyed-Hash Message Authentication Code (HMAC)", GetExamples());
        command.Options.Add(_keyOption);
        command.Options.Add(_algoOption);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument) ?? string.Empty;
            var key = parseResult.GetValue(_keyOption) ?? string.Empty;
            var algo = parseResult.GetValue(_algoOption) ?? "sha256";
            GenerateHmac(text, key, algo);
        });

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Generate HMAC-SHA256 of text", "nhash crypto hmac \"Hello message\" -k \"mySecretKey\""),
            new("Generate HMAC-SHA512 of text", "nhash crypto hmac \"Hello message\" --key \"mySecretKey\" --type sha512"),
        ];

    private void GenerateHmac(string text, string key, string algo)
    {
        var result = hmacService.Calculate(text, key, algo);
        outputProvider.AppendLine(result);
    }
}
