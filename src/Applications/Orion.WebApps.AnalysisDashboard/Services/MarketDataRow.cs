
//// Updated MarketDataRow to include indicators dictionary
//public class MarketDataRow
//{
//    public DateTime DateTime { get; set; }
//    public decimal Open { get; set; }
//    public decimal High { get; set; }
//    public decimal Low { get; set; }
//    public decimal Close { get; set; }
//    public long Volume { get; set; }
//    public Dictionary<string, decimal> Indicators { get; set; } = new();

//    public MarketDataRow Clone()
//    {
//        return new MarketDataRow
//        {
//            DateTime = this.DateTime,
//            Open = this.Open,
//            High = this.High,
//            Low = this.Low,
//            Close = this.Close,
//            Volume = this.Volume,
//            Indicators = new Dictionary<string, decimal>(this.Indicators)
//        };
//    }
//}
