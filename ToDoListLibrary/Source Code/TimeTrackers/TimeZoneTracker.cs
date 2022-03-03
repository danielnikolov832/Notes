namespace ToDoListLibrary.TimeTrackers
{
    public static class TimeZoneTracker
    {
        private readonly static object __lockObj = new object();

        // Needed as a dummy value, otherwize when type initialization happens (upon access of a static 
        // field or method) the '_timeZoneInfo' field will lead to a NullReferenceException, which will be
        // caught by the static constructor invocation API of the CLR and result in a TypeInitializationException
        // with the NullReferenceException as its inner exception. 
        private static readonly TimeZoneInfo dummyValueForCorrectTypeInitialization = TimeZoneInfo.Utc;

        // Do not modify directly, use getset_timeZoneInfo
        // The aftermentioned data access from database or other data holder must occur in the static constructor for this class
        private static TimeZoneInfo _timeZoneInfo  = dummyValueForCorrectTypeInitialization; // Get from some database or other data holder, then save on process termination

        public static event EventHandler<TimeZoneInfo>? OnTimeZoneInfoChanged;
        private static void Call_OnTimeZoneInfoChanged(TimeZoneInfo timeZoneInfo)
        {
            OnTimeZoneInfoChanged?.Invoke(null, timeZoneInfo);
        }

        public static TimeZoneInfo getset_timeZoneInfo
        {
            get => _timeZoneInfo;

            set
            {
                lock(__lockObj)
                {
                    SetTimeZoneInfo(value);
                }
            }
        }
        private static void SetTimeZoneInfo(TimeZoneInfo timeZoneInfo)
        {
            // Get value for '_timeZoneInfo' from some database or other data holder, then save on process termination
            // (aka. Serialize and deserialize the '_timeZoneInfo)

            if (IsTimeZoneInfoEqualToTheSavedOne(timeZoneInfo) == false)
            {
                _timeZoneInfo = timeZoneInfo;

                Call_OnTimeZoneInfoChanged(_timeZoneInfo);
            }
        }

        static TimeZoneTracker()
        {
            SetTimeZoneInfo(TimeZoneInfo.Local);
        }

        private static bool IsTimeZoneInfoEqualToTheSavedOne(TimeZoneInfo timeZoneInfo)
        {
            return _timeZoneInfo.BaseUtcOffset.Equals(timeZoneInfo.BaseUtcOffset);
        }
    }
}