//namespace Orion.WebApps.AanalysisDashboard.Steps
//{    /// <summary>
//    /// Represents the category or source of a trading signal
//    /// Used to classify and weight different types of market analysis
//    /// </summary>
//    public enum SignalCategory
//    {
//        /// <summary>
//        /// Technical analysis signals (indicators, patterns, price action)
//        /// </summary>
//        Technical = 1,

//        /// <summary>
//        /// Fundamental analysis signals (company financials, valuations)
//        /// </summary>
//        Fundamental = 2,

//        /// <summary>
//        /// Macroeconomic signals (GDP, interest rates, inflation)
//        /// </summary>
//        Macro = 3,

//        /// <summary>
//        /// Market sentiment signals (fear/greed, positioning, surveys)
//        /// </summary>
//        Sentiment = 4,

//        /// <summary>
//        /// Intermarket analysis signals (correlations, cross-asset relationships)
//        /// </summary>
//        Intermarket = 5,

//        /// <summary>
//        /// Chart pattern recognition signals
//        /// </summary>
//        Pattern = 6,

//        /// <summary>
//        /// Volume analysis signals
//        /// </summary>
//        Volume = 7,

//        /// <summary>
//        /// Volatility-based signals
//        /// </summary>
//        Volatility = 8,

//        /// <summary>
//        /// Order flow and market microstructure signals
//        /// </summary>
//        OrderFlow = 9,

//        /// <summary>
//        /// Seasonality and calendar-based signals
//        /// </summary>
//        Seasonality = 10,

//        /// <summary>
//        /// News and event-driven signals
//        /// </summary>
//        News = 11,

//        /// <summary>
//        /// Quantitative and algorithmic signals
//        /// </summary>
//        Quantitative = 12
//    }

//    /// <summary>
//    /// Extension methods for SignalCategory
//    /// </summary>
//    public static class SignalCategoryExtensions
//    {
//        /// <summary>
//        /// Returns a human-readable display string
//        /// </summary>
//        public static string ToDisplayString(this SignalCategory category)
//        {
//            return category switch
//            {
//                SignalCategory.Technical => "Technical",
//                SignalCategory.Fundamental => "Fundamental",
//                SignalCategory.Macro => "Macroeconomic",
//                SignalCategory.Sentiment => "Sentiment",
//                SignalCategory.Intermarket => "Intermarket",
//                SignalCategory.Pattern => "Pattern",
//                SignalCategory.Volume => "Volume",
//                SignalCategory.Volatility => "Volatility",
//                SignalCategory.OrderFlow => "Order Flow",
//                SignalCategory.Seasonality => "Seasonality",
//                SignalCategory.News => "News",
//                SignalCategory.Quantitative => "Quantitative",
//                _ => "Unknown"
//            };
//        }

//        /// <summary>
//        /// Returns an emoji representation for the category
//        /// </summary>
//        public static string ToEmoji(this SignalCategory category)
//        {
//            return category switch
//            {
//                SignalCategory.Technical => "📊",
//                SignalCategory.Fundamental => "📈",
//                SignalCategory.Macro => "🌍",
//                SignalCategory.Sentiment => "😊",
//                SignalCategory.Intermarket => "🔗",
//                SignalCategory.Pattern => "📐",
//                SignalCategory.Volume => "📊",
//                SignalCategory.Volatility => "📉",
//                SignalCategory.OrderFlow => "💹",
//                SignalCategory.Seasonality => "📅",
//                SignalCategory.News => "📰",
//                SignalCategory.Quantitative => "🤖",
//                _ => "📌"
//            };
//        }

//        /// <summary>
//        /// Returns the default weight multiplier for this category in signal calculations
//        /// </summary>
//        public static decimal GetDefaultWeight(this SignalCategory category)
//        {
//            return category switch
//            {
//                SignalCategory.Technical => 1.0m,
//                SignalCategory.Fundamental => 1.2m,
//                SignalCategory.Macro => 1.5m,
//                SignalCategory.Sentiment => 0.8m,
//                SignalCategory.Intermarket => 1.1m,
//                SignalCategory.Pattern => 1.0m,
//                SignalCategory.Volume => 0.9m,
//                SignalCategory.Volatility => 0.8m,
//                SignalCategory.OrderFlow => 1.1m,
//                SignalCategory.Seasonality => 0.6m,
//                SignalCategory.News => 0.7m,
//                SignalCategory.Quantitative => 1.3m,
//                _ => 1.0m
//            };
//        }

