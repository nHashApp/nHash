using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Encodes.SubCommands;

namespace nHash.Console.CommandLines.Encodes;

public class EncodeCommand(
    IJwtTokenCommand jwtTokenFeature,
    IBase64Command base64Feature,
    IUrlCommand urlFeature,
    IHtmlCommand htmlFeature,
    IBase58Command base58Feature,
    IBase32Command base32Feature,
    IHexCommand hexFeature,
    IBase62Command base62Feature,
    IBase85Command base85Feature,
    IBase36Command base36Feature)
    : IEncodeCommand
{
    public BaseCommand Command => GetCommand();

    private BaseCommand GetCommand()
    {
        List<IFeature> features =
        [
            jwtTokenFeature,
            base64Feature,
            base58Feature,
            urlFeature,
            htmlFeature,
            base32Feature,
            hexFeature,
            base62Feature,
            base85Feature,
            base36Feature,
        ];

        var command = new BaseCommand("encode", "Encode/Decode features (JWT, Base64, URL, HTML, Base32, Hex, Base62, Base85, Base36)");
        command.Aliases.Add("e");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}