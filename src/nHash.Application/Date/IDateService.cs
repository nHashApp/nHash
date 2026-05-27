namespace nHash.Application.Date;

public interface IDateService
{
    string EpochToDateTime(long epochValue, bool isMilliseconds);
    string DateTimeToEpoch(string dateTimeStr);
    string ConvertCalendars(string dateTimeStr, string fromCalendar, string toCalendar);
    string CalculateDifference(string startStr, string endStr);
    string ConvertTimezone(string dateTimeStr, string fromTimezoneId, string toTimezoneId);
}
