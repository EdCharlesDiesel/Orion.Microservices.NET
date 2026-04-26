namespace Orion.API.TradingEconomics.Enum;

public enum AuditRecordType
{
    Decision,
    PipelineStep,
    Error,
    Event,
    StateChange,
    Compliance
}

public enum ComplianceDecision
{
    Approved = 0,
    Rejected = 1,
    ManualReview = 2
}

public enum HealthComponentType
{
    DataProvider,
    PipelineEngine,
    ExternalService,
    Infrastructure,
    Database,
    Cache,
    MessageQueue,
    Custom
}

public enum RiskAction
{
    AllowTrade = 0,
    BlockTrade = 1,
    ReducePosition = 2,
    ClosePosition = 3,
    EmergencyFlatten = 4
}

public enum MarketRegime
{
    RiskOn,
    RiskOff,
    Stagflation,
    Goldilocks
}

public enum IndicatorFrequency
{
    Daily,
    Weekly,
    Monthly,
    Quarterly,
    Yearly
}