//using System;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using System.Text;

//namespace MyUtils.Extensions.DateTimeExtension
//{
//    public interface ITzTimeZoneSource
//    {
//        IList<ITzTimeZone> ZoneList { get; }
//        ITzTimeZone GetTimeZone(string tzName);
//    }

//    public interface ITzTimeZone
//    {
//        string StandardName { get; }

//        TimeSpan GetUtcOffset(DateTime time);
//        string GetAbbreviation(DateTime time);
//    }

//    public class TzTimeZoneSource : ITzTimeZoneSource
//    {
//        public ReadOnlyCollection<TzTimeZone.TzZoneInfo> ZoneInfoList
//        {
//            get { return TzTimeZone.ZoneList; }
//        }

//        public IList<ITzTimeZone> ZoneList
//        {
//            get
//            {
//                return ZoneInfoList.Select(TzTimeZoneWrapper.Wrap).ToList<ITzTimeZone>();
//            }
//        }

//        public ITzTimeZone GetTimeZone(string tzName)
//        {
//            return TzTimeZoneWrapper.Wrap(TzTimeZone.GetTimeZone(tzName));
//        }
//    }
//    public static class TzTimeZoneProvider
//    {
//        private static ITzTimeZoneSource _timeZoneSource = null;

//        public static ITzTimeZoneSource Instance
//        {
//            get
//            {
//                if (_timeZoneSource == null)
//                {
//                    _timeZoneSource = new TzTimeZoneSource();
//                }
//                return _timeZoneSource;
//            }
//        }

//        public static IList<ITzTimeZone> ZoneList
//        {
//            get { return Instance.ZoneList; }
//        }

//        public static ITzTimeZone GetTimeZone(string tzName)
//        {
//            return Instance.GetTimeZone(tzName);
//        }

//        public static void SetInstance(ITzTimeZoneSource newSource)
//        {
//            _timeZoneSource = newSource;
//        }
//    }
//    [DataContract(Namespace = "")]
//    [Serializable]
//    public class OlsonTimeZone
//    {
//        #region Data Members
//        static readonly IList<ITzTimeZone> _allTimeZones;

//        ITzTimeZone _tz;
//        string _zone;
//        string _region;
//        string _location;
//        #endregion

//        #region Constructors
//        static OlsonTimeZone()
//        {
//            _allTimeZones = TzTimeZoneProvider.ZoneList;
//        }

//        public OlsonTimeZone(string zoneName)
//        {
//            //get timezone
//            var tz = TzTimeZoneProvider.GetTimeZone(zoneName);
//            if (tz == null)
//                throw new ArgumentException("Invalid time zone name", "zoneName");

//            initialize(tz);
//        }

//        private OlsonTimeZone(ITzTimeZone tz)
//        {
//            initialize(tz);
//        }

//        #endregion

//        #region Properties
//        [DataMember]
//        public double UtcOffset
//        {
//            get
//            {
//                //Utc offset as of now
//                return _tz.GetUtcOffset(DateTime.Now).TotalHours;
//            }
//        }

//        [DataMember]
//        public string Name
//        {
//            get { return _tz.StandardName; }
//        }

//        [DataMember]
//        public string Zone
//        {
//            get { return _zone; }
//        }

//        [DataMember]
//        public string Region
//        {
//            get { return _region; }
//        }

//        [DataMember]
//        public string Location
//        {
//            get { return _location; }
//        }

//        [DataMember]
//        public string Abbreviation
//        {
//            get { return _tz.GetAbbreviation(DateTime.Now); }
//        }
//        #endregion

//        #region Class Methods
//        public static OlsonTimeZone GetZone(string name)
//        {
//            return new OlsonTimeZone(name);
//        }

//        public static double GetUtcOffSet(string zoneName)
//        {
//            return GetUtcOffset(zoneName, DateTime.UtcNow).TotalHours;
//        }

//        public static string GetAbbreviation(string zoneName, DateTime dateTime)
//        {
//            if (string.IsNullOrEmpty(zoneName))
//                zoneName = "GMT";

//            //get time-zone
//            var tz = TzTimeZoneProvider.GetTimeZone(zoneName);
//            if (tz == null)
//                throw new ArgumentException("Invalid time-zone name.", "zoneName");

//            return tz.GetAbbreviation(dateTime);
//        }

