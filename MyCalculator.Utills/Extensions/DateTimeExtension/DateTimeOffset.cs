//using System;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using System.Text;

//namespace MyUtils.Extensions.DateTimeExtension
//{
//    [Serializable]
//    //[DataContract(Namespace = "")]
//    public struct DateTimeOffsetEx :
//       IComparable,
//       IComparable<DateTimeOffsetEx>,
//       IEquatable<DateTimeOffsetEx>,
//       IFormattable,
//       ISerializable
//    {

//        #region Data Members
//        DateTime _dt;
//        DateTime _utc;
//        TimeSpan? _offset;
//        string _fmt;
//        string _timeZoneName;
//        #endregion

//        #region Class Consts
//        public const string UtcTimeFormat = "GMT";
//        public static readonly DateTimeOffsetEx MinValue = new DateTimeOffsetEx(DateTime.MinValue.Ticks, null, null, null);
//        public static readonly DateTimeOffsetEx MaxValue = new DateTimeOffsetEx(DateTime.MaxValue.Ticks, null, null, null);
//        #endregion

//        #region Constructors
//        static DateTime calcUtc(DateTime dt, TimeSpan? offset)
//        {
//            if (offset.HasValue)
//                return new DateTime(dt.Ticks - offset.Value.Ticks); //to UTC
//            return dt;
//        }

//        /// <summary>
//        /// Initializes a new instance of the DateTimeOffsetEx structure using the specified DateTime value.
//        /// </summary>
//        /// <param name="dateTime"></param>
//        public DateTimeOffsetEx(DateTime dateTime, string timeFormat) :
//            this(dateTime, null, timeFormat, null)
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the DateTimeOffsetEx structure using the specified DateTime value and offset.
//        /// </summary>
//        /// <param name="dateTime">A date and time.</param>
//        /// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
//        public DateTimeOffsetEx(DateTime dateTime, TimeSpan? offset, string timeFormat) :
//            this(dateTime, offset, timeFormat, null)
//        {
//        }

//        public DateTimeOffsetEx(DateTime dateTime, TimeSpan? offset, string timeFormat, string timeZoneName)
//        {
//            //timezone name is provided
//            if (!string.IsNullOrEmpty(timeZoneName))
//            {
//                //always override this information based on the local date
//                //since date calc can change the utc offset based on daylight saving time
//                var ckOffset = OlsonTimeZone.GetUtcOffset(timeZoneName, dateTime);
//                if (offset != ckOffset)
//                {
//                    //offset is not correct probably due to daylight savings time
//                    offset = ckOffset;
//                    timeFormat = OlsonTimeZone.GetAbbreviation(timeZoneName, dateTime);
//                }
//            }

//            _dt = dateTime;
//            _offset = offset;
//            _fmt = timeFormat;
//            _timeZoneName = timeZoneName;
//            _utc = calcUtc(dateTime, offset);
//        }

//        /// <summary>
//        /// Initializes a new instance of the DateTimeOffsetEx structure using the specified DateTimeOffset value.
//        /// </summary>
//        /// <param name="dateTimeOffset"></param>
//        /// <param name="timeFormat"></param>
//        public DateTimeOffsetEx(DateTimeOffset dateTimeOffset, string timeFormat, string timeZoneName) :
//            this(dateTimeOffset.DateTime, dateTimeOffset.Offset, timeFormat, timeZoneName)
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the DateTimeOffsetEx structure using the specified number of ticks and offset.
//        /// </summary>
//        /// <param name="ticks">A date and time expressed as the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight on January 1, 0001.</param>
//        /// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
//        public DateTimeOffsetEx(long ticks, TimeSpan? offset, string timeFormat, string timeZoneName) :
//            this(new DateTime(ticks), offset, timeFormat, timeZoneName)
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the DateTimeOffset structure using the specified 
//        /// year, month, day, hour, minute, second, and offset.
//        /// </summary>
//        /// <param name="year">The year (1 through 9999).</param>
//        /// <param name="month">The month (1 through 12).</param>
//        /// <param name="day">The day (1 through the number of days in month).</param>
//        /// <param name="hour">The hours (0 through 23). </param>
//        /// <param name="minute">The minutes (0 through 59).</param>
//        /// <param name="second">The seconds (0 through 59).</param>
//        /// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
//        public DateTimeOffsetEx(
//            int year,
//            int month,
//            int day,
//            int hour,
//            int minute,
//            int second,
//            TimeSpan? offset,
//            string timeFormat,
//            string timeZoneName
//            )
//            : this(new DateTime(year, month, day, hour, minute, second), offset, timeFormat, timeZoneName)
//        {
//        }


