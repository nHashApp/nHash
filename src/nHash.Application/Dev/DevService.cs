using System.Text;
using System.Text.RegularExpressions;

namespace nHash.Application.Dev;

public class DevService : IDevService
{
    public string ParseCron(string cronExpression, int nextExecutionCount)
    {
        if (string.IsNullOrWhiteSpace(cronExpression))
            return "Error: Cron expression cannot be empty.";

        var parts = cronExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 5)
            return "Error: Invalid Cron expression. Must contain exactly 5 fields (minute, hour, day-of-month, month, day-of-week).";

        try
        {
            var minuteDesc = DescribeCronField(parts[0], "minute", 0, 59);
            var hourDesc = DescribeCronField(parts[1], "hour", 0, 23);
            var dayDesc = DescribeCronField(parts[2], "day of month", 1, 31);
            var monthDesc = DescribeCronField(parts[3], "month", 1, 12);
            var dowDesc = DescribeCronField(parts[4], "day of week", 0, 6, true);

            var sb = new StringBuilder();
            sb.AppendLine("Cron Description:");
            sb.AppendLine($"- Minutes: {minuteDesc}");
            sb.AppendLine($"- Hours: {hourDesc}");
            sb.AppendLine($"- Days of Month: {dayDesc}");
            sb.AppendLine($"- Months: {monthDesc}");
            sb.AppendLine($"- Days of Week: {dowDesc}");
            sb.AppendLine();

            var matcher = new CronMatcher(parts);
            var nextTimes = matcher.GetNextExecutions(DateTime.Now, nextExecutionCount);
            sb.AppendLine($"Next {nextExecutionCount} Executions (Local Time):");
            if (nextTimes.Count == 0)
            {
                sb.AppendLine("  No matching execution times found in the near future.");
            }
            else
            {
                foreach (var time in nextTimes)
                {
                    sb.AppendLine($"  - {time:yyyy-MM-dd HH:mm:ss}");
                }
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Cron Parsing Error: {ex.Message}";
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

    public string TestRegex(string pattern, string input)
    {
        if (string.IsNullOrEmpty(pattern)) return "Error: Pattern cannot be empty.";
        if (input == null) return "Error: Input cannot be null.";

        try
        {
            var regex = new Regex(pattern, RegexOptions.Compiled);
            var matches = regex.Matches(input);

            var sb = new StringBuilder();
            sb.AppendLine($"Regex Pattern: {pattern}");
            sb.AppendLine($"Input Text: {input}");
            sb.AppendLine($"Is Match: {regex.IsMatch(input)}");
            sb.AppendLine($"Matches Count: {matches.Count}");
            sb.AppendLine();

            int matchIndex = 1;
            foreach (Match match in matches)
            {
                sb.AppendLine($"Match #{matchIndex++}:");
                sb.AppendLine($"  - Value: \"{match.Value}\"");
                sb.AppendLine($"  - Index: {match.Index}");
                sb.AppendLine($"  - Length: {match.Length}");
                
                if (match.Groups.Count > 1)
                {
                    sb.AppendLine("  - Capture Groups:");
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        var group = match.Groups[i];
                        sb.AppendLine($"    Group {i} ({regex.GroupNameFromNumber(i)}): \"{group.Value}\" (Index: {group.Index})");
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            return $"Regex Error: {ex.Message}";
        }
    }

    public string ConvertColor(string inputColor)
    {
        if (string.IsNullOrWhiteSpace(inputColor)) return "Error: Input color cannot be empty.";
        inputColor = inputColor.Trim().ToLowerInvariant();

        try
        {
            int r = 0, g = 0, b = 0;

            if (inputColor.StartsWith('#') || (inputColor.Length == 6 && !inputColor.Contains(',')))
            {
                var hex = inputColor.StartsWith('#') ? inputColor[1..] : inputColor;
                if (hex.Length != 6 && hex.Length != 3)
                {
                    return "Error: Invalid Hex color format. Use #RRGGBB or #RGB.";
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
                if (parts.Length < 3) return "Error: Invalid RGB color format. Use r,g,b.";
                r = int.Parse(parts[0]);
                g = int.Parse(parts[1]);
                b = int.Parse(parts[2]);
            }
            else
            {
                return "Error: Unsupported color format. Use hex (#FF5733) or RGB (255,87,51).";
            }

            if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
            {
                return "Error: Color values must be between 0 and 255.";
            }

            var hexStr = $"#{r:x2}{g:x2}{b:x2}".ToUpperInvariant();
            var rgbStr = $"rgb({r}, {g}, {b})";
            var (h, s, l) = RgbToHsl(r, g, b);
            var hslStr = $"hsl({h:0}, {s:0}%, {l:0}%)";
            var (c, m, y, k) = RgbToCmyk(r, g, b);
            var cmykStr = $"cmyk({c:0}%, {m:0}%, {y:0}%, {k:0}%)";

            var colorBlock = $"\x1b[48;2;{r};{g};{b}m      \x1b[0m";

            return $"Color Details:\n" +
                   $"- Visual Preview: {colorBlock}\n" +
                   $"- HEX: {hexStr}\n" +
                   $"- RGB: {rgbStr}\n" +
                   $"- HSL: {hslStr}\n" +
                   $"- CMYK: {cmykStr}";
        }
        catch (Exception ex)
        {
            return $"Color Conversion Error: {ex.Message}";
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
}
