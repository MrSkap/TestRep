using HistoryRepositoryDB;
using MongoDB.Driver;
using Serilog;
using Serilog.Core;
using Services;
using ServiseEntities;
using ILogger = Serilog.ILogger;

WebApplicationBuilder builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>(provider => new UnitOfWork(
    provider.GetRequiredService<ServiceHistoryDatabaseOptions>(),
    provider.GetRequiredService<ILogger>(),
    provider.GetRequiredService<IHistoryRepository>(),
    provider.GetRequiredService<ILastServiceStatusRepository>()));
builder.Services.AddTransient<ServiceHistoryDatabaseOptions>(_ => new ServiceHistoryDatabaseOptions(){ ConnectionString = builder.Configuration.GetSection("ServiceHistoryDatabase").Value});
builder.Services.AddTransient<IMongoDbInitializer, MongoDbInitializer>(provider => new MongoDbInitializer(
    provider.GetRequiredService<ServiceHistoryDatabaseOptions>(),
    provider.GetRequiredService<ILogger>()));

builder.Services.AddTransient<ILogger, Logger>(_ => new LoggerConfiguration().WriteTo.Console().CreateLogger());

builder.Services.AddTransient<IHistoryRepository, HistoryRepository>(provider =>
    new HistoryRepository(provider.GetRequiredService<IUnitOfWork>().Session));

builder.Services.AddTransient<ILastServiceStatusRepository, LastServiceStatusRepository>(provider =>
    new LastServiceStatusRepository(provider.GetRequiredService<IUnitOfWork>().Session));

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
