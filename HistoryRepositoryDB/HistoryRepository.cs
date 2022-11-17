using Microsoft.Extensions.Options;
using MongoDB.Bson;
using ServiseEntities;
using MongoDB.Driver;
using Services;

namespace HistoryRepositoryDB;

public class HistoryRepository//:IServiceStatusCollector
{
    private readonly MongoClient _client;
    private readonly IOptions<ServiceHistoryDatabaseOptions> _config;
    private IMongoCollection<ServiceHistroy> _allServicesHistory;
    private IMongoCollection<ServiceStatus> _lastServicesStatus;
    public HistoryRepository(IOptions<ServiceHistoryDatabaseOptions> config)
    {
        _config = config;
        _client = new MongoClient(_config.Value.ConnctionString);
        _allServicesHistory = _client.GetDatabase(_config.Value.DatabaseName).GetCollection<ServiceHistroy>(_config.Value.AllServicesHistoryCollectionName);
        _lastServicesStatus = _client.GetDatabase(_config.Value.DatabaseName).GetCollection<ServiceStatus>(_config.Value.LastServicesStatusCollectionName);
    }
    public List<ServiceStatus> GetHistory() => throw new NotImplementedException();

    public void SetHistory() => throw new NotImplementedException();
    public void ChangeServiceStatus(string serviceName, Health status, DateTimeOffset time)
    {
        _allServicesHistory.FindOneAndUpdate(element => element.Name == serviceName,
            Builders<ServiceHistroy>.Update.AddToSet("History", new ServiceStatus(serviceName, status, time)));
        _lastServicesStatus.FindOneAndReplace(el => el.Name == serviceName, new ServiceStatus(serviceName, status, time));
    }

    public async Task<ServiceStatus> GetServiceStatus(string serviceName)
    {
        return _lastServicesStatus.FindAsync(el => el.Name == serviceName).Result.Current.First();
    }

    public async void AddServiceHistory(string serviceName, List<ServiceStatus> history)
    {
        _allServicesHistory.FindOneAndUpdate(element => element.Name == serviceName,
            Builders<ServiceHistroy>.Update.Push("History", history));
        _lastServicesStatus.FindOneAndReplace(el => el.Name == serviceName,
                (await _allServicesHistory.FindAsync(element => element.Name == serviceName))
                                .ToList()[0].History.OrderByDescending(el => el.TimeOfStatusUpdate).First());
    }

    public async Task<List<ServiceStatus>> GetServiceHistory(string serviceName, HistoryRequestParameters parameters)
    {
        return (await _allServicesHistory.FindAsync(element => element.Name == serviceName))
            .ToList()[0].History;
    }

    public async Task<List<ServiceStatus>> GetServicesStatus()
    {
        return await _lastServicesStatus.Find(x => true).ToListAsync();
    }
}