//        /// <summary>
//        /// Returns the typical reliability score for this category (0-100)
//        /// </summary>
//        public static int GetReliabilityScore(this SignalCategory category)
//        {
//            return category switch
//            {
//                SignalCategory.Technical => 70,
//                SignalCategory.Fundamental => 75,
//                SignalCategory.Macro => 80,
//                SignalCategory.Sentiment => 50,
//                SignalCategory.Intermarket => 65,
//                SignalCategory.Pattern => 60,
//                SignalCategory.Volume => 65,
//                SignalCategory.Volatility => 55,
//                SignalCategory.OrderFlow => 75,
//                SignalCategory.Seasonality => 45,
//                SignalCategory.News => 35,
//                SignalCategory.Quantitative => 85,
//                _ => 50
//            };
//        }

//        /// <summary>
//        /// Returns the typical time horizon for signals of this category
//        /// </summary>
//        public static TimeSpan GetTypicalTimeHorizon(this SignalCategory category)
//        {
//            return category switch
//            {
//                SignalCategory.Technical => TimeSpan.FromDays(5),
//                SignalCategory.Fundamental => TimeSpan.FromDays(90),
//                SignalCategory.Macro => TimeSpan.FromDays(180),
//                SignalCategory.Sentiment => TimeSpan.FromDays(10),
//                SignalCategory.Intermarket => TimeSpan.FromDays(30),
//                SignalCategory.Pattern => TimeSpan.FromDays(7),
//                SignalCategory.Volume => TimeSpan.FromDays(3),
//                SignalCategory.Volatility => TimeSpan.FromDays(5),
//                SignalCategory.OrderFlow => TimeSpan.FromHours(4),
//                SignalCategory.Seasonality => TimeSpan.FromDays(30),
//                SignalCategory.News => TimeSpan.FromHours(24),
//                SignalCategory.Quantitative => TimeSpan.FromDays(14),
//                _ => TimeSpan.FromDays(7)
//            };
//        }

//        /// <summary>
//        /// Returns true if this category is considered leading (predictive)
//        /// </summary>
//        public static bool IsLeading(this SignalCategory category)
//        {
//            return category switch
//            {
//                SignalCategory.Technical => true,
//                SignalCategory.OrderFlow => true,
//                SignalCategory.Quantitative => true,
//                SignalCategory.Sentiment => true,
//                _ => false
//            };
//        }

//        /// <summary>
//        /// Returns true if this category is considered lagging (confirming)
//        /// </summary>
//        public static bool IsLagging(this SignalCategory category)
//        {
//            return !category.IsLeading();
//        }

//        /// <summary>
//        /// Returns a CSS class for styling this category
//        /// </summary>
//        public static string ToCssClass(this SignalCategory category)
//        {
//            return category switch
//            {
//                SignalCategory.Technical => "signal-technical",
//                SignalCategory.Fundamental => "signal-fundamental",
//                SignalCategory.Macro => "signal-macro",
//                SignalCategory.Sentiment => "signal-sentiment",
//                SignalCategory.Intermarket => "signal-intermarket",
//                SignalCategory.Pattern => "signal-pattern",
//                SignalCategory.Volume => "signal-volume",
//                SignalCategory.Volatility => "signal-volatility",
//                SignalCategory.OrderFlow => "signal-orderflow",
//                SignalCategory.Seasonality => "signal-seasonality",
//                SignalCategory.News => "signal-news",
//                SignalCategory.Quantitative => "signal-quantitative",
//                _ => "signal-default"
//            };
//        }

//        /// <summary>
//        /// Returns a Bootstrap badge class for this category
//        /// </summary>
//        public static string ToBootstrapBadgeClass(this SignalCategory category)
//        {
//            return category switch
//            {
//                SignalCategory.Technical => "bg-primary",
//                SignalCategory.Fundamental => "bg-success",
//                SignalCategory.Macro => "bg-info",
//                SignalCategory.Sentiment => "bg-warning",
//                SignalCategory.Intermarket => "bg-secondary",
//                SignalCategory.Pattern => "bg-light text-dark",
//                SignalCategory.Volume => "bg-dark",
//                SignalCategory.Volatility => "bg-danger",
//                SignalCategory.OrderFlow => "bg-info",
//                SignalCategory.Seasonality => "bg-secondary",
//                SignalCategory.News => "bg-warning",
//                SignalCategory.Quantitative => "bg-primary",
//                _ => "bg-secondary"
//            };
//        }

//        /// <summary>
//        /// Returns the correlation with other categories
//        /// </summary>
//        public static decimal GetCorrelationWith(this SignalCategory category, SignalCategory other)
//        {
//            if (category == other) return 1.0m;

