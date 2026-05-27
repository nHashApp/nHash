using nHash.Application.Cryptos;
using nHash.Console.CommandLines.Base;

namespace nHash.Console.CommandLines.Cryptos;

public class CipherCommand(ICipherService cipherService, IOutputProvider outputProvider) : ICipherCommand
{
    public BaseCommand Command => GetFeatureCommand();
    private readonly Argument<string> _textArgument = new("text") { Description = "Text payload to encrypt or decrypt" };
    private readonly Option<string> _passOption = new("--pass", "-p") { Description = "Password/Passphrase for key derivation", DefaultValueFactory = _ => string.Empty };
    private readonly Option<string> _algoOption = new("--type", "-t") { Description = "Cipher type (aes, chacha)", DefaultValueFactory = _ => "aes" };
    private readonly Option<bool> _decryptOption = new("--decrypt", "-d") { Description = "Decrypt the payload instead of encrypting" };

    private BaseCommand GetFeatureCommand()
    {
        var command = new BaseCommand("cipher", "Symmetric encryption & decryption (AES-GCM, ChaCha20-Poly1305)", GetExamples());
        command.Options.Add(_passOption);
        command.Options.Add(_algoOption);
        command.Options.Add(_decryptOption);
        command.Arguments.Add(_textArgument);
        command.SetAction(parseResult =>
        {
            var text = parseResult.GetValue(_textArgument) ?? string.Empty;
            var pass = parseResult.GetValue(_passOption) ?? string.Empty;
            var algo = parseResult.GetValue(_algoOption) ?? "aes";
            var decrypt = parseResult.GetValue(_decryptOption);

            if (decrypt)
            {
                var result = cipherService.Decrypt(text, pass, algo);
                outputProvider.AppendLine(result);
            }
            else
            {
                var result = cipherService.Encrypt(text, pass, algo);
                outputProvider.AppendLine(result);
            }
        });

        return command;
    }
    
    private static List<KeyValuePair<string, string>> GetExamples() =>
        [
            new("Encrypt text using AES-GCM", "nhash crypto cipher \"sensitive data\" -p \"strongPassword\""),
            new("Decrypt ciphertext using AES-GCM", "nhash crypto cipher <HEX_STRING> --pass \"strongPassword\" --decrypt"),
            new("Encrypt text using ChaCha20", "nhash crypto cipher \"sensitive data\" -p \"strongPassword\" -t chacha"),
        ];
}
