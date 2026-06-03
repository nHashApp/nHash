namespace nHash.Application.Date.Models;

public class EpochToDateTimeResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public DateTimeOffset DateTimeOffset { get; set; }
    public DateTime UtcDateTime { get; set; }
    public DateTime LocalDateTime { get; set; }
}

public class DateTimeToEpochResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty; // "Current Epoch" or "Epoch"
    public long Seconds { get; set; }
    public long Milliseconds { get; set; }
}

public class CalendarConvertResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string ConvertedDate { get; set; } = string.Empty;
}

public class CalculateDifferenceResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public double TotalDays { get; set; }
    public double TotalHours { get; set; }
    public double TotalMinutes { get; set; }
    public double TotalSeconds { get; set; }
    public int Days { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }
}

public class ConvertTimezoneResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string SourceTimezone { get; set; } = string.Empty;
    public DateTime SourceTime { get; set; }
    public string TargetTimezone { get; set; } = string.Empty;
    public DateTime TargetTime { get; set; }
}

public class ParseIso8601Result
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Iso8601String { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
    public int Second { get; set; }
    public int Millisecond { get; set; }
    public TimeSpan Offset { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int WeekOfYear { get; set; }
    public int DayOfYear { get; set; }
    public bool IsUtc { get; set; }
}

public class AddDurationResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public DateTimeOffset Input { get; set; }
    public string Duration { get; set; } = string.Empty;
    public DateTimeOffset Result { get; set; }
}

public class CountWorkingDaysResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DayOfWeek StartDayOfWeek { get; set; }
    public DateTime EndDate { get; set; }
    public DayOfWeek EndDayOfWeek { get; set; }
    public int TotalDays { get; set; }
    public int WeekendDays { get; set; }
    public int WorkingDays { get; set; }
}
