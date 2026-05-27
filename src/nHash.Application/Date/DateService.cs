using System.Globalization;

namespace nHash.Application.Date;

public class DateService : IDateService
{
    public string EpochToDateTime(long epochValue, bool isMilliseconds)
    {
        try
        {
            var offset = isMilliseconds
                ? DateTimeOffset.FromUnixTimeMilliseconds(epochValue)
                : DateTimeOffset.FromUnixTimeSeconds(epochValue);

            return $"UTC: {offset.UtcDateTime:yyyy-MM-dd HH:mm:ss.fff K}\nLocal: {offset.LocalDateTime:yyyy-MM-dd HH:mm:ss.fff K}";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public string DateTimeToEpoch(string dateTimeStr)
    {
        if (string.IsNullOrWhiteSpace(dateTimeStr))
        {
            var now = DateTimeOffset.UtcNow;
            return $"Current Epoch (Seconds): {now.ToUnixTimeSeconds()}\nCurrent Epoch (Milliseconds): {now.ToUnixTimeMilliseconds()}";
        }

        try
        {
            var offset = DateTimeOffset.Parse(dateTimeStr, CultureInfo.InvariantCulture);
            return $"Epoch (Seconds): {offset.ToUnixTimeSeconds()}\nEpoch (Milliseconds): {offset.ToUnixTimeMilliseconds()}";
        }
        catch (Exception ex)
        {
            return $"Error parsing date: {ex.Message}";
        }
    }

    public string ConvertCalendars(string dateTimeStr, string fromCalendar, string toCalendar)
    {
        if (string.IsNullOrWhiteSpace(dateTimeStr)) return "Error: Input date cannot be empty.";
        
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
                return $"{pc.GetYear(gregorianDate):0000}/{pc.GetMonth(gregorianDate):00}/{pc.GetDayOfMonth(gregorianDate):00} {gregorianDate.Hour:00}:{gregorianDate.Minute:00}:{gregorianDate.Second:00}";
            }
            else if (to is "hijri" or "islamic")
            {
                var hc = new UmAlQuraCalendar();
                return $"{hc.GetYear(gregorianDate):0000}/{hc.GetMonth(gregorianDate):00}/{hc.GetDayOfMonth(gregorianDate):00} {gregorianDate.Hour:00}:{gregorianDate.Minute:00}:{gregorianDate.Second:00}";
            }
            else
            {
                return gregorianDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
        }
        catch (Exception ex)
        {
            return $"Calendar Conversion Error: {ex.Message}";
        }
    }

    public string CalculateDifference(string startStr, string endStr)
    {
        try
        {
            var start = DateTimeOffset.Parse(startStr, CultureInfo.InvariantCulture);
            var end = DateTimeOffset.Parse(endStr, CultureInfo.InvariantCulture);
            var diff = end - start;

            var absDiff = diff.Duration();
            return $"Difference details:\n" +
                   $"- Total Days: {absDiff.TotalDays:N2} days\n" +
                   $"- Total Hours: {absDiff.TotalHours:N2} hours\n" +
                   $"- Total Minutes: {absDiff.TotalMinutes:N2} minutes\n" +
                   $"- Total Seconds: {absDiff.TotalSeconds:N0} seconds\n" +
                   $"- Human Readable: {absDiff.Days}d {absDiff.Hours}h {absDiff.Minutes}m {absDiff.Seconds}s";
        }
        catch (Exception ex)
        {
            return $"Difference calculation error: {ex.Message}";
        }
    }

    public string ConvertTimezone(string dateTimeStr, string fromTimezoneId, string toTimezoneId)
    {
        try
        {
            var parsed = DateTime.Parse(dateTimeStr, CultureInfo.InvariantCulture);
            
            var fromTz = TimeZoneInfo.FindSystemTimeZoneById(fromTimezoneId);
            var toTz = TimeZoneInfo.FindSystemTimeZoneById(toTimezoneId);

            var utc = TimeZoneInfo.ConvertTimeToUtc(parsed, fromTz);
            var targetTime = TimeZoneInfo.ConvertTimeFromUtc(utc, toTz);

            return $"Source ({fromTimezoneId}): {parsed:yyyy-MM-dd HH:mm:ss}\n" +
                   $"Target ({toTimezoneId}): {targetTime:yyyy-MM-dd HH:mm:ss}";
        }
        catch (Exception ex)
        {
            return $"Timezone Conversion Error: {ex.Message}";
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
}
