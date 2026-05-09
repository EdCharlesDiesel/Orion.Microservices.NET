namespace Orion.API.TradingEconomics.Entities
{
    public class TradingEconomicsResponse<T>
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public List<T> Data { get; set; } = new();
    }
}
