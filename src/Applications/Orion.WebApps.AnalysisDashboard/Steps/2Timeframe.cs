//namespace Orion.WebApps.AanalysisDashboard.Steps
//{
//    /// <summary>
//    /// Represents trading timeframes with their duration in minutes
//    /// </summary>
//    public enum Timeframe
//    {
//        /// <summary>
//        /// 1 minute timeframe
//        /// </summary>
//        Minute1 = 1,

//        /// <summary>
//        /// 5 minute timeframe
//        /// </summary>
//        Minute5 = 5,

//        /// <summary>
//        /// 15 minute timeframe
//        /// </summary>
//        Minute15 = 15,

//        /// <summary>
//        /// 30 minute timeframe
//        /// </summary>
//        Minute30 = 30,

//        /// <summary>
//        /// 1 hour timeframe
//        /// </summary>
//        Hourly = 60,

//        /// <summary>
//        /// 4 hour timeframe
//        /// </summary>
//        FourHour = 240,

//        /// <summary>
//        /// Daily timeframe
//        /// </summary>
//        Daily = 1440,

//        /// <summary>
//        /// Weekly timeframe
//        /// </summary>
//        Weekly = 10080,

//        /// <summary>
//        /// Monthly timeframe (approximated as 30 days)
//        /// </summary>
//        Monthly = 43200
//    }

//    /// <summary>
//    /// Extension methods for Timeframe
//    /// </summary>
//    public static class TimeframeExtensions
//    {
//        /// <summary>
//        /// Returns the duration of the timeframe
//        /// </summary>
//        public static TimeSpan ToTimeSpan(this Timeframe timeframe)
//        {
//            return TimeSpan.FromMinutes((int)timeframe);
//        }

//        /// <summary>
//        /// Returns a human-readable display string for the timeframe
//        /// </summary>
//        public static string ToDisplayString(this Timeframe timeframe)
//        {
//            return timeframe switch
//            {
//                Timeframe.Minute1 => "1m",
//                Timeframe.Minute5 => "5m",
//                Timeframe.Minute15 => "15m",
//                Timeframe.Minute30 => "30m",
//                Timeframe.Hourly => "1h",
//                Timeframe.FourHour => "4h",
//                Timeframe.Daily => "D",
//                Timeframe.Weekly => "W",
//                Timeframe.Monthly => "M",
//                _ => timeframe.ToString()
//            };
//        }

//        /// <summary>
//        /// Returns a full descriptive name for the timeframe
//        /// </summary>
//        public static string ToFullName(this Timeframe timeframe)
//        {
//            return timeframe switch
//            {
//                Timeframe.Minute1 => "1 Minute",
//                Timeframe.Minute5 => "5 Minutes",
//                Timeframe.Minute15 => "15 Minutes",
//                Timeframe.Minute30 => "30 Minutes",
//                Timeframe.Hourly => "1 Hour",
//                Timeframe.FourHour => "4 Hours",
//                Timeframe.Daily => "Daily",
//                Timeframe.Weekly => "Weekly",
//                Timeframe.Monthly => "Monthly",
//                _ => timeframe.ToString()
//            };
//        }

//        /// <summary>
//        /// Returns true if this is a higher timeframe than the specified one
//        /// </summary>
//        public static bool IsHigherThan(this Timeframe current, Timeframe other)
//        {
//            return (int)current > (int)other;
//        }

//        /// <summary>
//        /// Returns true if this is a lower timeframe than the specified one
//        /// </summary>
//        public static bool IsLowerThan(this Timeframe current, Timeframe other)
//        {
//            return (int)current < (int)other;
//        }

//        /// <summary>
//        /// Returns the next higher timeframe
//        /// </summary>
//        public static Timeframe GetNextHigher(this Timeframe timeframe)
//        {
//            return timeframe switch
//            {
//                Timeframe.Minute1 => Timeframe.Minute5,
//                Timeframe.Minute5 => Timeframe.Minute15,
//                Timeframe.Minute15 => Timeframe.Minute30,
//                Timeframe.Minute30 => Timeframe.Hourly,
//                Timeframe.Hourly => Timeframe.FourHour,
//                Timeframe.FourHour => Timeframe.Daily,
//                Timeframe.Daily => Timeframe.Weekly,
//                Timeframe.Weekly => Timeframe.Monthly,
//                Timeframe.Monthly => Timeframe.Monthly,
//                _ => timeframe
//            };
//        }

//        /// <summary>
//        /// Returns the next lower timeframe
//        /// </summary>
//        public static Timeframe GetNextLower(this Timeframe timeframe)
//        {
//            return timeframe switch
//            {
//                Timeframe.Minute1 => Timeframe.Minute1,
//                Timeframe.Minute5 => Timeframe.Minute1,
//                Timeframe.Minute15 => Timeframe.Minute5,
//                Timeframe.Minute30 => Timeframe.Minute15,
//                Timeframe.Hourly => Timeframe.Minute30,
//                Timeframe.FourHour => Timeframe.Hourly,
//                Timeframe.Daily => Timeframe.FourHour,
//                Timeframe.Weekly => Timeframe.Daily,
//                Timeframe.Monthly => Timeframe.Weekly,
//                _ => timeframe
//            };
//        }

