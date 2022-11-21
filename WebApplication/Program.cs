using HistoryRepositoryDB;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services;
using ServiseEntities;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

var client = new MongoClient(builder.Configuration.GetSection(ServiceHistoryDatabaseOptions.ConfigurationKey)
    .GetSection("ConnectionString").Value);

builder.Services.AddTransient<IServiceHistoryCollector, ServiceHistoryCollector>();
builder.Services.AddSingleton<IHistoryRepository, HistoryRepository>(options =>
    new HistoryRepository(client.GetDatabase(
        builder.Configuration.GetSection(ServiceHistoryDatabaseOptions.ConfigurationKey)
            .GetSection("DatabaseName").Value)));
builder.Services.AddSingleton<ILastServiceStatusRepository, LastServiceStatusRepository>(options =>
    new LastServiceStatusRepository(client.GetDatabase(
        builder.Configuration.GetSection(ServiceHistoryDatabaseOptions.ConfigurationKey)
            .GetSection("DatabaseName").Value)));
builder.Services.AddSingleton<IRepositoriesWorker, RepositoriesWorker>(provider =>
    new RepositoriesWorker(provider.GetRequiredService<ILastServiceStatusRepository>(),
        provider.GetRequiredService<IHistoryRepository>(),
        client));
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