//        /// <summary>
//        /// Initializes a new instance of the DateTimeOffset structure using the specified 
//        /// year, month, day, hour, minute, second, millisecond, and offset.
//        /// </summary>
//        /// <param name="year">The year (1 through 9999).</param>
//        /// <param name="month">The month (1 through 12).</param>
//        /// <param name="day">The day (1 through the number of days in month).</param>
//        /// <param name="hour">The hours (0 through 23). </param>
//        /// <param name="minute">The minutes (0 through 59).</param>
//        /// <param name="second">The seconds (0 through 59).</param>
//        /// <param name="millisecond">The milliseconds (0 through 999).</param>
//        /// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
//        public DateTimeOffsetEx(
//            int year,
//            int month,
//            int day,
//            int hour,
//            int minute,
//            int second,
//            int millisecond,
//            TimeSpan? offset,
//            string timeFormat,
//            string timeZoneName
//            )
//            : this(new DateTime(year, month, day, hour, minute, second, millisecond), offset, timeFormat, timeZoneName)
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the DateTimeOffset structure using 
//        /// the specified year, month, day, hour, minute, second, millisecond, 
//        /// and offset of a specified calendar.
//        /// </summary>
//        /// <param name="year">The year (1 through 9999).</param>
//        /// <param name="month">The month (1 through 12).</param>
//        /// <param name="day">The day (1 through the number of days in month).</param>
//        /// <param name="hour">The hours (0 through 23). </param>
//        /// <param name="minute">The minutes (0 through 59).</param>
//        /// <param name="second">The seconds (0 through 59).</param>
//        /// <param name="millisecond">The milliseconds (0 through 999).</param>
//        /// <param name="calendar">The calendar that is used to interpret year, month, and day.</param>
//        /// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
//        public DateTimeOffsetEx(
//            int year,
//            int month,
//            int day,
//            int hour,
//            int minute,
//            int second,
//            int millisecond,
//            Calendar calendar,
//            TimeSpan? offset,
//            string timeFormat,
//            string timeZoneName
//            )
//            : this(new DateTime(year, month, day, hour, minute, second, millisecond, calendar), offset, timeFormat, timeZoneName)
//        {
//        }

//        private DateTimeOffsetEx(SerializationInfo info, StreamingContext context)
//        {

//            //get required properties
//            try
//            {
//                _dt = info.GetDateTime("DateTime");
//            }
//            catch
//            {
//                var s = info.GetString("DateTime");
//                _dt = DateTime.Parse(s);
//            }

//            _offset = new TimeSpan(0);

//            try
//            {
//                _offset = (TimeSpan?)info.GetValue("Offset", typeof(TimeSpan?));
//            }
//            catch (Exception ex)
//            {
//                if ((int)info.GetValue("Offset", typeof(int)) != 0)
//                    throw new Exception(ex.Message);
//            }

//            //recalc
//            _utc = calcUtc(_dt, _offset);

//            //initialize optional
//            _fmt = null;
//            _timeZoneName = null;

//            //get optional properties
//            bool useNow = false;
//            foreach (SerializationEntry entry in info)
//            {
//                if (StringComparer.Ordinal.Equals("TimeFormat", entry.Name))
//                {
//                    //time format. cannot cast it may be JToken from Newtonsoft
//                    //the serialization object does it right
//                    _fmt = info.GetString("TimeFormat");
//                }
//                else if (StringComparer.Ordinal.Equals("TimeZoneName", entry.Name))
//                {
//                    //zone name - see TimeFormat for same concept for why not to use a cast
//                    _timeZoneName = info.GetString("TimeZoneName");
//                }
//                else if (StringComparer.Ordinal.Equals("Now", entry.Name) && info.GetBoolean("Now") == true)
//                {
//                    //special ghost field 
//                    useNow = true;
//                }
//            }



//            //default time format
//            if (string.IsNullOrEmpty(_fmt))
//                _fmt = UtcTimeFormat;

//            //zone name
//            if (!string.IsNullOrEmpty(_timeZoneName))
//            {
//                _offset = OlsonTimeZone.GetUtcOffset(_timeZoneName, _dt);
//                _fmt = OlsonTimeZone.GetAbbreviation(_timeZoneName, _dt);
//                _utc = calcUtc(_dt, _offset);
//            }

//            //use now
//            if (useNow)
//                assignNow();
//        }
//        #endregion

//        #region IComparable Members
//        int IComparable.CompareTo(object obj)
//        {
//            return CompareTo((DateTimeOffsetEx)obj);
//        }
//        #endregion

