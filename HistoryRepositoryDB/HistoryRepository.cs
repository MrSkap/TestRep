using MongoDB.Driver;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class HistoryRepository : IHistoryRepository
{
    private readonly IClientSessionHandle _session;
    private readonly IMongoCollection<ServiceStatus> _collection;
    private const string DatabaseName = "admin";
    private const string CollectionName = "History";

    public HistoryRepository(IClientSessionHandle session)
    {
        _session = session;
        _collection = _session.Client.GetDatabase(DatabaseName).GetCollection<ServiceStatus>(CollectionName);
    }

    public async Task SetStatus(ServiceStatus service) => await _collection.InsertOneAsync(_session, service);

    public async Task SetOrAddServiceStatuses(IEnumerable<ServiceStatus> serviceHistory)
    {
        var options = new InsertManyOptions
        {
            IsOrdered = true,
        };
        await _collection.InsertManyAsync(_session, serviceHistory, options);
    }

    public async Task<List<ServiceStatus>> GetServiceStatuses(string serviceName, int offset, int take) =>
        (await _collection.FindAsync(_session, el => el.Name == serviceName))
        .ToList()
        .OrderByDescending(el => el.TimeOfStatusUpdate)
        .Skip(offset)
        .Take(take)
        .ToList();
}
