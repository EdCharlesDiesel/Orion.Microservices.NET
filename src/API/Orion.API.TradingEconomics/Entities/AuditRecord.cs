using Orion.API.TradingEconomics.Engine;

namespace Orion.API.TradingEconomics.Entities;


public class AuditRecord
{
    public string Stage { get; set; } = "";
    public string Status { get; set; } = "";
    public string Pair { get; set; } = "";
    public string Reason { get; set; } = "";
    public string PayloadJson { get; set; } = "";
    public DateTime TimestampUtc { get; set; }
    public Guid CorrelationId { get; set; }
    public string SessionId { get; set; }
    public string TraderId { get; set; }
    public DateTime ExecutionTime { get; set; }
        
    // Pipeline Input
    public ForexMarketInput Input { get; set; }
        
    // Pipeline Results
    public NormalizedMarketContext NormalizedContext { get; set; }
    public RegimeResult Regime { get; set; }
    public ScenarioResult Scenario { get; set; }
    public ProbabilisticResult Probabilities { get; set; }
    public MacroSimulationResult MacroSimulation { get; set; }
    public SignalResult Signal { get; set; }
    public RiskEvaluation Risk { get; set; }
    public decimal PositionSize { get; set; }
    public ExecutionResult Execution { get; set; }
    public ExitStrategy Exit { get; set; }
        
    // Final Decision
    public TradingDecision Decision { get; set; }
        
    // Metadata
    public TimeSpan TotalProcessingTime { get; set; }
    public Dictionary<string, TimeSpan> StepTimings { get; set; }
    public string Version { get; set; }
    public Dictionary<string, object> Tags { get; set; }
}