using MongoDB.Driver;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class LastServiceStatusRepository : ILastServiceStatusRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private const string CollectionName = "LastServicesStatus";
    private readonly IMongoCollection<ServiceStatus> _collection;


    public LastServiceStatusRepository(IMongoDatabase database, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _collection = database.GetCollection<ServiceStatus>(CollectionName);
    }

    public async Task<ServiceStatus?> GetServiceStatus(string serviceName)
    {
        IAsyncCursor<ServiceStatus>? result = await _collection.FindAsync(el => el.Name == serviceName);
        return (await result.ToListAsync()).Count > 0 ? (await _collection.FindAsync(el => el.Name == serviceName)).First() : null;
    }

    public async Task SetServiceStatus(ServiceStatus serviceStatus)
    {
        serviceStatus.Id = (await _collection.FindAsync(el=>el.Name == serviceStatus.Name)).First().Id;
        async void Operation() => await _collection.ReplaceOneAsync
            (el => el.Name == serviceStatus.Name, serviceStatus, new ReplaceOptions { IsUpsert = true });
        _unitOfWork.AddOperation(new Task(Operation));
    }

    public async Task<List<ServiceStatus>> GetAllServicesStatus()
        => await _collection.Find(service => true).ToListAsync();
}
