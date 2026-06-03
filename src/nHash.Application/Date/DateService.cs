using System.Globalization;
using nHash.Application.Date.Models;

namespace nHash.Application.Date;

public class DateService : IDateService
{
    public EpochToDateTimeResult EpochToDateTime(long epochValue, bool isMilliseconds)
    {
        var result = new EpochToDateTimeResult();
        try
        {
            var offset = isMilliseconds
                ? DateTimeOffset.FromUnixTimeMilliseconds(epochValue)
                : DateTimeOffset.FromUnixTimeSeconds(epochValue);

            result.DateTimeOffset = offset;
            result.UtcDateTime = offset.UtcDateTime;
            result.LocalDateTime = offset.LocalDateTime;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error: {ex.Message}";
            return result;
        }
    }

    public DateTimeToEpochResult DateTimeToEpoch(string dateTimeStr)
    {
        var result = new DateTimeToEpochResult();
        if (string.IsNullOrWhiteSpace(dateTimeStr))
        {
            var now = DateTimeOffset.UtcNow;
            result.Label = "Current Epoch";
            result.Seconds = now.ToUnixTimeSeconds();
            result.Milliseconds = now.ToUnixTimeMilliseconds();
            result.Success = true;
            return result;
        }

        try
        {
            var offset = DateTimeOffset.Parse(dateTimeStr, CultureInfo.InvariantCulture);
            result.Label = "Epoch";
            result.Seconds = offset.ToUnixTimeSeconds();
            result.Milliseconds = offset.ToUnixTimeMilliseconds();
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error parsing date: {ex.Message}";
            return result;
        }
    }

    public CalendarConvertResult ConvertCalendars(string dateTimeStr, string fromCalendar, string toCalendar)
    {
        var result = new CalendarConvertResult();
        if (string.IsNullOrWhiteSpace(dateTimeStr))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Input date cannot be empty.";
            return result;
        }

        var from = fromCalendar.ToLowerInvariant().Trim();
        var to = toCalendar.ToLowerInvariant().Trim();

