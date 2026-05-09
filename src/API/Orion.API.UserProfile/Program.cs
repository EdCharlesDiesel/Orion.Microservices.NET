using System.Reflection;
using Microsoft.OpenApi.Models;
using Orion.API.UserProfile;
using Orion.API.UserProfile.Mappings;
using Orion.Domain.IRepositories;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); // ✅ Fixes timestamp issues with Npgsql

var builder = WebApplication.CreateBuilder(args);

// ✅ Configuration
// var configuration = builder.Configuration;
// string connectionString = configuration.GetConnectionString("DefaultConnection");
//
// // ✅ Add EF Core with PostgreSQL
// builder.Services.AddDbContext<UserProfileDbContext>(options =>
//     options.UseNpgsql(connectionString));

// ✅ Register application services

builder.Services.AddScoped<IUserProfileServices,UserProfileRepository>();
builder.Services.AddAutoMapper(typeof(UserProfileProfile));

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
        Title = "Orion UserProfile API",
        Version = "v1",
        Description = "An API for economic events and stock analysis.",
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

public class UserProfileRepository : IUserProfileServices
{
    public async Task<IEnumerable<UserProfile>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(UserProfile entity)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(UserProfile entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetUserProfilesByDate(DateTime startDate, DateTime endDate)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetUserProfilesByCountries(params string[] countries)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetUserProfilesByCountriesAndDates(DateTime startDate, DateTime endDate, params string[] countries)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetUserProfilesByIndicator(params string[] indicators)
    {
        throw new NotImplementedException();
    }

    public async Task<UserProfile> Create(UserProfile userProfile)
    {
        throw new NotImplementedException();
    }
}
