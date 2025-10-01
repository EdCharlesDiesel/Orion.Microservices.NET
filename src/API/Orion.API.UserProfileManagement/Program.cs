// Program.cs for UserManagement API
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Orion.API.UserProfileManagement;
using Orion.API.UserProfileManagement.Services;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Services;
using Orion.DataAccess.Postgres.Tools;

var builder = WebApplication.CreateBuilder(args);

// JWT Settings
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<OrionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<OrionDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
builder.Services.AddJwtAuthentication(jwtSettings);

// Custom services
builder.Services.AddScoped<IAuthService, AuthService>();

// Health Checks
builder.Services.AddCustomHealthChecks(builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
// app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

// Models