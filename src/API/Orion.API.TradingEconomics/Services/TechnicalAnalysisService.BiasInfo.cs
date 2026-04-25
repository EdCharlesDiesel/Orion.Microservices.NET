namespace Orion.API.TradingEconomics.Services
{
 
        public class BiasInfo
        {
            public string Bias { get; set; } = "Neutral";
            public int Strength { get; set; }
            public decimal Confidence { get; set; }
            public List<string> Reasons { get; set; } = new();
            public string Reason { get; internal set; }
        }        
    
}