//        #region IComparable<DateTimeOffsetEx> Members
//        /// <summary>
//        /// Compares the current DateTimeOffsetEx object to a specified 
//        /// DateTimeOffsetEx object and indicates whether the current object 
//        /// is earlier than, the same as, or later than the second DateTimeOffsetEx
//        /// object. 
//        /// </summary>
//        /// <param name="other"></param>
//        /// <returns></returns>
//        public int CompareTo(DateTimeOffsetEx other)
//        {
//            //if either is specified as time everywhere then
//            //compare using local time
//            if (!_offset.HasValue || !other._offset.HasValue)
//                return _dt.Ticks.CompareTo(other._dt.Ticks);

//            //convert to utc and compare
//            return UtcTicks.CompareTo(other.UtcTicks);
//        }

//        #endregion

//        #region IEquatable<DateTimeOffsetEx> Members

//        public bool Equals(DateTimeOffsetEx other)
//        {
//            return CompareTo(other) == 0;
//        }

//        #endregion

//        #region IFormattable Members

//        /// <summary>
//        /// Converts the value of the current DateTimeOffset object to its 
//        /// equivalent string representation using the specified format and 
//        /// culture-specific format information. 
//        /// </summary>
//        /// <param name="format"></param>
//        /// <param name="formatProvider"></param>
//        /// <returns></returns>
//        public string ToString(string format, IFormatProvider formatProvider)
//        {
//            if (_offset.HasValue)
//            {
//                string text = string.Empty;
//                try
//                {
//                    DateTimeOffset dtOff = new DateTimeOffset(_dt.Ticks, _offset.Value);
//                    text = dtOff.ToString(format, formatProvider);
//                }
//                catch { }


//                //check if time is part of the string
//                if (string.IsNullOrEmpty(_fmt) && text.IndexOf(':') > -1)
//                    text = string.Concat(text, " ", _fmt);

//                return text;
//            }
//            return _dt.ToString(format, formatProvider);
//        }

//        #endregion

//        #region ISerializable Members
//        [System.Security.SecurityCritical]
//        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
//        {
//            //issue with microsoft JSON serialization format, it factors in the host's timezone in the
//            //DateTime value. so we need to neutralize the effect
//            //info.AddValue("DateTime", DateTime.SpecifyKind(_dt, DateTimeKind.Utc) + (DateTime.UtcNow - DateTime.Now));
//            info.AddValue("DateTime", _dt.ToString("s"));

//            info.AddValue("Offset", _offset);
//            info.AddValue("TimeFormat", _fmt);
//            info.AddValue("TimeZoneName", _timeZoneName);
//        }

//        #endregion

//        #region Object overrides
//        public override bool Equals(object obj)
//        {
//            if (obj is DateTimeOffsetEx)
//                return Equals((DateTimeOffsetEx)obj);

//            return false;
//        }

//        public override int GetHashCode()
//        {
//            return _utc.GetHashCode();
//        }

//        public override string ToString()
//        {
//            return _dt.ToString();
//            //throw new NotImplementedException();
//        }
//        #endregion

//        #region Properties
//        /// <summary>
//        /// Gets a DateTime value that represents the date component of the current DateTimeOffsetEx object. 
//        /// </summary>
//        public DateTime Date
//        {
//            get { return _dt.Date; }
//        }

//        /// <summary>
//        /// Gets a DateTime value that represents the date and time of the current DateTimeOffsetEx object. 
//        /// </summary>
//        [DataMember(Name = "DateTime", IsRequired = true)]
//        [XmlElement("DateTime")]
//        public DateTime DateTime
//        {
//            get { return _dt; }

//            //fix for serialization
//            private set { _dt = value; }
//        }

//        /// <summary>
//        /// Gets a DateTimeOffset value.
//        /// </summary>
//        public DateTimeOffset DateTimeOffset
//        {
//            get
//            {
//                return new DateTimeOffset(_dt.Ticks, _offset.HasValue ? _offset.Value : TimeSpan.Zero);
//            }
//        }

//        /// <summary>
//        /// Gets the day of the month represented by the current DateTimeOffsetEx object. 
//        /// </summary>
//        public int Day
//        {
//            get { return _dt.Day; }
//        }

//        /// <summary>
//        /// Gets the day of the week represented by the current DateTimeOffsetEx object. 
//        /// </summary>
//        public DayOfWeek DayOfWeek
//        {
//            get { return _dt.DayOfWeek; }
//        }

