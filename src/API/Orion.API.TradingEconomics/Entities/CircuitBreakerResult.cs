namespace Orion.API.TradingEconomics.Entities
{
    public sealed class CircuitBreakerResult
    {
        public bool IsTripped { get; set; }
        public string Reason { get; set; } = "";

        public static CircuitBreakerResult Clear(string reason)
        {
            return new CircuitBreakerResult
            {
                IsTripped = false,
                Reason = reason
            };
        }

        public static CircuitBreakerResult Tripped(string reason)
        {
            return new CircuitBreakerResult
            {
                IsTripped = true,
                Reason = reason
            };
        }
    }
}
