using System;

namespace Helpers.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime SystemDateTimeNow => DateTime.UtcNow;

        public static long ToUnixTimeStamp(this DateTime dateTime)
        {
            return (long)(dateTime - DateTime.UnixEpoch).TotalSeconds;
        }

        public static DateTime FromUnixTimeStamp(this long unixEpoch)
        {
            return DateTime.UnixEpoch.AddSeconds(unixEpoch);
        }
    }
}
