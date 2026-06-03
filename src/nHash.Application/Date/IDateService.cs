using nHash.Application.Date.Models;

namespace nHash.Application.Date;

public interface IDateService
{
    EpochToDateTimeResult EpochToDateTime(long epochValue, bool isMilliseconds);
    DateTimeToEpochResult DateTimeToEpoch(string dateTimeStr);
    CalendarConvertResult ConvertCalendars(string dateTimeStr, string fromCalendar, string toCalendar);
    CalculateDifferenceResult CalculateDifference(string startStr, string endStr);
    ConvertTimezoneResult ConvertTimezone(string dateTimeStr, string fromTimezoneId, string toTimezoneId);
    ParseIso8601Result ParseIso8601(string iso8601String);
    AddDurationResult AddDuration(string dateTimeStr, string duration);
    CountWorkingDaysResult CountWorkingDays(string startStr, string endStr);
}
