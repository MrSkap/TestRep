using MongoDB.Driver;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class LastServiceStatusRepository : ILastServiceStatusRepository
{
    private readonly IClientSessionHandle _session;
    private const string DatabaseName = "admin";
    private const string CollectionName = "LastServicesStatus";
    private readonly IMongoCollection<ServiceStatus> _collection;

    public LastServiceStatusRepository(IClientSessionHandle session)
    {
        _session = session;
        _collection = session.Client.GetDatabase(DatabaseName).GetCollection<ServiceStatus>(CollectionName);
    }

    public async Task<ServiceStatus?> GetServiceStatus(string serviceName)
    {
        IAsyncCursor<ServiceStatus>? result = await _collection.FindAsync(_session, el => el.Name == serviceName);
        return (await result.ToListAsync()).Count > 0 ? result.First() : null;
    }

    public async Task SetServiceStatus(ServiceStatus serviceStatus)
    {
        if ((await (await _collection.FindAsync(el => el.Id == serviceStatus.Id || el.Name == serviceStatus.Name)).ToListAsync()).Count == 0)
        {
            await _collection.InsertOneAsync(_session, serviceStatus);
        }
        else
        {
            await _collection.UpdateOneAsync
            (_session, el => el.Name == serviceStatus.Name,
                Builders<ServiceStatus>.Update.Set(s => s.Health, serviceStatus.Health));
            await _collection.UpdateOneAsync
            (_session, el => el.Name == serviceStatus.Name,
                Builders<ServiceStatus>.Update.Set(s => s.TimeOfStatusUpdate, serviceStatus.TimeOfStatusUpdate));
            await _collection.UpdateOneAsync
            (_session, el => el.Id == serviceStatus.Id,
                Builders<ServiceStatus>.Update.Set(s => s.Name, serviceStatus.Name));
        }
    }

    public async Task<List<ServiceStatus>> GetAllServicesStatus()
        => await _collection.Find(_session, service => true).ToListAsync();
}
