

using nHash.Features.HashAlgorithms;

var features = new List<IFeature>()
{
    new GuidFeature(),
    new Md5Feature(),
    new Sha1Feature(),
    new Sha256Feature(),
    new Sha384Feature(),
    new Sha512Feature(),
    new Base64Feature(),
};

var rootCommand = new RootCommand("Hash utilities in command-line mode");
foreach (var command in features)
{
    rootCommand.AddCommand(command.Command);
}

return await rootCommand.InvokeAsync(args);