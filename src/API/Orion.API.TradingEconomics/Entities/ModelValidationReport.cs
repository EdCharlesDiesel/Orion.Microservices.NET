namespace Orion.API.TradingEconomics.Entities;

public sealed class ModelValidationReport
{
    public bool IsValid { get; set; }
    public decimal Score { get; set; }
    public string Verdict { get; set; } = "";
    public string Reason { get; set; } = "";

    public static ModelValidationReport Fail(string reason)
    {
        return new ModelValidationReport
        {
            IsValid = false,
            Score = 0,
            Verdict = "FAILED_VALIDATION",
            Reason = reason
        };
    }
}