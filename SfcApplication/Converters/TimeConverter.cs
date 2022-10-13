using System;

namespace SfcApplication.Converters
{
    public class TimeConverter
    {
        public static string EstimatedRemainingTimeToString(TimeSpan time)
        {
            if (time == TimeSpan.MaxValue) return "--:--:--";
            return $"{(int)time.TotalHours}:{time.Minutes}:{time.Seconds}";
        }
    }
}
