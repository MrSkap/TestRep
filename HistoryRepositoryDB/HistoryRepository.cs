using Microsoft.Extensions.Options;
using ServiseEntities;
using MongoDB.Driver;

namespace HistoryRepositoryDB;

public class HistoryRepository:IHistoryRepositoryDB
{
    private readonly MongoClient _client;
    private readonly IOptions<DBConnection> _config;
    public HistoryRepository(IOptions<DBConnection> config)
    {
        _config = config;
        _client = new MongoClient(_config.Value.ConectionString);


    }
    public List<ServiceStatus> GetHistory() => throw new NotImplementedException();

    public void SetHistory() => throw new NotImplementedException();
}
