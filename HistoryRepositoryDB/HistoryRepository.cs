using MongoDB.Driver;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class HistoryRepository : IHistoryRepository
{
    private readonly IMongoCollection<ServiceStatus> _collection;
    private readonly IUnitOfWork _unitOfWork;

    public HistoryRepository(IMongoDatabase database, IUnitOfWork unitOfWork)
    {
        _collection = database.GetCollection<ServiceStatus>("History");
        _unitOfWork = unitOfWork;
    }

    public Task SetStatus(ServiceStatus service)
    {
        async void Operation() => await _collection.InsertOneAsync(service);
        _unitOfWork.AddOperation(new Task(Operation));
        return Task.CompletedTask;
    }

    public Task SetOrAddServiceStatuses(IEnumerable<ServiceStatus> serviceHistory)
    {
        var options = new InsertManyOptions
        {
            IsOrdered = true,
        };
        async void Operation() => await _collection.InsertManyAsync(serviceHistory, options);
        _unitOfWork.AddOperation(new Task(Operation));
        return Task.CompletedTask;
    }

    public async Task<List<ServiceStatus>> GetServiceStatuses(string serviceName, int offset, int take) =>
        (await _collection.FindAsync(el => el.Name == serviceName))
        .ToList()
        .OrderByDescending(el => el.TimeOfStatusUpdate)
        .Skip(offset)
        .Take(take)
        .ToList();
}
