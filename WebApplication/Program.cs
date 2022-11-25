using HistoryRepositoryDB;
using MongoDB.Driver;
using Services;
using ServiseEntities;
using Serilog;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(
    builder.Configuration.GetSection(ServiceHistoryDatabaseOptions.ConfigurationKey).GetSection("ConnectionString").Value));
settings.DirectConnection = true;
var client = new MongoClient(settings);

Log.Logger.Information(client.Settings.ToString());

var initializer = new MongoDbInitializer(client, Log.Logger);
await initializer.Initialize();
Log.Logger.Information("Initialization is completed!");
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>(_ => new UnitOfWork(client, Log.Logger));
builder.Services.AddSingleton<IHistoryRepository, HistoryRepository>(provider =>
    new HistoryRepository(
        client.GetDatabase(builder.Configuration.GetSection(ServiceHistoryDatabaseOptions.ConfigurationKey).GetSection("DatabaseName").Value),
        provider.GetRequiredService<IUnitOfWork>()));
builder.Services.AddSingleton<ILastServiceStatusRepository, LastServiceStatusRepository>(provider =>
    new LastServiceStatusRepository(
        client.GetDatabase(builder.Configuration.GetSection(ServiceHistoryDatabaseOptions.ConfigurationKey).GetSection("DatabaseName").Value),
        provider.GetRequiredService<IUnitOfWork>()));
builder.Services.AddSingleton<IServiceHistoryCollector, ServiceHistoryCollector>(provider => new ServiceHistoryCollector(
    provider.GetRequiredService<IUnitOfWork>(),
    provider.GetRequiredService<ILastServiceStatusRepository>(),
    provider.GetRequiredService<IHistoryRepository>()));
builder.Services.AddControllers();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});
builder.Services.AddCors(options => {
    options.AddPolicy(name: "MyPolicy",
        policy =>
        {
            policy.WithOrigins(
                    "https://localhost:44398",
                    "http://localhost:4200",
                    "http://localhost:80")
                .AllowAnyMethod();
        });
});
var app = builder.Build();
app.MapControllers();
app.UseCors("MyPolicy");
app.Run();

