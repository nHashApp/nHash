using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Cryptos.Hashes;
using nHash.Console.CommandLines.Cryptos.Passwords;

namespace nHash.Console.CommandLines.Cryptos;

public class CryptoCommand(
    IHashCommand hashFeature,
    IPasswordCommand passwordFeature,
    IHmacCommand hmacFeature,
    ICipherCommand cipherFeature,
    ISignatureCommand signatureFeature,
    IBCryptCommand bcryptFeature)
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
            signatureFeature,
            bcryptFeature
        ];

        var command = new BaseCommand("crypto", "Security & Cryptography utilities (Hash, Password, HMAC, Cipher, RSA Signature, BCrypt)");
        command.Aliases.Add("cr");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}

