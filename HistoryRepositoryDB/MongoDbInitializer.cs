﻿using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class MongoDbInitializer : IMongoDbInitializer
{
    private readonly IMongoClient _client;
    private readonly ILogger _logger;

    public MongoDbInitializer(IOptions<ServiceHistoryDatabaseOptions> options, ILogger logger)
    {
        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(options.Value.ConnectionString));
        //settings.DirectConnection = true;
        _client = new MongoClient(settings);
        _logger = logger;
    }

    public async Task Initialize()
    {
        _logger.Information("Starting initializing... ");
        _logger.Information(_client.Settings.Server.Host);
        IMongoDatabase database = _client.GetDatabase("admin");
        _logger.Information(database.Settings.ToString());
        if (!await IsReplicaInitializedAsync(database))
        {
            await InitReplicaSetAsync(database);
        }
    }

    private async Task InitReplicaSetAsync(IMongoDatabase database)
    {
        _logger.Information("Create replica...");
        await database.RunCommandAsync<BsonDocument>(BsonDocument.Parse("{ replSetInitiate: 1 }"));
    }

    private async Task<bool> IsReplicaInitializedAsync(IMongoDatabase database)
    {
        try
        {
            _logger.Information(database.Settings.ToString());
            var result = await database.RunCommandAsync<BsonDocument>(BsonDocument.Parse("{ replSetGetStatus: 1 }"));
        }
        catch (Exception ex)
        {
            _logger.Error("Replica is not initialized" + "/n" + ex.Message);
            return false;
        }

        _logger.Information("Replica was created!");
        return true;
    }
}