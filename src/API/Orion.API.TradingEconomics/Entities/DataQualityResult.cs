namespace Orion.API.TradingEconomics.Entities
{
    public sealed class DataQualityResult
    {
        public bool IsValid { get; set; }
        public string Reason { get; set; } = "";
        public IEnumerable<string?> Issues { get; }
        public int Score { get; set; }
        public bool CanRetry { get; set; }

        public static DataQualityResult Pass(string reason)
        {
            return new DataQualityResult
            {
                IsValid = true,
                Reason = reason
            };
        }

        public static DataQualityResult Fail(string reason)
        {
            return new DataQualityResult
            {
                IsValid = false,
                Reason = reason
            };
        }

        public bool Message(char obj)
        {
            throw new NotImplementedException();
        }
    }
}
