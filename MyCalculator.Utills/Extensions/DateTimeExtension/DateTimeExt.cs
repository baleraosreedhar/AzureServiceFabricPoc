using System;

namespace MyUtils.Extensions.DateTimeExtension
{
    /// <summary>
    /// </summary>
    public static class DateTimeExt
    {
        /// <summary>
        ///     To the time stamp.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime self)
        {
            return DateTimeToStamp(self);
        }

        /// <summary>
        ///     Froms the time stamp.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="timeStamp">The time stamp.</param>
        /// <returns></returns>
        public static DateTime FromTimeStamp(this DateTime self, long timeStamp)
        {
            var dtStart = new DateTime(1970, 1, 1);
            var tmp = timeStamp.ToString();
            if (tmp.Length == 10)
                timeStamp *= 10000000;
            else
                timeStamp *= 10000;
            var toNow = new TimeSpan(timeStamp);
            return dtStart.Add(toNow);
        }

        /// <summary>
        ///     Dates the time to stamp.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static long DateTimeToStamp(DateTime time)
        {
            var startTime = new DateTime(1970, 1, 1);
            return (long) (time - startTime).TotalMilliseconds;
        }


    }
}