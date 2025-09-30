using Orion.DataAccess.Postgres.Tools;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext + UnitOfWork + Services
builder.Services.AddPostgresDataAccess(builder.Configuration);
var app = builder.Build();
app.Run();
