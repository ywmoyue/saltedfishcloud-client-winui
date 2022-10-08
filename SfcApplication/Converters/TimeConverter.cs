using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
