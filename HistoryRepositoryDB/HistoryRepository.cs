using MongoDB.Driver;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class HistoryRepository : IHistoryRepository
{
    private readonly IMongoCollection<ServiceStatus> _collection;

    public HistoryRepository(IMongoDatabase database) => _collection = database.GetCollection<ServiceStatus>("history");

    // public async Task SetStatus(ServiceStatus service) => await _collection?.InsertOneAsync(service)!;

    //public async Task<ServiceStatus?> GetServiceStatus(string serviceName) => (await _collection.FindAsync(el => el.Name == serviceName)).First();

    public async Task AddServiceStatuses(List<ServiceStatus> serviceHistory) => await _collection.InsertManyAsync(serviceHistory);

    public async Task<List<ServiceStatus>> GetServiceStatuses(string serviceName, int offset, int take) =>
        (await _collection.FindAsync(el => true))
        .ToList()
        .OrderByDescending(el => el.TimeOfStatusUpdate)
        .Skip(offset)
        .Take(take)
        .ToList();
}
