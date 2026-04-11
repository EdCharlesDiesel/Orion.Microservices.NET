using MudBlazor.Services;
using Orion.WebApps.AanalysisDashboardBlazor.Components;
using Orion.WebApps.AanalysisDashboardBlazor.Interfaces;
using Orion.WebApps.AanalysisDashboardBlazor.Services;

namespace Orion.WebApps.AanalysisDashboardBlazor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure services
        ConfigureServices(builder);

        var app = builder.Build();

        // Configure middleware pipeline
        ConfigureMiddleware(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Add Razor and Blazor services
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        // Add MudBlazor for UI components
        builder.Services.AddMudServices();

        // Add caching
        builder.Services.AddMemoryCache();

        // Add HTTP client for API calls
        builder.Services.AddHttpClient();

        // Register application services
        RegisterServerServices(builder.Services);
    }

    private static void RegisterServerServices(IServiceCollection services)
    {
        // Singleton services (shared across all requests)
        services.AddSingleton<YahooFinanceProvider>();
        services.AddSingleton<IMarketDataProvider>(sp =>
            sp.GetRequiredService<YahooFinanceProvider>());
        services.AddSingleton<TechnicalIndicatorService>();
        services.AddSingleton<EntrySignalService>();
        services.AddSingleton<TradingIdeaService>();

        // Scoped services (per request)
        services.AddScoped<DataService>();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        // Development-specific configuration
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts(); // HTTP Strict Transport Security
        }

        // Security and routing
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();
        app.UseRouting();

        // Map endpoints
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
    }
}