//        /// <summary>
//        /// Gets the day of the year represented by the current DateTimeOffsetEx object. 
//        /// </summary>
//        public int DayOfYear
//        {
//            get { return _dt.DayOfYear; }
//        }

//        /// <summary>
//        /// Gets or sets the time format (the abbreviated time zone code). 
//        /// </summary>
//        [DataMember(Name = "TimeFormat")]
//        [XmlElement("TimeFormat")]
//        public string TimeFormat
//        {
//            get { return _fmt ?? UtcTimeFormat; }
//            set { _fmt = value; }
//        }

//        /// <summary>
//        /// Gets or sets the time zone name.
//        /// </summary>
//        [DataMember(Name = "TimeZoneName")]
//        [XmlElement("TimeZoneName")]
//        public string TimeZoneName
//        {
//            get { return _timeZoneName; }
//            set { _timeZoneName = value; }
//        }

//        /// <summary>
//        /// Gets the hour component of the time represented by the current DateTimeOffsetEx object. 
//        /// </summary>
//        public int Hour
//        {
//            get { return _dt.Hour; }
//        }

//        /// <summary>
//        /// Gets a DateTime value that represents the local date and time of the client endpoint.
//        /// </summary>
//        public DateTime LocalDateTime
//        {
//            //always local
//            get { return _dt; }
//        }

//        /// <summary>
//        /// Gets the millisecond component of the time represented by the current DateTimeOffsetEx object. 
//        /// </summary>
//        public int Millisecond
//        {
//            get { return _dt.Millisecond; }
//        }

//        /// <summary>
//        /// Gets the minute component of the time represented by the current DateTimeOffsetEx object. 
//        /// </summary>

//        public int Minute
//        {
//            get { return _dt.Minute; }
//        }

//        /// <summary>
//        /// Gets the month component of the date represented by the current DateTimeOffsetEx object. 
//        /// </summary>

//        public int Month
//        {
//            get { return _dt.Month; }
//        }

//        /// <summary>
//        /// Gets the time's offset from Coordinated Universal Time (UTC). 
//        /// A null offset means local everywhere.
//        /// </summary>
//        [DataMember(Name = "Offset", IsRequired = true)]
//        [XmlElement("Offset")]
//        public TimeSpan? Offset
//        {
//            get { return _offset; }

//            //fix for serialization
//            private set { _offset = value; }
//        }

//        /// <summary>
//        /// Gets the second component of the clock time represented by the current DateTimeOffsetEx object. 
//        /// </summary>
//        public int Second
//        {
//            get { return _dt.Second; }
//        }

//        /// <summary>
//        /// Gets the number of ticks that represents the date and time of the current DateTimeOffsetEx
//        /// object in clock time. 
//        /// </summary>
//        public long Ticks
//        {
//            get { return _dt.Ticks; }
//        }

//        /// <summary>
//        /// Gets the time of day for the current DateTimeOffsetEx object. 
//        /// </summary>
//        public TimeSpan TimeOfDay
//        {
//            get { return _dt.TimeOfDay; }
//        }

//        /// <summary>
//        /// Gets a DateTime value that represents the Coordinated Universal Time (UTC) date 
//        /// and time of the current DateTimeOffsetEx object. 
//        /// </summary>
//        public DateTime UtcDateTime
//        {
//            get
//            {
//                return new DateTime(UtcTicks, DateTimeKind.Utc);
//            }
//        }

//        /// <summary>
//        /// Gets the number of ticks that represents the date and time of the current 
//        /// DateTimeOffsetEx object in Coordinated Universal Time (UTC). 
//        /// </summary>
//        public long UtcTicks
//        {
//            get
//            {
//                return _utc.Ticks;
//            }
//        }

//        /// <summary>
//        /// Gets the year component of the date represented by the current DateTimeOffsetEx object. 
//        /// </summary>
//        public int Year
//        {
//            get { return _dt.Year; }
//        }

//        /// <summary>
//        /// Shadow property for setting server's current date/time.
//        /// Also see deserialization ctor.
//        /// </summary>
//        [DataMember(Name = "Now", IsRequired = false)]
//        [XmlElement("Now")]
//        private bool Now
//        {
//            set
//            {
//                if (value)
//                    assignNow();
//            }
//        }
//        #endregion

//        #region Methods
//        /// <summary>
//        /// Creates DateTimeOffsetEx with just the date portion, without the time.
//        /// </summary>
//        /// <returns></returns>
//        public DateTimeOffsetEx DateWithoutTime()
//        {
//            //already midnight
//            if (_dt.TimeOfDay.Ticks == 0)
//                return this;

