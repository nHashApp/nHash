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

    public string ParseIso8601(string iso8601String)
    {
        if (string.IsNullOrWhiteSpace(iso8601String))
            return "Error: ISO 8601 string cannot be empty.";

        try
        {
            var dto = DateTimeOffset.Parse(iso8601String, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var cal = new GregorianCalendar();
            int weekOfYear = cal.GetWeekOfYear(dto.DateTime, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return $"ISO 8601:    {iso8601String}\n" +
                   $"Year:        {dto.Year}\n" +
                   $"Month:       {dto.Month}\n" +
                   $"Day:         {dto.Day}\n" +
                   $"Hour:        {dto.Hour}\n" +
                   $"Minute:      {dto.Minute}\n" +
                   $"Second:      {dto.Second}\n" +
                   $"Millisecond: {dto.Millisecond}\n" +
                   $"Offset:      {dto.Offset}\n" +
                   $"DayOfWeek:   {dto.DayOfWeek}\n" +
                   $"WeekOfYear:  {weekOfYear}\n" +
                   $"DayOfYear:   {dto.DayOfYear}\n" +
                   $"IsUtc:       {dto.Offset == TimeSpan.Zero}";
        }
        catch (Exception ex)
        {
            return $"Error parsing ISO 8601: {ex.Message}";
        }
    }

    public string AddDuration(string dateTimeStr, string duration)
    {
        if (string.IsNullOrWhiteSpace(dateTimeStr))
            return "Error: Date-time string cannot be empty.";
        if (string.IsNullOrWhiteSpace(duration))
            return "Error: Duration string cannot be empty.";

        try
        {
            var dto = DateTimeOffset.Parse(dateTimeStr, CultureInfo.InvariantCulture);
            var result = ApplyDuration(dto, duration);
            return $"Input:    {dto:O}\nDuration: {duration}\nResult:   {result:O}";
        }
        catch (Exception ex)
        {
            return $"Error adding duration: {ex.Message}";
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

    public string CountWorkingDays(string startStr, string endStr)
    {
        if (string.IsNullOrWhiteSpace(startStr) || string.IsNullOrWhiteSpace(endStr))
            return "Error: Start and end dates cannot be empty.";

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

            return $"Start:        {start:yyyy-MM-dd} ({start.DayOfWeek})\n" +
                   $"End:          {end:yyyy-MM-dd} ({end.DayOfWeek})\n" +
                   $"Total Days:   {totalDays}\n" +
                   $"Weekend Days: {weekendDays}\n" +
                   $"Working Days: {count}";
        }
        catch (Exception ex)
        {
            return $"Error counting working days: {ex.Message}";
        }
    }
}