//        /// <summary>
//        /// Returns the weight of the timeframe for multi-timeframe analysis
//        /// Higher timeframes have more weight
//        /// </summary>
//        public static int GetAnalysisWeight(this Timeframe timeframe)
//        {
//            return timeframe switch
//            {
//                Timeframe.Minute1 => 1,
//                Timeframe.Minute5 => 1,
//                Timeframe.Minute15 => 2,
//                Timeframe.Minute30 => 2,
//                Timeframe.Hourly => 3,
//                Timeframe.FourHour => 4,
//                Timeframe.Daily => 5,
//                Timeframe.Weekly => 6,
//                Timeframe.Monthly => 7,
//                _ => 1
//            };
//        }

//        /// <summary>
//        /// Returns true if this is an intraday timeframe
//        /// </summary>
//        public static bool IsIntraday(this Timeframe timeframe)
//        {
//            return timeframe < Timeframe.Daily;
//        }

//        /// <summary>
//        /// Returns the typical number of candles needed for a valid analysis
//        /// </summary>
//        public static int GetMinimumCandlesForAnalysis(this Timeframe timeframe)
//        {
//            return timeframe switch
//            {
//                Timeframe.Minute1 => 100,
//                Timeframe.Minute5 => 100,
//                Timeframe.Minute15 => 80,
//                Timeframe.Minute30 => 70,
//                Timeframe.Hourly => 60,
//                Timeframe.FourHour => 50,
//                Timeframe.Daily => 30,
//                Timeframe.Weekly => 20,
//                Timeframe.Monthly => 12,
//                _ => 50
//            };
//        }

//        /// <summary>
//        /// Returns the typical lookback period in days for this timeframe
//        /// </summary>
//        public static int GetTypicalLookbackDays(this Timeframe timeframe)
//        {
//            return timeframe switch
//            {
//                Timeframe.Minute1 => 1,
//                Timeframe.Minute5 => 2,
//                Timeframe.Minute15 => 3,
//                Timeframe.Minute30 => 5,
//                Timeframe.Hourly => 7,
//                Timeframe.FourHour => 14,
//                Timeframe.Daily => 30,
//                Timeframe.Weekly => 90,
//                Timeframe.Monthly => 365,
//                _ => 30
//            };
//        }

//        /// <summary>
//        /// Parses a string to Timeframe
//        /// </summary>
//        public static Timeframe Parse(string value)
//        {
//            if (string.IsNullOrWhiteSpace(value))
//                return Timeframe.Daily;

//            var normalized = value.ToLowerInvariant().Replace(" ", "");

//            return normalized switch
//            {
//                "1m" or "m1" or "1min" => Timeframe.Minute1,
//                "5m" or "m5" or "5min" => Timeframe.Minute5,
//                "15m" or "m15" or "15min" => Timeframe.Minute15,
//                "30m" or "m30" or "30min" => Timeframe.Minute30,
//                "1h" or "h1" or "60" or "hourly" => Timeframe.Hourly,
//                "4h" or "h4" or "240" or "4hour" => Timeframe.FourHour,
//                "1d" or "d" or "daily" or "1440" => Timeframe.Daily,
//                "1w" or "w" or "weekly" => Timeframe.Weekly,
//                "1m" or "m" or "monthly" => Timeframe.Monthly,
//                _ => Enum.TryParse<Timeframe>(value, true, out var result) ? result : Timeframe.Daily
//            };
//        }

//        /// <summary>
//        /// Converts timeframe to Yahoo Finance interval string
//        /// </summary>
//        public static string ToYahooFinanceInterval(this Timeframe timeframe)
//        {
//            return timeframe switch
//            {
//                Timeframe.Minute1 => "1m",
//                Timeframe.Minute5 => "5m",
//                Timeframe.Minute15 => "15m",
//                Timeframe.Minute30 => "30m",
//                Timeframe.Hourly => "1h",
//                Timeframe.FourHour => "1h", // Yahoo doesn't have 4h, needs resampling
//                Timeframe.Daily => "1d",
//                Timeframe.Weekly => "1wk",
//                Timeframe.Monthly => "1mo",
//                _ => "1d"
//            };
//        }

//        /// <summary>
//        /// Converts timeframe to TradingView interval string
//        /// </summary>
//        public static string ToTradingViewInterval(this Timeframe timeframe)
//        {
//            return timeframe switch
//            {
//                Timeframe.Minute1 => "1",
//                Timeframe.Minute5 => "5",
//                Timeframe.Minute15 => "15",
//                Timeframe.Minute30 => "30",
//                Timeframe.Hourly => "60",
//                Timeframe.FourHour => "240",
//                Timeframe.Daily => "D",
//                Timeframe.Weekly => "W",
//                Timeframe.Monthly => "M",
//                _ => "D"
//            };
//        }

//        /// <summary>
//        /// Gets all standard timeframes for analysis
//        /// </summary>
//        public static Timeframe[] GetStandardTimeframes()
//        {
//            return new[]
//            {
//                Timeframe.Hourly,
//                Timeframe.FourHour,
//                Timeframe.Daily
//            };
//        }

//        /// <summary>
//        /// Gets all timeframes ordered from lowest to highest
//        /// </summary>
//        public static Timeframe[] GetAllOrdered()
//        {
//            return new[]
//            {
//                Timeframe.Minute1,
//                Timeframe.Minute5,
//                Timeframe.Minute15,
//                Timeframe.Minute30,
//                Timeframe.Hourly,
//                Timeframe.FourHour,
//                Timeframe.Daily,
//                Timeframe.Weekly,
//                Timeframe.Monthly
//            };
//        }
//    }
//}
