using MongoDB.Driver;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class LastServiceStatusRepository: ILastServiceStatusRepository
{
    private const string _collectionName = "LastServicesStatus";
    private IMongoDatabase _database;
    private IMongoCollection<ServiceStatus> _collection;
    public LastServiceStatusRepository(IMongoDatabase database)
    {
        _database = database;
        _collection = _database.GetCollection<ServiceStatus>(_collectionName);
    }
    public async Task<ServiceStatus?> GetServiceStatus(string serviceName)
    {
        return (await _collection.FindAsync(el => el.Name == serviceName)).First();
    }

    public async Task SetServiceStatus(ServiceStatus serviceStatus) =>
        await Task.Run(() =>
        {
            _collection.ReplaceOne(el => el.Name == serviceStatus.Name, serviceStatus);
            // _collection.FindOneAndUpdate(el => el.Name == serviceStatus.Name,
            //     Builders<ServiceStatus>.Update.Set(service => service.Health, serviceStatus.Health));
            // _collection.FindOneAndUpdate(el => el.Name == serviceStatus.Name,
            //     Builders<ServiceStatus>.Update.Set(service => service.TimeOfStatusUpdate, serviceStatus.TimeOfStatusUpdate));
        });

    public async Task<List<ServiceStatus>?> GetAllServicesStatus() => await _collection.Find(service => true).ToListAsync();
}
