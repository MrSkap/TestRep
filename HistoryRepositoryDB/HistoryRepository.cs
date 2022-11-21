using Microsoft.Extensions.Options;
using MongoDB.Bson;
using ServiseEntities;
using MongoDB.Driver;

namespace HistoryRepositoryDB;

public class HistoryRepository : IHistoryRepository
{
    private readonly IMongoCollection<ServiceStatus> _collection;

    public HistoryRepository(IMongoDatabase database) => _collection = database.GetCollection<ServiceStatus>("History");

    public async Task SetStatus(ServiceStatus service) => await _collection.InsertOneAsync(service);

    public async Task SetOrAddServiceStatuses(IEnumerable<ServiceStatus> serviceHistory) => await _collection.InsertManyAsync(serviceHistory);

    public async Task<List<ServiceStatus>> GetServiceStatuses(string serviceName, int offset, int take) =>
        (await _collection.FindAsync(el => el.Name == serviceName))
        .ToList()
        .OrderByDescending(el => el.TimeOfStatusUpdate)
        .Skip(offset)
        .Take(take)
        .ToList();
}
