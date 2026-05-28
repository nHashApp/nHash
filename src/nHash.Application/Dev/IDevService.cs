namespace nHash.Application.Dev;

public interface IDevService
{
    string ParseCron(string cronExpression, int nextExecutionCount);
    string TestRegex(string pattern, string input);
    string ConvertColor(string inputColor);
    string BuildJwt(string headerJson, string payloadJson);
    string CompareSemver(string version1, string version2);
    string InspectNumber(string number);
}
