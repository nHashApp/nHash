namespace nHash.Features.HashAlgorithms;

public abstract class BaseHashAlgorithm : IFeature
{
    private string HashName { get; }
    private string CommandName { get; }

    public Command Command => GetFeatureCommand();
    private readonly Option<bool> _lowerCase;
    private readonly Option<string> _fileName;
    private readonly Argument<string> _textArgument;

    protected abstract byte[] CalculateHash(byte[] input);

    protected BaseHashAlgorithm(string commandName, string hashName)
    {
        HashName = hashName;
        CommandName = commandName.ToLower();
        _lowerCase = new Option<bool>(name: "--lower", description: "Generate lower case");
        _fileName = new Option<string>(name: "--file", description: $"File name for calculate {hashName}");
        _textArgument = new Argument<string>("text", GetDefaultValue, $"text for calculate {hashName} fingerprint");
    }

    private Command GetFeatureCommand()
    {
        var command = new Command(CommandName, $"Calculate {HashName} fingerprint")
        {
            _lowerCase,
            _fileName
        };
        command.AddArgument(_textArgument);
        command.SetHandler(CalculateTextHash, _textArgument, _lowerCase, _fileName);

        return command;
    }

    private static string GetDefaultValue()
    {
        return string.Empty;
    }

    private void CalculateTextHash(string text, bool lowerCase, string fileName)
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(text);
            CalculateHash(inputBytes, lowerCase);
            return;
        }

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            var fileBytes = File.ReadAllBytes(fileName);
            CalculateHash(fileBytes, lowerCase);
        }
    }

    private void CalculateHash(byte[] inputBytes, bool lowerCase)
    {
        var hashBytes = CalculateHash(inputBytes);
        var hashedText = Convert.ToHexString(hashBytes);

        if (lowerCase)
        {
            hashedText = hashedText.ToLower();
        }

        Console.WriteLine(hashedText);
    }
}