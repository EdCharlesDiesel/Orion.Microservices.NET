using System.Reflection;
using System.Threading.RateLimiting;
using JasperFx;
using Marten;
using Microsoft.OpenApi.Models;
using Orion.API.TradingEconomics.Configuration;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Engine.Interfaces.Orion.API.TradingEconomics.Engine.Interfaces;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Helpers;
using Orion.API.TradingEconomics.Interfaces;
using Orion.API.TradingEconomics.Services;
using YahooQuotesApi;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMarten(options =>
{
    options.Connection(
        builder.Configuration.GetConnectionString("MacroDbConnection")
        ?? throw new InvalidOperationException("Missing MacroDb connection string."));

    options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

    options.Schema.For<TradePlan>()
        .Index(x => x.Status)
        .Index(x => x.Pair)
        .Index(x => x.OpenedAt)
        .Index(x => x.ClosedAt);

    options.Schema.For<OrderRequest>()
        .Index(x => x.Status)
        .Index(x => x.Pair)
        .Index(x => x.CreatedAt);

    options.Schema.For<OrderState>()
        .Index(x => x.Status)
        .Index(x => x.Pair)
        .Index(x => x.FilledAt);

    // options.Schema.For<AuditRecord>()
        // .Index(x => x.Stage)
        // .Index(x => x.Status)
        // .Index(x => x.Pair)
        // .Index(x => x.Re);
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Orion TradingEconomics API",
        Version = "v1",
        Description = "An API for economic events and stock analysis.",
        Contact = new OpenApiContact
        {
            Name = "Khotso Mokhethi",
            Email = "Mokhetkc@hotmail.com",
            Url = new Uri("https://github.com/EdCharlesDiesel")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.Configure<AppConfiguration>(
    builder.Configuration.GetSection("AppConfiguration"));

builder.Services.AddHttpClient("FRED", client =>
{
    client.BaseAddress = new Uri("https://api.stlouisfed.org/fred/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.Configure<MarketPipelineOptions>(options =>
{
    options.EnableCaching = true;
    options.CacheExpirationSeconds = 300;
    options.ValidationRetries = 2;
    options.EnableEnrichment = true;
});

builder.Services.AddMemoryCache();

builder.Services.AddSingleton(new NormalizationOptions
{
    MinimumWindowSize = 6,
    WinsorizeOutliers = true,
    WinsorizeZLimit = 4.0m
});

builder.Services.AddScoped<INormalizationEngine, NormalizationEngine>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();

builder.Services.AddSingleton<YahooQuotes>(sp =>
    new YahooQuotesBuilder()
        .WithLogger(sp.GetRequiredService<ILogger<YahooQuotes>>())
        .Build());

//Services
builder.Services.AddScoped<IFredService, FredService>();
builder.Services.AddScoped<IOrderBookProvider, OrderBookProvider>();
builder.Services.AddScoped<IAuditStorage, AuditStorage>();


//Engines
builder.Services.AddScoped<IAdvancedExecutionEngine,AdvancedExecutionEngine>();
builder.Services.AddScoped<IAlertEngine,AlertEngine>();
builder.Services.AddScoped<IAlphaEngine,AlphaEngine>();
builder.Services.AddScoped<IAuditTrailEngine,AuditTrailEngine>();
builder.Services.AddScoped<IBacktestEngine,BacktestEngine>();
builder.Services.AddScoped<ICircuitBreakerEngine,CircuitBreakerEngine>();
builder.Services.AddScoped<IComplianceEngine, ComplianceEngine>();
builder.Services.AddScoped<ICorrelationEngine, CorrelationEngine>();
builder.Services.AddScoped<IConfigurationEngine, ConfigurationEngine>();
builder.Services.AddScoped<IDataQualityEngine, DataQualityEngine>();
builder.Services.AddScoped<IMacroSimulationEngine, DynamicMacroSimulationEngine>();
builder.Services.AddScoped<IEconomicCalendarRiskEngine, EconomicCalendarRiskEngine>();
builder.Services.AddScoped<IExecutionEngine, ExecutionEngine>();
builder.Services.AddScoped<IExitEngine, ExitEngine>();
builder.Services.AddScoped<IFxPricingEngine, FxPricingEngine>();
builder.Services.AddScoped<IHedgingEngine, HedgingEngine>();
builder.Services.AddScoped<ILiquidityEngine, LiquidityEngine>();
builder.Services.AddScoped<IMarketDataEngine, MarketDataEngine>();
builder.Services.AddScoped<IMarketReplayEngine, MarketReplayEngine>();
builder.Services.AddScoped<IModelValidationEngine, ModelValidationEngine>();
builder.Services.AddScoped<IMonteCarloEngine, MonteCarloEngine>();
builder.Services.AddScoped<INormalizationEngine, NormalizationEngine>();
builder.Services.AddScoped<IOrderManagementEngine, OrderManagementEngine>();
builder.Services.AddScoped<IPerformanceAnalyticsEngine, PerformanceAnalyticsEngine>();
builder.Services.AddScoped<IPortfolioEngine, PortfolioEngine>();
builder.Services.AddScoped<IPositionSizingEngine, PositionSizingEngine>();
builder.Services.AddScoped<IProbabilisticScenarioEngine, ProbabilisticScenarioEngine>();
builder.Services.AddScoped<IRealBacktestEngine, RealBacktestEngine>();
builder.Services.AddScoped<IRealTimeRiskEngine, RealTimeRiskEngine>();
builder.Services.AddScoped<IRegimeEngine, RegimeEngine>();
builder.Services.AddScoped<IRiskEngine, RiskEngine>();
builder.Services.AddScoped<IScenarioEngine, ScenarioEngine>();
builder.Services.AddScoped<ISentimentEngine, SentimentEngine>();
builder.Services.AddScoped<ITradeLifecycleEngine, TradeLifecycleEngine>();
// builder.Services.AddScoped<IWalkForwardEngine, WalkForwardEngine>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        httpContext => RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("AngularApp");

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();