namespace Orion.API.TradingEconomics.Entities;

public class PipelineStepAudit
{
    public string StepName { get; set; }
    public object Data { get; set; }
    public string DataType { get; set; }
    public TimeSpan Duration { get; set; }
}