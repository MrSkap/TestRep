using HistoryRepositoryDB;
using MongoDB.Driver;
using Services;
using ServiseEntities;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

//var client = new MongoClient("mongodb://root:root@localhost:27018");
MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl("mongodb://root:root@localhost:27018"));
settings.DirectConnection = true;
var client = new MongoClient(settings);
var initializer = new MongoDbInitializer(client);
await initializer.Initialize();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>(_ => new UnitOfWork(client));
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