//            return (category, other) switch
//            {
//                (SignalCategory.Technical, SignalCategory.Volume) => 0.7m,
//                (SignalCategory.Technical, SignalCategory.Pattern) => 0.8m,
//                (SignalCategory.Fundamental, SignalCategory.Macro) => 0.9m,
//                (SignalCategory.Macro, SignalCategory.Intermarket) => 0.7m,
//                (SignalCategory.Sentiment, SignalCategory.Technical) => 0.4m,
//                (SignalCategory.Quantitative, SignalCategory.Technical) => 0.6m,
//                _ => 0.3m
//            };
//        }

//        /// <summary>
//        /// Parses a string to SignalCategory
//        /// </summary>
//        public static SignalCategory Parse(string value)
//        {
//            if (string.IsNullOrWhiteSpace(value))
//                return SignalCategory.Technical;

//            var normalized = value.ToLowerInvariant().Trim();

//            return normalized switch
//            {
//                "technical" or "tech" => SignalCategory.Technical,
//                "fundamental" or "fund" => SignalCategory.Fundamental,
//                "macro" or "macroeconomic" or "econ" => SignalCategory.Macro,
//                "sentiment" or "sent" => SignalCategory.Sentiment,
//                "intermarket" or "inter" => SignalCategory.Intermarket,
//                "pattern" or "pat" => SignalCategory.Pattern,
//                "volume" or "vol" => SignalCategory.Volume,
//                "volatility" or "vix" => SignalCategory.Volatility,
//                "orderflow" or "order flow" or "flow" => SignalCategory.OrderFlow,
//                "seasonality" or "season" => SignalCategory.Seasonality,
//                "news" => SignalCategory.News,
//                "quantitative" or "quant" or "algo" => SignalCategory.Quantitative,
//                _ => Enum.TryParse<SignalCategory>(value, true, out var result) ? result : SignalCategory.Technical
//            };
//        }

//        /// <summary>
//        /// Gets categories suitable for short-term trading
//        /// </summary>
//        public static SignalCategory[] GetShortTermCategories()
//        {
//            return new[]
//            {
//                SignalCategory.Technical,
//                SignalCategory.OrderFlow,
//                SignalCategory.Volume,
//                SignalCategory.Volatility
//            };
//        }

//        /// <summary>
//        /// Gets categories suitable for long-term investing
//        /// </summary>
//        public static SignalCategory[] GetLongTermCategories()
//        {
//            return new[]
//            {
//                SignalCategory.Fundamental,
//                SignalCategory.Macro,
//                SignalCategory.Intermarket,
//                SignalCategory.Seasonality
//            };
//        }

//        /// <summary>
//        /// Gets all categories ordered by typical reliability
//        /// </summary>
//        public static SignalCategory[] GetOrderedByReliability()
//        {
//            return Enum.GetValues<SignalCategory>()
//                .OrderByDescending(c => c.GetReliabilityScore())
//                .ToArray();
//        }

//        /// <summary>
//        /// Combines multiple categories to suggest a primary category
//        /// </summary>
//        public static SignalCategory GetDominantCategory(IEnumerable<SignalCategory> categories)
//        {
//            if (categories == null || !categories.Any())
//                return SignalCategory.Technical;

//            return categories
//                .GroupBy(c => c)
//                .OrderByDescending(g => g.Sum(c => c.GetDefaultWeight()))
//                .ThenByDescending(g => g.Count())
//                .First()
//                .Key;
//        }

//        /// <summary>
//        /// Returns a description of what this category analyzes
//        /// </summary>
//        public static string GetDescription(this SignalCategory category)
//        {
//            return category switch
//            {
//                SignalCategory.Technical => "Price action, indicators, and chart patterns",
//                SignalCategory.Fundamental => "Company financials, earnings, and valuations",
//                SignalCategory.Macro => "Economic data, central bank policy, and geopolitical events",
//                SignalCategory.Sentiment => "Market psychology, positioning, and surveys",
//                SignalCategory.Intermarket => "Cross-asset relationships and correlations",
//                SignalCategory.Pattern => "Chart patterns and formations",
//                SignalCategory.Volume => "Trading volume analysis and anomalies",
//                SignalCategory.Volatility => "Market volatility and risk metrics",
//                SignalCategory.OrderFlow => "Market microstructure and order book dynamics",
//                SignalCategory.Seasonality => "Calendar effects and seasonal patterns",
//                SignalCategory.News => "Breaking news and event-driven catalysts",
//                SignalCategory.Quantitative => "Statistical models and algorithmic signals",
//                _ => "Unknown category"
//            };
//        }
//    }
//}
