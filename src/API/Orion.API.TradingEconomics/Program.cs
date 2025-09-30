using System.Reflection;
using Microsoft.OpenApi.Models;
using Orion.API.TradingEconomics.Mappings;
using Orion.DataAccess.Postgres.Repositories;
using Orion.Domain.IRepositories;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); // ✅ Fixes timestamp issues with Npgsql

var builder = WebApplication.CreateBuilder(args);

// ✅ Configuration
// var configuration = builder.Configuration;
// string connectionString = configuration.GetConnectionString("DefaultConnection");
//
// // ✅ Add EF Core with PostgreSQL
// builder.Services.AddDbContext<TradingEconomicsContext>(options =>
//     options.UseNpgsql(connectionString));

// ✅ Register application services

 builder.Services.AddScoped<CalendarRepository>();
// builder.Services.AddScoped<IComtradeServices, ComtradeRepository>();
// builder.Services.AddScoped<IForecastServices, ForecastRepository>();
// builder.Services.AddAutoMapper(typeof(CalendarEventProfile));

// ✅ Add HTTP client support if needed
builder.Services.AddHttpClient(); // Or named client if needed

// ✅ Add controller support
builder.Services.AddControllers();

// ✅ Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Orion Trading Economics API",
        Version = "v1",
        Description = "An API for Trading Economics events and analysis.",
        Contact = new OpenApiContact 
        {
            Name = "Khotso Mokhethi",
            Email = "Mokhetkc@hotmail.com", // Replace with your actual email
            Url = new Uri("https://github.com/EdCharlesDiesel")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Optional: Include XML comments (if you enable them in .csproj)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Optional: Add JWT bearer auth support if you're using authentication
    /*
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    */
});

var app = builder.Build();

// ✅ Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Better error info in dev
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error"); // Optional: your custom error endpoint
    app.UseHsts(); // Enforce HTTPS in production
}

app.UseHttpsRedirection();

// Optional: add if you configure authentication
// app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class CalendarRepository
{
}