//            return new DateTimeOffsetEx(Date, _offset, _fmt, _timeZoneName);
//        }

//        /// <summary>
//        /// This returns true date without time format information (EDT, EST)
//        /// This is to use instead of DateWithoutTime method
//        /// </summary>
//        /// <returns></returns>
//        public DateTime DateOnly()
//        {
//            return new DateTimeOffsetEx(Date, _offset, _fmt, _timeZoneName).Date;
//        }

//        /// <summary>
//        /// Creates DateTimeOffsetEx for midnight of the instance's date.
//        /// </summary>
//        /// <returns></returns>
//        public DateTimeOffsetEx StartOfDay()
//        {
//            return DateWithoutTime();
//        }

//        /// <summary>
//        /// Creates DateTimeOffsetEx for noon (12pm) of the instance's date.
//        /// </summary>
//        /// <returns></returns>
//        public DateTimeOffsetEx Noon()
//        {
//            //already noon
//            if (_dt.TimeOfDay.TotalHours == 12.0)
//                return this;

//            return new DateTimeOffsetEx(
//                Year,
//                Month,
//                Day,
//                12,
//                0,
//                0,
//                0,
//                _offset,
//                _fmt,
//                _timeZoneName
//                );
//        }

//        /// <summary>
//        /// Creates DateTimeOffsetEx that is the closest to next day's midnight
//        /// but it's still considered the same day
//        /// </summary>
//        /// <returns></returns>
//        public DateTimeOffsetEx EndOfDay()
//        {
//            //the 3 millisecond is sql's DATETIME column resolution
//            //going closer may round to the start of next day
//            return new DateTimeOffsetEx(
//                Year,
//                Month,
//                Day,
//                23,
//                59,
//                59,
//                997,
//                _offset,
//                _fmt,
//                _timeZoneName
//                );
//        }

//        /// <summary>
//        /// Creates DateTimeOffsetEx that represents the start (midnight) of the next day.
//        /// </summary>
//        /// <returns></returns>
//        public DateTimeOffsetEx StartOfNextDay()
//        {
//            return new DateTimeOffsetEx(Date.AddDays(1), _offset, _fmt, _timeZoneName);
//        }

//        /// <summary>
//        /// Creates a DateTimePeriod object that represents a period of one day based on 
//        /// date of the instance.
//        /// </summary>
//        /// <returns></returns>
//        public DateTimePeriod DayTimePeriod()
//        {
//            if (!string.IsNullOrEmpty(this.TimeZoneName))
//                return DayDateTimePeriod.Create(this.DateTime, this.TimeZoneName);
//            return DayDateTimePeriod.Create(this);
//        }

//        /// <summary>
//        /// Adds a specified time interval to a DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="timeSpan"></param>
//        /// <returns></returns>
//        [Pure]
//        public DateTimeOffsetEx Add(TimeSpan timeSpan)
//        {
//            return new DateTimeOffsetEx(_dt.Ticks + timeSpan.Ticks, _offset, _fmt, _timeZoneName);
//        }

//        /// <summary>
//        /// Adds a specified number of whole and fractional days to the current DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="days"></param>
//        /// <returns></returns>
//        [Pure]
//        public DateTimeOffsetEx AddDays(double days)
//        {
//            return Add(TimeSpan.FromDays(days));
//        }

//        /// <summary>
//        /// Adds a specified number of whole and fractional hours to the current DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="hours"></param>
//        /// <returns></returns>
//        [Pure]
//        public DateTimeOffsetEx AddHours(double hours)
//        {
//            return Add(TimeSpan.FromHours(hours));
//        }

//        /// <summary>
//        /// Adds a specified number of milliseconds to the current DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="milliseconds"></param>
//        /// <returns></returns>
//        [Pure]
//        public DateTimeOffsetEx AddMilliseconds(double milliseconds)
//        {
//            return Add(TimeSpan.FromMilliseconds(milliseconds));
//        }

//        /// <summary>
//        /// Adds a specified number of whole and fractional minutes to the current DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="minutes"></param>
//        /// <returns></returns>
//        [Pure]
//        public DateTimeOffsetEx AddMinutes(double minutes)
//        {
//            return Add(TimeSpan.FromMinutes(minutes));
//        }

//        /// <summary>
//        /// Adds a specified number of months to the current DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="months"></param>
//        /// <returns></returns>
//        [Pure]
//        public DateTimeOffsetEx AddMonths(int months)
//        {
//            return new DateTimeOffsetEx(_dt.AddMonths(months).Ticks, _offset, _fmt, _timeZoneName);
//        }

