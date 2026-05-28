using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Hashes;
using nHash.Console.CommandLines.Passwords;

namespace nHash.Console.CommandLines.Cryptos;

public class CryptoCommand(
    IHashCommand hashFeature,
    IPasswordCommand passwordFeature,
    IHmacCommand hmacFeature,
    ICipherCommand cipherFeature,
    ISignatureCommand signatureFeature)
    : ICryptoCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        List<IFeature> features =
        [
            hashFeature,
            passwordFeature,
            hmacFeature,
            cipherFeature,
            signatureFeature
        ];

        var command = new BaseCommand("crypto", "Security & Cryptography utilities (Hash, Password, HMAC, Cipher, RSA Signature)");
        command.Aliases.Add("cr");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}

