using MongoDB.Bson;
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
        if ((await (await _collection.FindAsync(el => el.Id == serviceStatus.Id || el.Name == serviceStatus.Name)).ToListAsync()).Count == 0)
        {
            async void Operation() => await _collection.InsertOneAsync(serviceStatus);
            _unitOfWork.AddOperation(new Task(Operation));
        }
        else
        {
            UpdateDefinition<ServiceStatus>? updateHealth = Builders<ServiceStatus>.Update.Set(s => s.Health, serviceStatus.Health);
            UpdateDefinition<ServiceStatus>? updateTime = Builders<ServiceStatus>.Update.Set(s => s.TimeOfStatusUpdate, serviceStatus.TimeOfStatusUpdate);

            async void Operation()
            {
                await _collection.UpdateOneAsync
                    (el => el.Name == serviceStatus.Name, updateHealth);
                await _collection.UpdateOneAsync
                    (el => el.Name == serviceStatus.Name, updateTime);
            }

            _unitOfWork.AddOperation(new Task(Operation));
        }
    }

    public async Task<List<ServiceStatus>> GetAllServicesStatus()
        => await _collection.Find(service => true).ToListAsync();
}