//        /// <summary>
//        /// Adds a specified number of weeks to teh current DateTimeOffsetEx object
//        /// </summary>
//        /// <param name="weeks"></param>
//        /// <returns></returns>
//        [Pure]
//        public DateTimeOffsetEx AddWeeks(int weeks)
//        {
//            return AddDays(weeks * 7);
//        }
//        /// <summary>
//        /// Adds a specified number of whole and fractional seconds to the current DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="seconds"></param>
//        /// <returns></returns>
//        [Pure]
//        public DateTimeOffsetEx AddSeconds(double seconds)
//        {
//            return Add(TimeSpan.FromSeconds(seconds));
//        }

//        /// <summary>
//        /// Adds a specified number of ticks to the current DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="ticks"></param>
//        /// <returns></returns>
//        [Pure]
//        public DateTimeOffsetEx AddTicks(long ticks)
//        {
//            return Add(TimeSpan.FromTicks(ticks));
//        }

//        /// <summary>
//        /// Adds a specified number of years to the DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="years"></param>
//        /// <returns></returns>
//        [Pure]
//        public DateTimeOffsetEx AddYears(int years)
//        {
//            return new DateTimeOffsetEx(_dt.AddYears(years).Ticks, _offset, _fmt, _timeZoneName);
//        }

//        /// <summary>
//        /// Determines whether the current DateTimeOffsetEx object represents the same 
//        /// time, same offset, same time format, and the same time zone name
//        /// as a specified DateTimeOffset object. 
//        /// </summary>
//        /// <param name="other"></param>
//        /// <returns></returns>
//        public bool EqualsExact(DateTimeOffsetEx other)
//        {
//            return _dt.Ticks == other.Ticks &&
//                _offset == other._offset &&
//                StringComparer.InvariantCultureIgnoreCase.Equals(_fmt, other._fmt) &&
//                StringComparer.InvariantCultureIgnoreCase.Equals(_timeZoneName, other._timeZoneName);
//        }

//        /// <summary>
//        /// Tests if this instances matches the same date (day resolution) as the provided parameter
//        /// </summary>
//        public bool EqualsDate(DateTimeOffsetEx other)
//        {
//            var otherMidDay = other.Noon();
//            return otherMidDay >= StartOfDay() && otherMidDay < StartOfNextDay();
//        }

//        /// <summary>
//        /// Converts the specified string representation of a date, time, and offset 
//        /// to its DateTimeOffsetEx equivalent. 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx Parse(string input)
//        {
//            throw new NotImplementedException();
//            //return new DateTimeOffsetEx(DateTimeOffset.Parse(input), null);
//        }

//        /// <summary>
//        /// Converts the specified string representation of a date and time to its 
//        /// DateTimeOffsetEx equivalent using the specified culture-specific format 
//        /// information. 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="formatProvider"></param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx Parse(string input, IFormatProvider formatProvider)
//        {
//            throw new NotImplementedException();
//            //return new DateTimeOffsetEx(DateTimeOffset.Parse(input, formatProvider), null);
//        }

//        /// <summary>
//        /// Converts the specified string representation of a date and time to its 
//        /// DateTimeOffsetEx equivalent using the specified culture-specific format 
//        /// information and formatting style. 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="formatProvider"></param>
//        /// <param name="styles"></param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx Parse(
//            string input,
//            IFormatProvider formatProvider,
//            DateTimeStyles styles
//            )
//        {
//            throw new NotImplementedException();
//            //return new DateTimeOffsetEx(DateTimeOffset.Parse(input, formatProvider, styles), null);
//        }

//        /// <summary>
//        /// Converts the specified string representation of a date and time to its 
//        /// DateTimeOffsetEx equivalent using the specified format and culture-specific 
//        /// format information. The format of the string representation must match 
//        /// the specified format exactly. 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="format"></param>
//        /// <param name="formatProvider"></param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx ParseExact(
//            string input,
//            string format,
//            IFormatProvider formatProvider
//            )
//        {
//            throw new NotImplementedException();
//            //return new DateTimeOffsetEx(DateTimeOffset.ParseExact(input, format, formatProvider), null);
//        }

//        /// <summary>
//        /// Converts the specified string representation of a date and time to its 
//        /// DateTimeOffsetEx equivalent using the specified format, culture-specific 
//        /// format information, and style. The format of the string representation must 
//        /// match the specified format exactly. 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="format"></param>
//        /// <param name="formatProvider"></param>
//        /// <param name="styles"></param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx ParseExact(
//            string input,
//            string format,
//            IFormatProvider formatProvider,
//            DateTimeStyles styles
//            )
//        {
//            throw new NotImplementedException();
//            //return new DateTimeOffsetEx(DateTimeOffset.ParseExact(input, format, formatProvider, styles), null);
//        }

