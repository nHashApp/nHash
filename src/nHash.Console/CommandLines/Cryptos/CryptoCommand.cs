using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Hashes;
using nHash.Console.CommandLines.Passwords;

namespace nHash.Console.CommandLines.Cryptos;

public class CryptoCommand(
    IHashCommand hashFeature,
    IPasswordCommand passwordFeature,
    IHmacCommand hmacFeature,
    ICipherCommand cipherFeature)
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
            cipherFeature
        ];

        var command = new BaseCommand("crypto", "Security & Cryptography utilities (Hash, Password, HMAC, Cipher)");
        command.Aliases.Add("cr");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}
