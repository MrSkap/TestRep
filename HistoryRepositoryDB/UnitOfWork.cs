using Microsoft.Extensions.Options;
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
        IOptions<ServiceHistoryDatabaseOptions> options,
        ILogger logger)
    {
        _logger = logger;
        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(options.Value.ConnectionString));
        settings.DirectConnection = true;
        _client = new MongoClient(settings);
        Session = _client.StartSession();
        _historyRepository = new HistoryRepository(Session);
        _lastServiceStatusRepository = new LastServiceStatusRepository(Session);
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