//        /// <summary>
//        /// Converts the specified string representation of a date and time to its DateTimeOffset equivalent 
//        /// using the specified formats, culture-specific format information, and style. The format of the 
//        /// string representation must match one of the specified formats exactly.
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="formats"></param>
//        /// <param name="formatProvider"></param>
//        /// <param name="styles"></param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx ParseExact(
//            string input,
//            string[] formats,
//            IFormatProvider formatProvider,
//            DateTimeStyles styles
//            )
//        {
//            throw new NotImplementedException();
//            //return new DateTimeOffsetEx(DateTimeOffset.ParseExact(input, formats, formatProvider, styles), null);
//        }

//        /// <summary>
//        /// Subtracts a DateTimeOffset value that represents a specific date and time from the current DateTimeOffset object. 
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        public TimeSpan Subtract(DateTimeOffsetEx value)
//        {
//            return _utc.Subtract(value._utc);
//        }

//        public DateTimeOffsetEx Subtract(TimeSpan value)
//        {
//            return Add(TimeSpan.FromTicks(-value.Ticks));
//        }

//        /// <summary>
//        /// Converts the value of the current DateTimeOffsetEx object to a Windows file time. 
//        /// </summary>
//        /// <returns></returns>
//        public long ToFileTime()
//        {
//            return _dt.ToFileTime();
//        }

//        /// <summary>
//        /// The DateTimeOffsetEx is already in the local time-zone where it was
//        /// created in. This method is a no-op.
//        /// </summary>
//        /// <returns></returns>
//        public DateTimeOffsetEx ToLocalTime()
//        {
//            return this;
//        }


//        /// <summary>
//        /// Converts the value of the current DateTimeOffsetEx object to its 
//        /// equivalent string representation using the specified format. 
//        /// </summary>
//        /// <param name="format"></param>
//        /// <returns></returns>
//        public string ToString(string format)
//        {
//            return ToString(format, CultureInfo.CurrentUICulture.DateTimeFormat);
//        }


//        /// <summary>
//        /// Converts the current DateTimeOffset object to a 
//        /// DateTimeOffsetEx value that represents the Coordinated 
//        /// Universal Time (UTC). 
//        /// </summary>
//        /// <returns></returns>
//        public DateTimeOffsetEx ToUniversalTime()
//        {
//            //already UTC
//            if (_offset == null || _offset.Value == TimeSpan.Zero)
//                return this;

//            //convert to UTC
//            return new DateTimeOffsetEx(
//                UtcTicks,
//                TimeSpan.Zero,
//                UtcTimeFormat,
//                UtcTimeFormat
//                );
//        }

//        /// <summary>
//        /// The start of a central place to compare two datetimeoffsetex where we
//        /// relly just care about the date portion, regardless of timezone, hours, minutes, format....
//        /// </summary>
//        /// <param name="date1"></param>
//        /// <param name="date2"></param>
//        /// <returns></returns>
//        public static bool SameDateOnly(DateTimeOffsetEx date1, DateTimeOffsetEx date2)
//        {
//            return (date1.DateOnly() == date2.DateOnly());
//        }

//        /// <summary>
//        /// Tries to converts a specified string representation of 
//        /// a date and time to its DateTimeOffsetExs equivalent, and 
//        /// returns a value that indicates whether the conversion succeeded. 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="result"></param>
//        /// <returns></returns>
//        public static bool TryParse(string input, out DateTimeOffsetEx result)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// Tries to convert a specified string representation of a date and time 
//        /// to its DateTimeOffsetEx equivalent, and returns a value that indicates 
//        /// whether the conversion succeeded. 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="formatProvider"></param>
//        /// <param name="styles"></param>
//        /// <param name="result"></param>
//        /// <returns></returns>
//        public static bool TryParse(
//            string input,
//            IFormatProvider formatProvider,
//            DateTimeStyles styles,
//            out DateTimeOffsetEx result
//            )
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// Converts the specified string representation of a date and time to its 
//        /// DateTimeOffsetEx equivalent using the specified format, culture-specific 
//        /// format information, and style. The format of the string representation 
//        /// must match the specified format exactly. 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="format"></param>
//        /// <param name="formatProvider"></param>
//        /// <param name="styles"></param>
//        /// <param name="result"></param>
//        /// <returns></returns>
//        public static bool TryParseExact(
//            string input,
//            string format,
//            IFormatProvider formatProvider,
//            DateTimeStyles styles,
//            out DateTimeOffset result
//            )
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// Converts the specified string representation of a date and time to its 
//        /// DateTimeOffsetEx equivalent using the specified array of formats, 
//        /// culture-specific format information, and style. The format of 
//        /// the string representation must match one of the specified formats exactly. 
//        /// </summary>
//        /// <param name="input"></param>
//        /// <param name="formats"></param>
//        /// <param name="formatProvider"></param>
//        /// <param name="styles"></param>
//        /// <param name="result"></param>
//        /// <returns></returns>
//        public static bool TryParseExact(
//            string input,
//            string[] formats,
//            IFormatProvider formatProvider,
//            DateTimeStyles styles,
//            out DateTimeOffset result
//            )
//        {
//            throw new NotImplementedException();
//        }

