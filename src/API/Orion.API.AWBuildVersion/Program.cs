using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Orion.API.AWBuildVersion.Mappings;
using Orion.DataAccess.Postgres.Tools;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();
// Service registration
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddPostgresDataAccess(builder.Configuration);

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
        Title = "Orion AWBuildVersion API",
        Version = "v1",
        Description = "An API for AWBuildVersion feature for Orion.",
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
     
});

// 🔐 Add Authentication with JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["OrionApi"],
            ValidAudience = builder.Configuration["Jwt:OrionApiUsers"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });


builder.Services.AddAuthorization();
var app = builder.Build();

app.UseCors(builder => 
    builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader());

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
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

