// Program.cs for Dashboard API

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Orion.API.Dashboard;
using Orion.API.Dashboard.Models;
using Orion.DataAccess.Postgres.Services;
using ServiceCollectionExtensions = Orion.DataAccess.Postgres.Services.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

// JWT Settings
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<DashboardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication
builder.Services.AddJwtAuthentication(jwtSettings);

// Custom services
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Redis Cache
// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration.GetConnectionString("Redis");
// });

ServiceCollectionExtensions.AddCustomHealthChecks(builder.Services, builder.Configuration.GetConnectionString("Postgres"));

// Health Checks
ServiceCollectionExtensions.AddCustomHealthChecks(builder.Services, builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

// Models
public class DashboardItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Priority { get; set; }
    public string Status { get; set; } = "Active";
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public class CreateDashboardItemRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Priority { get; set; } = 1;
}

public class UpdateDashboardItemRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Priority { get; set; }
    public string Status { get; set; } = string.Empty;
}

// DbContext
public class DashboardDbContext : DbContext
{
    public DashboardDbContext(DbContextOptions<DashboardDbContext> options) : base(options) { }

    public DbSet<DashboardItem> DashboardItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DashboardItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(450);
            
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedBy);
        });
    }
}

// Services
public interface IDashboardService
{
    Task<DashboardStats> GetDashboardStatsAsync(string userId);
    Task<IEnumerable<DashboardItem>> GetDashboardItemsAsync(string userId, string? category = null, int page = 1, int pageSize = 10);
    Task<DashboardItem?> GetDashboardItemAsync(int id, string userId);
    Task<DashboardItem> CreateDashboardItemAsync(CreateDashboardItemRequest request, string userId);
    Task<DashboardItem?> UpdateDashboardItemAsync(int id, UpdateDashboardItemRequest request, string userId);
    Task<bool> DeleteDashboardItemAsync(int id, string userId);
}

public class DashboardService : IDashboardService
{
    private readonly DashboardDbContext _context;
    private readonly IDistributedCache _cache;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(DashboardDbContext context, IDistributedCache cache, ILogger<DashboardService> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<DashboardStats> GetDashboardStatsAsync(string userId)
    {
        var cacheKey = $"dashboard_stats_{userId}";
        var cachedStats = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedStats))
        {
            return System.Text.Json.JsonSerializer.Deserialize<DashboardStats>(cachedStats)!;
        }

        var items = await _context.DashboardItems
            .Where(d => d.CreatedBy == userId)
            .ToListAsync();

        var stats = new DashboardStats
        {
            TotalItems = items.Count,
            ActiveItems = items.Count(i => i.Status == "Active"),
            CompletedItems = items.Count(i => i.Status == "Completed"),
            PendingItems = items.Count(i => i.Status == "Pending"),
            ItemsByCategory = items.GroupBy(i => i.Category)
                                  .ToDictionary(g => g.Key, g => g.Count()),
            RecentItems = items.OrderByDescending(i => i.CreatedAt)
                             .Take(5)
                             .ToList()
        };

        // Cache for 5 minutes
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        await _cache.SetStringAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(stats), cacheOptions);

        return stats;
    }

    public async Task<IEnumerable<DashboardItem>> GetDashboardItemsAsync(string userId, string? category = null, int page = 1, int pageSize = 10)
    {
        var query = _context.DashboardItems.Where(d => d.CreatedBy == userId);

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(d => d.Category == category);
        }

        return await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<DashboardItem?> GetDashboardItemAsync(int id, string userId)
    {
        return await _context.DashboardItems
            .FirstOrDefaultAsync(d => d.Id == id && d.CreatedBy == userId);
    }

    public async Task<DashboardItem> CreateDashboardItemAsync(CreateDashboardItemRequest request, string userId)
    {
        var item = new DashboardItem
        {
            Title = request.Title,
            Description = request.Description,
            Category = request.Category,
            Priority = request.Priority,
            CreatedBy = userId
        };

        _context.DashboardItems.Add(item);
        await _context.SaveChangesAsync();

        // Invalidate cache
        await _cache.RemoveAsync($"dashboard_stats_{userId}");

        _logger.LogInformation("Dashboard item created: {ItemId} by user {UserId}", item.Id, userId);
        return item;
    }

    public async Task<DashboardItem?> UpdateDashboardItemAsync(int id, UpdateDashboardItemRequest request, string userId)
    {
        var item = await _context.DashboardItems
            .FirstOrDefaultAsync(d => d.Id == id && d.CreatedBy == userId);

        if (item == null)
            return null;

        item.Title = request.Title;
        item.Description = request.Description;
        item.Category = request.Category;
        item.Priority = request.Priority;
        item.Status = request.Status;
        item.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Invalidate cache
        await _cache.RemoveAsync($"dashboard_stats_{userId}");

        _logger.LogInformation("Dashboard item updated: {ItemId} by user {UserId}", item.Id, userId);
        return item;
    }

    public async Task<bool> DeleteDashboardItemAsync(int id, string userId)
    {
        var item = await _context.DashboardItems
            .FirstOrDefaultAsync(d => d.Id == id && d.CreatedBy == userId);

        if (item == null)
            return false;

        _context.DashboardItems.Remove(item);
        await _context.SaveChangesAsync();

        // Invalidate cache
        await _cache.RemoveAsync($"dashboard_stats_{userId}");

        _logger.LogInformation("Dashboard item deleted: {ItemId} by user {UserId}", item.Id, userId);
        return true;
    }
}

// Controllers
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{


}