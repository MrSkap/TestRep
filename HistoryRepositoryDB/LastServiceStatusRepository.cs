using MongoDB.Driver;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class LastServiceStatusRepository : ILastServiceStatusRepository
{
    private const string CollectionName = "LastServicesStatus";
    private readonly IMongoCollection<ServiceStatus> _collection;

    public LastServiceStatusRepository(IMongoDatabase database) => _collection = database.GetCollection<ServiceStatus>(CollectionName);

    public async Task<ServiceStatus?> GetServiceStatus(string serviceName)
        => (await _collection.FindAsync(el => el.Name == serviceName)).First();

    public async Task SetServiceStatus(ServiceStatus serviceStatus) =>
        await _collection.ReplaceOneAsync(el => el.Name == serviceStatus.Name, serviceStatus, new ReplaceOptions { IsUpsert = true });

    public async Task<List<ServiceStatus>> GetAllServicesStatus()
        => await _collection.Find(service => true).ToListAsync();
}
