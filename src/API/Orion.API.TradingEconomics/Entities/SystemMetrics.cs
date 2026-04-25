namespace Orion.API.TradingEconomics.Entities;

public class SystemMetrics
{
    public DateTime ProcessStartTime { get; set; }
    public TimeSpan UpTime { get; set; }
    public int ThreadCount { get; set; }
    public int HandleCount { get; set; }
    public long WorkingSet { get; set; }
    public long PeakWorkingSet { get; set; }
    public long PrivateMemory { get; set; }
    public long VirtualMemory { get; set; }
    public long PagedMemory { get; set; }
    public long GcTotalMemory { get; set; }
    public long PipelineDecisionsProcessed { get; set; }
    public double ErrorRate { get; set; }
}