//        public static TimeSpan GetUtcOffset(string zoneName, DateTime dateTime)
//        {
//            if (string.IsNullOrEmpty(zoneName))
//                zoneName = "GMT";

//            //get time-zone
//            var tz = TzTimeZoneProvider.GetTimeZone(zoneName);
//            if (tz == null)
//                throw new ArgumentException("Invalid time-zone name.", "zoneName");

//            //return offset
//            return tz.GetUtcOffset(dateTime);
//        }

//        public static List<OlsonTimeZone> GetAllTimeZones()
//        {
//            return TimeZones.ToList();
//        }

//        private static IList<OlsonTimeZone> TimeZones => LazyTimeZones.Value;

//        private static Lazy<IList<OlsonTimeZone>> LazyTimeZones { get; }
//                    = new Lazy<IList<OlsonTimeZone>>(() => _allTimeZones.Select(t => new OlsonTimeZone(t)).ToList().AsReadOnly());

//        /// <summary>
//        /// Compatiblity leftover. Use NowLocal(zoneName).
//        /// </summary>
//        /// <param name="zoneName"></param>
//        /// <param name="UtcOffsetHours"></param>
//        /// <returns></returns>
//        [Obsolete("Use NowLocal(string)")]
//        public static DateTimeOffsetEx NowLocal(string zoneName, double UtcOffsetHours)
//        {
//            //don't make sense to honor UtcOffsetHours param. our time zone class
//            //knows better what offset to use (whole point of having the Tz Database)
//            return NowLocal(zoneName);
//        }

//        /// <summary>
//        /// Creates the DateTimeOffsetEx based on the current local time in the
//        /// specified time zone.
//        /// </summary>
//        /// <param name="zoneName">Standard name of the time-zone.</param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx NowLocal(string zoneName)
//        {
//            return LocalTime(DateTime.UtcNow, zoneName);
//        }

//        /// <summary>
//        /// Creates local DateTimeOffsetEx based on the specified time in the time-zone.
//        /// Used to retrieve historic time.
//        /// Fails with a ArgumentException if "zoneName" cannot be resolved to a valid time-zone entry.
//        /// </summary>
//        /// <param name="utcTime">Use UTC date time.</param>
//        /// <param name="zoneName">The standard zone name</param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx LocalTime(DateTime utcTime, string zoneName)
//        {
//            var tz = TzTimeZoneProvider.GetTimeZone(zoneName);
//            if (tz == null)
//                throw new ArgumentException("Invalid time-zone name.", "zoneName");
//            //tz = PublicDomain.TzTimeZone.GetTimeZone("GMT");

//            //UTC offset at the specified time
//            TimeSpan offset = tz.GetUtcOffset(utcTime);

//            return new DateTimeOffsetEx(
//                utcTime.Ticks + offset.Ticks,    //local time in the time-zone
//                offset,
//                tz.GetAbbreviation(utcTime),
//                tz.StandardName
//                );
//        }

//        /// <summary>
//        /// Creates local DateTimeOffsetEx based on the specified time in the time-zone.
//        /// Used to retrieve historic time.
//        /// In contrast to <see cref="LocalTime"/> this method falls back to "GMT" time-zone
//        /// for the "local" time-zone if the provided "zoneName" cannot be resolved to a valid
//        /// time-zone entry. (MOSO-17682)
//        /// </summary>
//        /// <param name="utcTime">Use UTC date time.</param>
//        /// <param name="zoneName">The standard zone name</param>
//        /// <returns></returns>
//        public static DateTimeOffsetEx TryLocalTime(DateTime utcTime, string zoneName)
//        {
//            var tz = TzTimeZoneProvider.GetTimeZone(zoneName);
//            if (tz == null)
//                tz = TzTimeZoneProvider.GetTimeZone("GMT");

//            //UTC offset at the specified time
//            TimeSpan offset = tz.GetUtcOffset(utcTime);

//            return new DateTimeOffsetEx(
//                utcTime.Ticks + offset.Ticks,    //local time in the time-zone
//                offset,
//                tz.GetAbbreviation(utcTime),
//                tz.StandardName
//                );
//        }

