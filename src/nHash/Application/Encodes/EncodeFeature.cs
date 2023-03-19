namespace nHash.Application.Encodes;

public class EncodeFeature : IEncodeFeature
{
    public Command Command => GetCommand();

    private readonly IJwtTokenFeature _jwtTokenFeature;
    private readonly IBase64Feature _base64Feature;
    private readonly IUrlFeature _urlFeature;
    private readonly IHtmlFeature _htmlFeature;

    public EncodeFeature(IJwtTokenFeature jwtTokenFeature, IBase64Feature base64Feature, IUrlFeature urlFeature,
        IHtmlFeature htmlFeature)
    {
        _jwtTokenFeature = jwtTokenFeature;
        _base64Feature = base64Feature;
        _urlFeature = urlFeature;
        _htmlFeature = htmlFeature;
    }

    private Command GetCommand()
    {
        var features = new List<IFeature>()
        {
            _jwtTokenFeature,
            _base64Feature,
            _urlFeature,
            _htmlFeature,
        };

        var command = new Command("encode", "Encode/Decode features (JWT, Base64, URL, HTML)");
        foreach (var feature in features)
        {
            command.AddCommand(feature.Command);
        }

        return command;
    }
}