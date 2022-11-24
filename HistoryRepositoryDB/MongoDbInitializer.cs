using MongoDB.Bson;
using MongoDB.Driver;

namespace HistoryRepositoryDB;

public class MongoDbInitializer : IMongoDbInitializer
{
    private readonly IMongoClient _client;

    public MongoDbInitializer(IMongoClient client) => _client = client;

    public async Task Initialize()
    {
        IMongoDatabase database = _client.GetDatabase("admin");
        if (!await IsReplicaInitializedAsync(database))
        {
            await InitReplicaSetAsync(database);
        }
    }

    private async Task InitReplicaSetAsync(IMongoDatabase database)
        => await database.RunCommandAsync<BsonDocument>(BsonDocument.Parse("{ replSetInitiate: 1 }"));

    private async Task<bool> IsReplicaInitializedAsync(IMongoDatabase database)
    {
        try
        {
            var result = await database.RunCommandAsync<BsonDocument>(BsonDocument.Parse("{ replSetGetStatus: 1 }"));
        }
        catch (MongoCommandException ex)
        {
            return false;
        }

        return true;
    }
}
