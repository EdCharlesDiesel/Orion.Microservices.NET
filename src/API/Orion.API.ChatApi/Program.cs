// // using Orion.API.ChatApi.Hubs;
// using Orion.API.ChatApi.Services;
//
// var builder = WebApplication.CreateBuilder(args);
//
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// // Register chat service as Singleton so state is shared across all connections
// // builder.Services.AddSingleton<IChatService, ChatService>();
//
// // SignalR with JSON serialization options
// builder.Services.AddSignalR(options =>
//     {
//         options.EnableDetailedErrors = builder.Environment.IsDevelopment();
//         options.MaximumReceiveMessageSize = 32 * 1024; // 32 KB
//     })
//     .AddJsonProtocol(options =>
//     {
//         options.PayloadSerializerOptions.PropertyNamingPolicy =
//             System.Text.Json.JsonNamingPolicy.CamelCase;
//     });
//
// // CORS — allow the frontend origin
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("ChatPolicy", policy =>
//     {
//         policy.WithOrigins(
//                 builder.Configuration["AllowedOrigins"] ?? "http://localhost:3000")
//             .AllowAnyHeader()
//             .AllowAnyMethod()
//             .AllowCredentials(); // Required for SignalR
//     });
// });
//
// // ── Pipeline ──────────────────────────────────────────────────────────────────
//
// var app = builder.Build();
//
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseCors("ChatPolicy");
// app.UseAuthorization();
//
// app.MapControllers();
// // app.MapHub<ChatHub>("/hubs/chat");   // WebSocket endpoint
//
// app.Run();