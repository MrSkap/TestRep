using MongoDB.Driver;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class LastServiceStatusRepository : ILastServiceStatusRepository
{
    private const string _collectionName = "LastServicesStatus";
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<ServiceStatus> _collection;

    public LastServiceStatusRepository(IMongoDatabase database)
    {
        _database = database;
        _collection = _database.GetCollection<ServiceStatus>(_collectionName);
    }

    public async Task<ServiceStatus?> GetServiceStatus(string serviceName)
        => (await _collection.FindAsync(el => el.Name == serviceName)).First();

    public async Task SetServiceStatus(ServiceStatus serviceStatus) =>
        await Task.Run(() =>
        {
            _collection.ReplaceOne(el => el.Name == serviceStatus.Name, serviceStatus);
        });

    public async Task<List<ServiceStatus>?> GetAllServicesStatus()
        => await _collection.Find(service => true).ToListAsync();
}
