namespace Orion.API.Dashboard.Models;

public class DashboardStats
{
    public int TotalItems { get; set; }
    public int ActiveItems { get; set; }
    public int CompletedItems { get; set; }
    public int PendingItems { get; set; }
    public Dictionary<string, int> ItemsByCategory { get; set; } = new();
    public List<DashboardItem> RecentItems { get; set; } = new();
}