        try
        {
            DateTime gregorianDate;

            if (from is "jalali" or "persian" or "shamsi")
            {
                gregorianDate = ParseJalali(dateTimeStr);
            }
            else if (from is "hijri" or "islamic")
            {
                gregorianDate = ParseHijri(dateTimeStr);
            }
            else
            {
                gregorianDate = DateTime.Parse(dateTimeStr, CultureInfo.InvariantCulture);
            }

            if (to is "jalali" or "persian" or "shamsi")
            {
                var pc = new PersianCalendar();
                result.ConvertedDate = $"{pc.GetYear(gregorianDate):0000}/{pc.GetMonth(gregorianDate):00}/{pc.GetDayOfMonth(gregorianDate):00} {gregorianDate.Hour:00}:{gregorianDate.Minute:00}:{gregorianDate.Second:00}";
            }
            else if (to is "hijri" or "islamic")
            {
                var hc = new UmAlQuraCalendar();
                result.ConvertedDate = $"{hc.GetYear(gregorianDate):0000}/{hc.GetMonth(gregorianDate):00}/{hc.GetDayOfMonth(gregorianDate):00} {gregorianDate.Hour:00}:{gregorianDate.Minute:00}:{gregorianDate.Second:00}";
            }
            else
            {
                result.ConvertedDate = gregorianDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }

            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Calendar Conversion Error: {ex.Message}";
            return result;
        }
    }

    public CalculateDifferenceResult CalculateDifference(string startStr, string endStr)
    {
        var result = new CalculateDifferenceResult();
        try
        {
            var start = DateTimeOffset.Parse(startStr, CultureInfo.InvariantCulture);
            var end = DateTimeOffset.Parse(endStr, CultureInfo.InvariantCulture);
            var diff = end - start;

            var absDiff = diff.Duration();
            result.TotalDays = absDiff.TotalDays;
            result.TotalHours = absDiff.TotalHours;
            result.TotalMinutes = absDiff.TotalMinutes;
            result.TotalSeconds = absDiff.TotalSeconds;
            result.Days = absDiff.Days;
            result.Hours = absDiff.Hours;
            result.Minutes = absDiff.Minutes;
            result.Seconds = absDiff.Seconds;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Difference calculation error: {ex.Message}";
            return result;
        }
    }

    public ConvertTimezoneResult ConvertTimezone(string dateTimeStr, string fromTimezoneId, string toTimezoneId)
    {
        var result = new ConvertTimezoneResult();
        try
        {
            var parsed = DateTime.Parse(dateTimeStr, CultureInfo.InvariantCulture);

            var fromTz = TimeZoneInfo.FindSystemTimeZoneById(fromTimezoneId);
            var toTz = TimeZoneInfo.FindSystemTimeZoneById(toTimezoneId);

            var utc = TimeZoneInfo.ConvertTimeToUtc(parsed, fromTz);
            var targetTime = TimeZoneInfo.ConvertTimeFromUtc(utc, toTz);

            result.SourceTimezone = fromTimezoneId;
            result.SourceTime = parsed;
            result.TargetTimezone = toTimezoneId;
            result.TargetTime = targetTime;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Timezone Conversion Error: {ex.Message}";
            return result;
        }
    }

    private static DateTime ParseJalali(string input)
    {
        var parts = SplitDateParts(input);
        var pc = new PersianCalendar();
        return new DateTime(parts.year, parts.month, parts.day, parts.hour, parts.minute, parts.second, pc);
    }

    private static DateTime ParseHijri(string input)
    {
        var parts = SplitDateParts(input);
        var hc = new UmAlQuraCalendar();
        return new DateTime(parts.year, parts.month, parts.day, parts.hour, parts.minute, parts.second, hc);
    }

    private static (int year, int month, int day, int hour, int minute, int second) SplitDateParts(string input)
    {
        var clean = input.Replace('-', '/').Replace('.', '/').Trim();
        var dateAndTime = clean.Split(' ');
        var dateParts = dateAndTime[0].Split('/');

        if (dateParts.Length < 3) throw new ArgumentException("Date format must contain year, month, and day.");

        int year = int.Parse(dateParts[0]);
        int month = int.Parse(dateParts[1]);
        int day = int.Parse(dateParts[2]);

        int hour = 0, minute = 0, second = 0;
        if (dateAndTime.Length > 1)
        {
            var timeParts = dateAndTime[1].Split(':');
            if (timeParts.Length > 0) hour = int.Parse(timeParts[0]);
            if (timeParts.Length > 1) minute = int.Parse(timeParts[1]);
            if (timeParts.Length > 2) second = int.Parse(timeParts[2]);
        }

        return (year, month, day, hour, minute, second);
    }

    public ParseIso8601Result ParseIso8601(string iso8601String)
    {
        var result = new ParseIso8601Result();
        if (string.IsNullOrWhiteSpace(iso8601String))
        {
            result.Success = false;
            result.ErrorMessage = "Error: ISO 8601 string cannot be empty.";
            return result;
        }

        try
        {
            var dto = DateTimeOffset.Parse(iso8601String, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var cal = new GregorianCalendar();
            int weekOfYear = cal.GetWeekOfYear(dto.DateTime, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            result.Iso8601String = iso8601String;
            result.Year = dto.Year;
            result.Month = dto.Month;
            result.Day = dto.Day;
            result.Hour = dto.Hour;
            result.Minute = dto.Minute;
            result.Second = dto.Second;
            result.Millisecond = dto.Millisecond;
            result.Offset = dto.Offset;
            result.DayOfWeek = dto.DayOfWeek;
            result.WeekOfYear = weekOfYear;
            result.DayOfYear = dto.DayOfYear;
            result.IsUtc = dto.Offset == TimeSpan.Zero;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error parsing ISO 8601: {ex.Message}";
            return result;
        }
    }

    public AddDurationResult AddDuration(string dateTimeStr, string duration)
    {
        var result = new AddDurationResult();
        if (string.IsNullOrWhiteSpace(dateTimeStr))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Date-time string cannot be empty.";
            return result;
        }
        if (string.IsNullOrWhiteSpace(duration))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Duration string cannot be empty.";
            return result;
        }

        try
        {
            var dto = DateTimeOffset.Parse(dateTimeStr, CultureInfo.InvariantCulture);
            var durationResult = ApplyDuration(dto, duration);

            result.Input = dto;
            result.Duration = duration;
            result.Result = durationResult;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error adding duration: {ex.Message}";
            return result;
        }
    }

    private static DateTimeOffset ApplyDuration(DateTimeOffset dto, string duration)
    {
        // Parse patterns like +1y2M3w4d5h6m7s or -30d
        var input = duration.Trim();
        int sign = 1;
        int start = 0;

        if (input.StartsWith('+')) { sign = 1; start = 1; }
        else if (input.StartsWith('-')) { sign = -1; start = 1; }

        var rest = input[start..];

        // Parse each unit group: number followed by unit letter
        int i = 0;
        while (i < rest.Length)
        {
            // Read digits
            int numStart = i;
            while (i < rest.Length && char.IsDigit(rest[i])) i++;
            if (i == numStart) { i++; continue; } // skip unexpected chars
            int amount = int.Parse(rest[numStart..i]) * sign;

            if (i >= rest.Length) break;
            char unit = rest[i++];

            dto = unit switch
            {
                'y' => dto.AddYears(amount),
                'M' => dto.AddMonths(amount),
                'w' => dto.AddDays(amount * 7),
                'd' => dto.AddDays(amount),
                'h' => dto.AddHours(amount),
                'm' => dto.AddMinutes(amount),
                's' => dto.AddSeconds(amount),
                _ => dto
            };
        }

        return dto;
    }

    public CountWorkingDaysResult CountWorkingDays(string startStr, string endStr)
    {
        var result = new CountWorkingDaysResult();
        if (string.IsNullOrWhiteSpace(startStr) || string.IsNullOrWhiteSpace(endStr))
        {
            result.Success = false;
            result.ErrorMessage = "Error: Start and end dates cannot be empty.";
            return result;
        }

        try
        {
            var start = DateTimeOffset.Parse(startStr, CultureInfo.InvariantCulture).Date;
            var end = DateTimeOffset.Parse(endStr, CultureInfo.InvariantCulture).Date;

            if (end < start)
                (start, end) = (end, start);

            int count = 0;
            var current = start;
            while (current <= end)
            {
                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                    count++;
                current = current.AddDays(1);
            }

            int totalDays = (end - start).Days + 1;
            int weekendDays = totalDays - count;

            result.StartDate = start;
            result.StartDayOfWeek = start.DayOfWeek;
            result.EndDate = end;
            result.EndDayOfWeek = end.DayOfWeek;
            result.TotalDays = totalDays;
            result.WeekendDays = weekendDays;
            result.WorkingDays = count;
            result.Success = true;
            return result;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Error counting working days: {ex.Message}";
            return result;
        }
    }
}
