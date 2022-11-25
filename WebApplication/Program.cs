using HistoryRepositoryDB;
using MongoDB.Driver;
using Serilog;
using Serilog.Core;
using Services;
using ServiseEntities;
using ILogger = Serilog.ILogger;

WebApplicationBuilder builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.Configure<ServiceHistoryDatabaseOptions>(
    builder.Configuration.GetSection("ServiceHistoryDatabase"));
builder.Services.AddTransient<IMongoDbInitializer, MongoDbInitializer>();

builder.Services.AddTransient<ILogger, Logger>(_ => new LoggerConfiguration().WriteTo.Console().CreateLogger());

builder.Services.AddSingleton<IServiceHistoryCollector, ServiceHistoryCollector>(provider => new ServiceHistoryCollector(
    provider.GetRequiredService<IUnitOfWork>(),
    provider.GetRequiredService<IMongoDbInitializer>()));

builder.Services.AddControllers();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        policy =>
        {
            policy.WithOrigins(
                    "https://localhost:44398",
                    "http://localhost:4200",
                    "http://localhost:80")
                .AllowAnyMethod();
        });
});
Microsoft.AspNetCore.Builder.WebApplication app = builder.Build();
app.MapControllers();
app.UseCors("MyPolicy");
app.Run();