//        /// <summary>
//        /// Resolves the time-zone name for a given DateTimeOffsetEx instance based on
//        /// the following strategy:
//        ///  - resolve to <see cref="DateTimeOffsetEx.TimeZoneName"/> if property is defined
//        ///  - resolve time-zone based on <see cref="DateTimeOffsetEx.TimeFormat"/> if property is defined
//        ///  - resolve time-zone based on <see cref="DateTimeOffsetEx.Offset"/> if property is defined
//        ///  - resolve to <code>null</code>
//        /// </summary>
//        /// <param name="dt"></param>
//        /// <returns></returns>
//        public static string GetTimeZoneName(DateTimeOffsetEx dt)
//        {
//            if (dt.TimeZoneName != null)
//                return dt.TimeZoneName;
//            if (dt.TimeFormat != null)
//                return TimeZones.FirstOrDefault(tz => tz.Abbreviation == dt.TimeFormat)?.Name;
//            if (dt.Offset != null)
//                return TimeZones.FirstOrDefault(tz => Math.Abs(tz.UtcOffset - dt.Offset.Value.TotalHours) < 0.01)?.Name;
//            return null;
//        }

//        #endregion

//        #region Support
//        private void initialize(ITzTimeZone tz)
//        {
//            //assign member
//            _tz = tz;

//            //get name
//            string name = Name;

//            //break out name
//            if (name.Contains('/'))
//            {
//                //zone name
//                string[] splitData = name.Split('/');
//                _zone = splitData[0];

//                //split to region and location
//                if (splitData.Length == 2)
//                {
//                    _location = splitData[1];
//                }
//                else if (splitData.Length > 2)
//                {
//                    _region = splitData[1];
//                    _location = splitData[2];
//                }
//            }
//            else
//            {
//                _zone = name;
//            }
//        }
//        #endregion


//        //private static List<OlsonTimeZone> createSortedList(List<OlsonTimeZone> orginalList)
//        //{
//        //    List<OlsonTimeZone> returnList = new List<OlsonTimeZone>(orginalList.Count());

//        //    // add the items that are from the Zone "America" and doesn't have a state
//        //    returnList.AddRange((from FocusTimeZone in orginalList
//        //                         where FocusTimeZone.Zone == "America" &&
//        //                         FocusTimeZone.Region.IsNullOrEmpty()
//        //                         select FocusTimeZone).ToList()
//        //        );

//        //    //Next Add the items that are from the Zone "America" and has a state
//        //    returnList.AddRange((from FocusTimeZone in orginalList
//        //                         where FocusTimeZone.Zone == "America" &&
//        //                         !FocusTimeZone.Region.IsNullOrEmpty()
//        //                         select FocusTimeZone).ToList()
//        //        );
//        //    //Then add the rest
//        //    returnList.AddRange((from FocusTimeZone in orginalList
//        //                         where FocusTimeZone.Zone != "America" &&
//        //                         !FocusTimeZone.Location.IsNullOrEmpty() 
//        //                         select FocusTimeZone).ToList()
//        //        );
//        //    //Finally add the items that are timezones only
//        //    returnList.AddRange((from FocusTimeZone in orginalList
//        //                         where FocusTimeZone.Region.IsNullOrEmpty() &&
//        //                         FocusTimeZone.Location.IsNullOrEmpty()
//        //                         select FocusTimeZone).ToList()
//        //        );
//        //    return returnList;
//        //}

//        //public static string GetFormat(string zoneName, DateTimeOffset dateTime)
//        //{
//        //    TzTimeZone zone = TzTimeZone.GetTimeZone(zoneName);
//        //    if (zone == null)
//        //        zone = TzTimeZone.GetTimeZone("GMT");

//        //    if (zone.IsDaylightSavingTime(dateTime.DateTime))
//        //        return zone.FindZone(dateTime.DateTime).Format.Replace("%s", "D");
//        //    else
//        //        return zone.FindZone(dateTime.DateTime).Format.Replace("%s", "S");
//        //}

//        //public static double GetUTCOffSet(string zoneName, DateTime dateTime)
//        //{
//        //    TzTimeZone zone = TzTimeZone.GetTimeZone(zoneName);
//        //    if (zone == null)
//        //        zone = TzTimeZone.GetTimeZone("GMT");

//        //    return ToUTCOffsetValue(zone.GetUtcOffset(dateTime));
//        //}


//    }
//}
