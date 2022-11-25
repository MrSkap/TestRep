using MongoDB.Driver;
using Serilog;
using ServiseEntities;

namespace HistoryRepositoryDB;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public IClientSessionHandle Session { get; }
    private readonly ILogger _logger;
    private readonly IHistoryRepository _historyRepository;
    private readonly ILastServiceStatusRepository _lastServiceStatusRepository;
    private readonly MongoClient _client;

    public UnitOfWork(
        ServiceHistoryDatabaseOptions options,
        ILogger logger,
        IHistoryRepository historyRepository,
        ILastServiceStatusRepository lastServiceStatusRepository)
    {
        _logger = logger;
        _historyRepository = historyRepository;
        _lastServiceStatusRepository = lastServiceStatusRepository;
        _client = new MongoClient(MongoClientSettings.FromUrl(new MongoUrl(options.ConnectionString)));
        Session = _client.StartSession();
    }

    public IHistoryRepository GetHistoryRepository() => _historyRepository;

    public ILastServiceStatusRepository GetLastServiceStatusRepository() => _lastServiceStatusRepository;

    public async Task SaveChanges()
    {
        _logger.Information("Start commiting...");
        await Session.CommitTransactionAsync();
        _logger.Information("Finish commiting!");
    }

    public async void Dispose()
    {
        await SaveChanges();
        Session.Dispose();
    }
}
