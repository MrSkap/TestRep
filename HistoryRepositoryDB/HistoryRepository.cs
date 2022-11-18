using Microsoft.Extensions.Options;
using MongoDB.Bson;
using ServiseEntities;
using MongoDB.Driver;

namespace HistoryRepositoryDB;

public class HistoryRepository : IHistoryRepositoryDB
{
    private readonly IMongoDatabase _database;

    public HistoryRepository(IMongoDatabase database) => _database = database;

    public async Task SetStatus(ServiceStatus service) =>
        await Task.Run(() =>
        {
            IMongoCollection<ServiceStatus>? collection = _database.GetCollection<ServiceStatus>(service.Name);
            collection?.InsertOne(service);
        });

    public async Task<ServiceStatus?> GetServiceStatus(string serviceName)
    {
        IMongoCollection<ServiceStatus>? collection = _database.GetCollection<ServiceStatus>(serviceName);
        return (await collection.FindAsync(el => el.Name == serviceName)).First();
    }

    public async Task SetOrAddServiceStatuses(List<ServiceStatus> serviceHistory)
    {
        IMongoCollection<ServiceStatus>? collection = _database.GetCollection<ServiceStatus>(serviceHistory.First().Name);
        await collection.InsertManyAsync(serviceHistory);
    }

    public async Task<List<ServiceStatus>?> GetServiceStatuses(string serviceName, int offset, int take)
    {
        IMongoCollection<ServiceStatus>? collection = _database.GetCollection<ServiceStatus>(serviceName);
        return (await collection.FindAsync(el => true))
            .ToList()
            .OrderByDescending(el => el.TimeOfStatusUpdate)
            .Skip(offset)
            .Take(take)
            .ToList();
    }
}
