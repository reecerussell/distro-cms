using System;

namespace Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static double Unix(this DateTime time)
        {
            var zeroPoint = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return time.Subtract(zeroPoint).TotalSeconds;
        }

        public static double Unix(this DateTimeOffset time)
        {
            var zeroPoint = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return time.Subtract(zeroPoint).TotalSeconds;
        }
    }
}
