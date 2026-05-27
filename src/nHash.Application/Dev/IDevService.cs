namespace nHash.Application.Dev;

public interface IDevService
{
    string ParseCron(string cronExpression, int nextExecutionCount);
    string TestRegex(string pattern, string input);
    string ConvertColor(string inputColor);
}
