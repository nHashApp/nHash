using System.Text.RegularExpressions;
using nHash.Application.Dev.Models;

namespace nHash.Application.Dev;

public class DevService : IDevService
{
    public CronParseResult ParseCron(string cronExpression, int nextExecutionCount)
    {
        var result = new CronParseResult();
        if (string.IsNullOrWhiteSpace(cronExpression))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Cron expression cannot be empty.";
            return result;
        }

        var parts = cronExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 5)
        {
            result.Success = false;
            result.ErrorMessage = "Error: Invalid Cron expression. Must contain exactly 5 fields (minute, hour, day-of-month, month, day-of-week).";
            return result;
        }

        try
        {
            result.MinuteDescription = DescribeCronField(parts[0], "minute", 0, 59);
            result.HourDescription = DescribeCronField(parts[1], "hour", 0, 23);
            result.DayOfMonthDescription = DescribeCronField(parts[2], "day of month", 1, 31);
            result.MonthDescription = DescribeCronField(parts[3], "month", 1, 12);
            result.DayOfWeekDescription = DescribeCronField(parts[4], "day of week", 0, 6, true);

            var matcher = new CronMatcher(parts);
            result.NextExecutions = matcher.GetNextExecutions(DateTime.Now, nextExecutionCount);
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Cron Parsing Error: {ex.Message}";
            return result;
        }
    }

    private static string DescribeCronField(string field, string name, int min, int max, bool isDow = false)
    {
        if (field == "*") return "every " + name;

        if (field.Contains('/'))
        {
            var stepParts = field.Split('/');
            var step = stepParts[1];
            var start = stepParts[0] == "*" ? "every" : "starting at " + stepParts[0];
            return $"every {step} {name}s, {start}";
        }

        if (field.Contains('-'))
        {
            return $"between {field.Replace("-", " and ")}";
        }

        if (field.Contains(','))
        {
            return $"at {name}s: {field}";
        }

        if (isDow)
        {
            if (int.TryParse(field, out int dowVal))
            {
                return ((DayOfWeek)dowVal).ToString();
            }
        }

        return "at " + name + " " + field;
    }

    private class CronMatcher
    {
        private readonly HashSet<int> _minutes;
        private readonly HashSet<int> _hours;
        private readonly HashSet<int> _days;
        private readonly HashSet<int> _months;
        private readonly HashSet<int> _dows;

        public CronMatcher(string[] parts)
        {
            _minutes = ParseCronFieldToSet(parts[0], 0, 59);
            _hours = ParseCronFieldToSet(parts[1], 0, 23);
            _days = ParseCronFieldToSet(parts[2], 1, 31);
            _months = ParseCronFieldToSet(parts[3], 1, 12);
            _dows = ParseCronFieldToSet(parts[4], 0, 6);
        }

        public List<DateTime> GetNextExecutions(DateTime start, int count)
        {
            var list = new List<DateTime>();
            var current = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute, 0).AddMinutes(1);
            var endLimit = current.AddYears(5);

            while (current < endLimit && list.Count < count)
            {
                if (_months.Contains(current.Month) &&
                    _days.Contains(current.Day) &&
                    _hours.Contains(current.Hour) &&
                    _minutes.Contains(current.Minute) &&
                    _dows.Contains((int)current.DayOfWeek))
                {
                    list.Add(current);
                }
                current = current.AddMinutes(1);
            }

            return list;
        }

        private static HashSet<int> ParseCronFieldToSet(string field, int min, int max)
        {
            var set = new HashSet<int>();
            if (field == "*")
            {
                for (int i = min; i <= max; i++) set.Add(i);
                return set;
            }

            var parts = field.Split(',');
            foreach (var part in parts)
            {
                if (part.Contains('/'))
                {
                    var divParts = part.Split('/');
                    var rangeStr = divParts[0];
                    var step = int.Parse(divParts[1]);

                    int rangeStart = min;
                    int rangeEnd = max;

                    if (rangeStr != "*")
                    {
                        if (rangeStr.Contains('-'))
                        {
                            var rParts = rangeStr.Split('-');
                            rangeStart = int.Parse(rParts[0]);
                            rangeEnd = int.Parse(rParts[1]);
                        }
                        else
                        {
                            rangeStart = int.Parse(rangeStr);
                        }
                    }

                    for (int i = rangeStart; i <= rangeEnd; i += step)
                    {
                        if (i >= min && i <= max) set.Add(i);
                    }
                }
                else if (part.Contains('-'))
                {
                    var rangeParts = part.Split('-');
                    int start = int.Parse(rangeParts[0]);
                    int end = int.Parse(rangeParts[1]);
                    for (int i = start; i <= end; i++)
                    {
                        if (i >= min && i <= max) set.Add(i);
                    }
                }
                else
                {
                    int val = int.Parse(part);
                    if (val >= min && val <= max) set.Add(val);
                }
            }

            return set;
        }
    }

    public RegexTestResult TestRegex(string pattern, string input)
    {
        var result = new RegexTestResult();
        if (string.IsNullOrEmpty(pattern))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Pattern cannot be empty.";
            return result;
        }
        if (input == null)
        {
            result.Success = false;
            result.ErrorMessage = "Error: Input cannot be null.";
            return result;
        }

        try
        {
            var regex = new Regex(pattern, RegexOptions.Compiled);
            var matches = regex.Matches(input);

            result.Pattern = pattern;
            result.Input = input;
            result.IsMatch = regex.IsMatch(input);
            result.MatchCount = matches.Count;
            result.Success = true;

            int matchIndex = 1;
            foreach (Match match in matches)
            {
                var matchDetail = new RegexMatchDetail
                {
                    MatchNumber = matchIndex++,
                    Value = match.Value,
                    Index = match.Index,
                    Length = match.Length
                };

                if (match.Groups.Count > 1)
                {
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        var group = match.Groups[i];
                        matchDetail.CaptureGroups.Add(new RegexCaptureGroupDetail
                        {
                            Index = i,
                            Name = regex.GroupNameFromNumber(i),
                            Value = group.Value
                        });
                    }
                }
                result.Matches.Add(matchDetail);
            }

            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Regex Error: {ex.Message}";
            return result;
        }
    }

    public ColorConvertResult ConvertColor(string inputColor)
    {
        var result = new ColorConvertResult();
        if (string.IsNullOrWhiteSpace(inputColor))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Input color cannot be empty.";
            return result;
        }
        inputColor = inputColor.Trim().ToLowerInvariant();

        try
        {
            int r = 0, g = 0, b = 0;

            if (inputColor.StartsWith('#') || (inputColor.Length == 6 && !inputColor.Contains(',')))
            {
                var hex = inputColor.StartsWith('#') ? inputColor[1..] : inputColor;
                if (hex.Length != 6 && hex.Length != 3)
                {
                    result.Success = false;
                    result.ErrorMessage = "Error: Invalid Hex color format. Use #RRGGBB or #RGB.";
                    return result;
                }

                if (hex.Length == 3)
                {
                    hex = $"{hex[0]}{hex[0]}{hex[1]}{hex[1]}{hex[2]}{hex[2]}";
                }

                r = Convert.ToInt32(hex[..2], 16);
                g = Convert.ToInt32(hex[2..4], 16);
                b = Convert.ToInt32(hex[4..], 16);
            }
            else if (inputColor.Contains(','))
            {
                var clean = inputColor.Replace("rgb", "").Replace("(", "").Replace(")", "").Trim();
                var parts = clean.Split(',');
                if (parts.Length < 3)
                {
                    result.Success = false;
                    result.ErrorMessage = "Error: Invalid RGB color format. Use r,g,b.";
                    return result;
                }
                r = int.Parse(parts[0]);
                g = int.Parse(parts[1]);
                b = int.Parse(parts[2]);
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = "Error: Unsupported color format. Use hex (#FF5733) or RGB (255,87,51).";
                return result;
            }

            if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
            {
                result.Success = false;
                result.ErrorMessage = "Error: Color values must be between 0 and 255.";
                return result;
            }

            result.R = r;
            result.G = g;
            result.B = b;
            result.Hex = $"#{r:x2}{g:x2}{b:x2}".ToUpperInvariant();
            result.Rgb = $"rgb({r}, {g}, {b})";
            var (h, s, l) = RgbToHsl(r, g, b);
            result.Hsl = $"hsl({h:0}, {s:0}%, {l:0}%)";
            var (c, m, y, k) = RgbToCmyk(r, g, b);
            result.Cmyk = $"cmyk({c:0}%, {m:0}%, {y:0}%, {k:0}%)";
            result.Success = true;

            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Color Conversion Error: {ex.Message}";
            return result;
        }
    }

    private static (double h, double s, double l) RgbToHsl(int r, int g, int b)
    {
        double rd = r / 255.0;
        double gd = g / 255.0;
        double bd = b / 255.0;

        double max = Math.Max(rd, Math.Max(gd, bd));
        double min = Math.Min(rd, Math.Min(gd, bd));

        double h = 0, s = 0, l = (max + min) / 2.0;

        if (max != min)
        {
            double d = max - min;
            s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);

            if (max == rd)
            {
                h = (gd - bd) / d + (gd < bd ? 6 : 0);
            }
            else if (max == gd)
            {
                h = (bd - rd) / d + 2;
            }
            else if (max == bd)
            {
                h = (rd - gd) / d + 4;
            }

            h /= 6.0;
        }

        return (h * 360, s * 100, l * 100);
    }

    private static (double c, double m, double y, double k) RgbToCmyk(int r, int g, int b)
    {
        double rd = r / 255.0;
        double gd = g / 255.0;
        double bd = b / 255.0;

        double k = 1 - Math.Max(rd, Math.Max(gd, bd));
        if (k == 1.0)
        {
            return (0, 0, 0, 100);
        }

        double c = (1 - rd - k) / (1 - k);
        double m = (1 - gd - k) / (1 - k);
        double y = (1 - bd - k) / (1 - k);

        return (c * 100, m * 100, y * 100, k * 100);
    }

    public JwtBuildResult BuildJwt(string headerJson, string payloadJson)
    {
        var result = new JwtBuildResult();
        if (string.IsNullOrWhiteSpace(payloadJson))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Payload JSON cannot be empty.";
            return result;
        }

        try
        {
            var headerBytes = System.Text.Encoding.UTF8.GetBytes(headerJson);
            var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payloadJson);

            var encodedHeader = Base64UrlEncode(headerBytes);
            var encodedPayload = Base64UrlEncode(payloadBytes);

            // Signature placeholder (unsigned)
            var signatureBytes = System.Text.Encoding.UTF8.GetBytes("unsigned");
            var encodedSignature = Base64UrlEncode(signatureBytes);

            result.Token = $"{encodedHeader}.{encodedPayload}.{encodedSignature}";
            result.Header = headerJson;
            result.Payload = payloadJson;
            result.Signature = "unsigned (placeholder)";
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error building JWT: {ex.Message}";
            return result;
        }
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    public SemverCompareResult CompareSemver(string version1, string version2)
    {
        var result = new SemverCompareResult();
        if (string.IsNullOrWhiteSpace(version1) || string.IsNullOrWhiteSpace(version2))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Both version strings are required.";
            return result;
        }

        try
        {
            var v1 = ParseSemver(version1);
            var v2 = ParseSemver(version2);

            result.Version1 = version1;
            result.Version2 = version2;

            result.Details1 = new SemverDetails
            {
                Original = version1,
                Major = v1.major,
                Minor = v1.minor,
                Patch = v1.patch,
                Prerelease = v1.prerelease,
                Build = v1.build
            };

            result.Details2 = new SemverDetails
            {
                Original = version2,
                Major = v2.major,
                Minor = v2.minor,
                Patch = v2.patch,
                Prerelease = v2.prerelease,
                Build = v2.build
            };

            if (v1.major != v2.major)
                result.ComparisonResult = v1.major > v2.major ? "v1 > v2" : "v1 < v2";
            else if (v1.minor != v2.minor)
                result.ComparisonResult = v1.minor > v2.minor ? "v1 > v2" : "v1 < v2";
            else if (v1.patch != v2.patch)
                result.ComparisonResult = v1.patch > v2.patch ? "v1 > v2" : "v1 < v2";
            else
                result.ComparisonResult = "v1 == v2";

            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error comparing semver: {ex.Message}";
            return result;
        }
    }

    private static (int major, int minor, int patch, string prerelease, string build) ParseSemver(string version)
    {
        var v = version.TrimStart('v', 'V');

        string build = string.Empty;
        string prerelease = string.Empty;

        int plusIdx = v.IndexOf('+');
        if (plusIdx >= 0)
        {
            build = v[(plusIdx + 1)..];
            v = v[..plusIdx];
        }

        int dashIdx = v.IndexOf('-');
        if (dashIdx >= 0)
        {
            prerelease = v[(dashIdx + 1)..];
            v = v[..dashIdx];
        }

        var parts = v.Split('.');
        int major = parts.Length > 0 ? int.Parse(parts[0]) : 0;
        int minor = parts.Length > 1 ? int.Parse(parts[1]) : 0;
        int patch = parts.Length > 2 ? int.Parse(parts[2]) : 0;

        return (major, minor, patch, prerelease, build);
    }

    public NumberInspectResult InspectNumber(string number)
    {
        var result = new NumberInspectResult();
        if (string.IsNullOrWhiteSpace(number))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Number cannot be empty.";
            return result;
        }

        try
        {
            result.Input = number;

            // Try integer first
            if (long.TryParse(number, out long intVal))
            {
                result.Success = true;
                result.IsInteger = true;
                result.IntegerValue = intVal;
                result.Type = "Integer (64-bit)";
                result.Decimal = $"{intVal:N0}";
                result.Binary = Convert.ToString(intVal, 2);
                result.Octal = Convert.ToString(intVal, 8);
                result.Hexadecimal = $"0x{intVal:X}";
                result.Scientific = $"{(double)intVal:E6}";
                result.Positive = intVal >= 0;
                result.Even = intVal % 2 == 0;
                return result;
            }

            // Try floating point
            if (double.TryParse(number, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double dblVal))
            {
                result.Success = true;
                result.IsInteger = false;
                result.DoubleValue = dblVal;
                result.Type = "Double (64-bit float)";
                result.Decimal = $"{dblVal}";
                result.Scientific = $"{dblVal:E6}";
                result.Hexadecimal = $"0x{BitConverter.DoubleToInt64Bits(dblVal):X16}";
                result.IsNaN = double.IsNaN(dblVal);
                result.IsInfinity = double.IsInfinity(dblVal);
                result.IsFinite = double.IsFinite(dblVal);
                return result;
            }

            result.Success = false;
            result.ErrorMessage = $"Error: '{number}' is not a valid number.";
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error inspecting number: {ex.Message}";
            return result;
        }
    }
}
