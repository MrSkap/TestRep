using Infotecs.Nvs.Common.MongoDb.Configuration;
using Infotecs.Nvs.Common.MongoDb.Extensions;
using Infotecs.Tuby.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;

namespace Infotecs.Nvs.Common.MongoDb.Initializers;

/// <summary>
/// Инициализатор базы данных.
/// </summary>
public sealed class MongoDbInitializer : IMongoDbInitializer
{
    private const string AlreadyInitializedError = "AlreadyInitialized";
    private const string NotInitializedError = "NotYetInitialized";
    private static readonly ILogger logger = Log.ForContext<MongoDbInitializer>();
    private readonly IOptions<MongoDbConnectionOptions> connectionOptions;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="connectionOptions"><see cref="IOptions{TOptions}"/>.</param>
    public MongoDbInitializer(IOptions<MongoDbConnectionOptions> connectionOptions) => this.connectionOptions = connectionOptions;

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionOptions.Value.ConnectionString));
        settings.DirectConnection = true;
        var client = new MongoClient(settings);
        IMongoDatabase database = client.GetDatabase("admin");
        if (!await IsReplicaInitializedAsync(database))
        {
            await InitReplicaSetAsync(database);
        }

        await SetCompatibilityAsync(database, "5.0");
    }

    private static async Task InitReplicaSetAsync(IMongoDatabase database)
    {
        try
        {
            logger.Information("-> Initializing mongo replica set");
            await database.RunCommandAsync<BsonDocument>(BsonDocument.Parse("{ replSetInitiate: 1 }"));
        }
        catch (MongoCommandException ex) when (ex.CodeName == AlreadyInitializedError)
        {
            logger.Debug("Already initialized");
        }
        finally
        {
            logger.Information("<- Initializing mongo replica set");
        }
    }

    private static async Task<bool> IsReplicaInitializedAsync(IMongoDatabase database)
    {
        try
        {
            var result = await database.RunCommandAsync<BsonDocument>(BsonDocument.Parse("{ replSetGetStatus: 1 }"));
            logger.Debug("Mongo replica set status: {Status}", result.ToJson());
        }
        catch (MongoCommandException ex) when (ex.CodeName == NotInitializedError)
        {
            return false;
        }

        return true;
    }

    private static async Task SetCompatibilityAsync(IMongoDatabase database, string version)
    {
        try
        {
            var compatibility = await database.RunCommandAsync<BsonDocument>(BsonDocument.Parse("{ getParameter: 1, featureCompatibilityVersion: 1 }"));
            string currentVersion = compatibility.GetBsonValueOptional("featureCompatibilityVersion", "version").Map(x => x.AsString).Value;
            if (currentVersion != version)
            {
                await database.RunCommandAsync<BsonDocument>(BsonDocument.Parse($"{{setFeatureCompatibilityVersion: \"{version}\"}}"));
            }
        }
        catch (MongoCommandException ex) when (ex.CodeName == NotInitializedError)
        {
            logger.Warning(ex, "Command setFeatureCompatibilityVersion failed");
        }
    }
}
