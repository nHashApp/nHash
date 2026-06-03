using nHash.Application.Dev.Models;

namespace nHash.Application.Dev;

public interface IDevService
{
    CronParseResult ParseCron(string cronExpression, int nextExecutionCount);
    RegexTestResult TestRegex(string pattern, string input);
    ColorConvertResult ConvertColor(string inputColor);
    JwtBuildResult BuildJwt(string headerJson, string payloadJson);
    SemverCompareResult CompareSemver(string version1, string version2);
    NumberInspectResult InspectNumber(string number);
}