//        public static DateTimeOffsetEx UtcNow()
//        {
//            return new DateTimeOffsetEx(
//                DateTime.UtcNow,
//                TimeSpan.Zero,
//                UtcTimeFormat,
//                UtcTimeFormat
//                );
//        }

//        private void assignNow()
//        {
//            _utc = DateTime.UtcNow;
//            long offsetTicks = _offset.HasValue ? _offset.Value.Ticks : 0;
//            _dt = new DateTime(_utc.Ticks + offsetTicks);
//        }
//        #endregion

//        #region Operators
//        /// <summary>
//        /// Adds a specified time interval to a DateTimeOffsetEx object that has 
//        /// a specified date and time, and yields a DateTimeOffset object that 
//        /// has new a date and time. 
//        /// </summary>
//        /// <param name="dt"></param>
//        /// <param name="timeSpan"></param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx operator +(DateTimeOffsetEx dt, TimeSpan timeSpan)
//        {
//            return dt.Add(timeSpan);
//        }

//        /// <summary>
//        /// Subtracts one DateTimeOffsetEx object from another and yields a time interval. 
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="y"></param>
//        /// <returns></returns>
//        public static TimeSpan operator -(DateTimeOffsetEx x, DateTimeOffsetEx y)
//        {
//            return x.Subtract(y);
//        }

//        /// <summary>
//        /// Subtracts a specified time interval from a specified date and time, and yields a new date and time.
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="timeSpan"></param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx operator -(DateTimeOffsetEx x, TimeSpan timeSpan)
//        {
//            return x.Subtract(timeSpan);
//        }

//        /// <summary>
//        /// Determines whether two specified DateTimeOffsetEx objects represent the same point in time. 
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="y"></param>
//        /// <returns></returns>
//        public static bool operator ==(DateTimeOffsetEx x, DateTimeOffsetEx y)
//        {
//            return x.Equals(y);
//        }

//        /// <summary>
//        /// Determines whether two specified DateTimeOffsetEx objects refer to 
//        /// different points in time. 
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="y"></param>
//        /// <returns></returns>
//        public static bool operator !=(DateTimeOffsetEx x, DateTimeOffsetEx y)
//        {
//            return !x.Equals(y);
//        }


//        /// <summary>
//        /// Determines whether one specified DateTimeOffsetEx object is greater 
//        /// than (or later than) a second specified DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="y"></param>
//        /// <returns></returns>
//        public static bool operator >(DateTimeOffsetEx x, DateTimeOffsetEx y)
//        {
//            return x.CompareTo(y) > 0;
//        }

//        /// <summary>
//        /// Determines whether one specified DateTimeOffsetEx object is greater 
//        /// than or equal to a second specified DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="y"></param>
//        /// <returns></returns>
//        public static bool operator >=(DateTimeOffsetEx x, DateTimeOffsetEx y)
//        {
//            return x.CompareTo(y) >= 0;
//        }


//        /// <summary>
//        /// Determines whether one specified DateTimeOffsetEx object is less 
//        /// than a second specified DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="y"></param>
//        /// <returns></returns>
//        public static bool operator <(DateTimeOffsetEx x, DateTimeOffsetEx y)
//        {
//            return x.CompareTo(y) < 0;
//        }

//        /// <summary>
//        /// Determines whether one specified DateTimeOffsetEx object is less 
//        /// than a second specified DateTimeOffsetEx object. 
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="y"></param>
//        /// <returns></returns>
//        public static bool operator <=(DateTimeOffsetEx x, DateTimeOffsetEx y)
//        {
//            return x.CompareTo(y) <= 0;
//        }
//        #endregion

//    }

//}
