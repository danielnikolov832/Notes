using System.Collections;

namespace ToDoListLibrary.TimeTrackers
{
    public static class TimeZoneConverter
    {

        static TimeZoneConverter()
        {
            TimeZoneTracker.OnTimeZoneInfoChanged += TimeZoneTracker_OnTimeZoneInfoChanged;
        }

        private static void TimeZoneTracker_OnTimeZoneInfoChanged(object? sender, TimeZoneInfo e)
        {
            // SetDateTimesOfNotesToTimeZoneInfo(e, ) // Argument limitedTimeNoteEnumerable must be gotten from database or other data holder
            // It must contain all LimitedTimeNote instances that the user has created
        }

        public static void SetDateTimesOfNotesToTimeZoneInfo(TimeZoneInfo timeZoneInfo, IEnumerable<LimitedTimeNote> limitedTimeNoteEnumerable)
        {
            foreach (LimitedTimeNote limitedTimeNoteItem in limitedTimeNoteEnumerable)
            {
                DateTimeOffset finalTimeOfLimitedTimeNoteItem = limitedTimeNoteItem.getset_finalTime;

                finalTimeOfLimitedTimeNoteItem = GetDateTimeOffsetForAGivenTimeZoneInfo(finalTimeOfLimitedTimeNoteItem, timeZoneInfo);
            }
        }

        // Gets a DateTimeOffset object, representing the same point in time as the 'dateTimeOffset' parameter, but 
        // in time zone specified by the 'timeZoneInfo' parameter.
        private static DateTimeOffset GetDateTimeOffsetForAGivenTimeZoneInfo(DateTimeOffset dateTimeOffset, TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTime(dateTimeOffset, timeZoneInfo);
        }
    }
}