using Microsoft.Extensions.Caching.Distributed;

namespace Orion.API.Dashboard.Services;

public class DashBoardService
{
    private readonly IDistributedCache _cache;

    public DashBoardService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task CacheDataAsync()
    {
        // Save string
        await _cache.SetStringAsync("greeting", "Hello from Redis!");

        // Retrieve string
        var value = await _cache.GetStringAsync("greeting");
        Console.WriteLine(value);
    }
}
