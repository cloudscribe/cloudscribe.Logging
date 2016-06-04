using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Logging.Web
{
    public static class TimeZoneExtensions
    {
        public static DateTime ToLocalTime(this DateTime utcDate, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(utcDate, DateTimeKind.Utc), timeZone);
            
        }
    }
}
