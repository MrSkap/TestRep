using MongoDB.Driver;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class LastServiceStatusRepository : ILastServiceStatusRepository
{
    private const string CollectionName = "LastServicesStatus";
    private readonly IMongoCollection<ServiceStatus> _collection;

    public LastServiceStatusRepository(IMongoDatabase database) => _collection = database.GetCollection<ServiceStatus>(CollectionName);

    public async Task<ServiceStatus?> GetServiceStatus(string serviceName) => await (await _collection.FindAsync(el => el.Name == serviceName)).FirstOrDefaultAsync();

    public async Task SetServiceStatus(ServiceStatus serviceStatus) =>
        await Task.Run(() =>
        {
            // Upsert
            _collection.ReplaceOne(el => el.Name == serviceStatus.Name, serviceStatus);
            // _collection.FindOneAndUpdate(el => el.Name == serviceStatus.Name,
            //     Builders<ServiceStatus>.Update.Set(service => service.Health, serviceStatus.Health));
            // _collection.FindOneAndUpdate(el => el.Name == serviceStatus.Name,
            //     Builders<ServiceStatus>.Update.Set(service => service.TimeOfStatusUpdate, serviceStatus.TimeOfStatusUpdate));
        });

    public async Task<List<ServiceStatus>> GetAllServicesStatus() => await _collection.Find(service => true).ToListAsync();
}
