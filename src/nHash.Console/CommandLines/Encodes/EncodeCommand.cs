using nHash.Console.CommandLines.Base;
using nHash.Console.CommandLines.Encodes.SubCommands;

namespace nHash.Console.CommandLines.Encodes;

public class EncodeCommand(
    IJwtTokenCommand jwtTokenFeature,
    IBase64Command base64Feature,
    IUrlCommand urlFeature,
    IHtmlCommand htmlFeature,
    IBase58Command base58Feature)
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
        ];

        var command = new BaseCommand("encode", "Encode/Decode features (JWT, Base64, URL, HTML)");
        command.Aliases.Add("e");
        foreach (var feature in features)
        {
            command.Subcommands.Add(feature.Command);
        }

        return command;
    }
}