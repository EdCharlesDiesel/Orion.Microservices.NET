using Skender.Stock.Indicators;

namespace Orion.WebApps.AnalysisDashboard.Models
{
    // IQuote gives Skender direct access — no conversion step ever needed
    public class Candle : IQuote
    {
        public DateTime Date { get; set; }   // IQuote uses Date, not DateTime
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }   // IQuote Volume is decimal
    }

    public class ForexCandle
    {
        public DateTime Timestamp { get; set; }
        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }

        // Convert from Candle (IQuote) to ForexCandle
        public static ForexCandle FromCandle(Candle candle)
        {
            return new ForexCandle
            {
                Timestamp = candle.Date,
                Date = candle.Date.ToString("yyyy-MM-dd"),
                Time = candle.Date.ToString("HH:mm:ss"),
                Open = candle.Open,
                High = candle.High,
                Low = candle.Low,
                Close = candle.Close,
                Volume = (int)candle.Volume
            };
        }

        // Convert to Candle (IQuote) for Skender
        public Candle ToCandle()
        {
            return new Candle
            {
                Date = Timestamp,
                Open = Open,
                High = High,
                Low = Low,
                Close = Close,
                Volume = Volume
            };
        }
    }

    public class ForexStatistics
    {
        public int TotalCandles { get; set; }
        public string DateRange { get; set; } = string.Empty;
        public decimal FirstPrice { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercent { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
    }

    public class ChartDataPoint
    {
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }
    }

    public class UploadResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ForexStatistics? Statistics { get; set; }
        public int TotalRows { get; set; }
    }

    public class IndicatorChartData
    {
        public List<DateTime> Dates { get; set; } = new();
        public Dictionary<string, List<double?>> Values { get; set; } = new();
    }
}