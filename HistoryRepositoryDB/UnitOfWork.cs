using MongoDB.Driver;
using Serilog;

namespace HistoryRepositoryDB;

public class UnitOfWork : IUnitOfWork
{
    private readonly ILogger _logger;
    public IDisposable Session => _session;
    private IClientSessionHandle _session { get; }
    private readonly List<Task> _operations;

    public UnitOfWork(IMongoClient client, ILogger logger)
    {
        _logger = logger;
        _operations = new List<Task>();
        _session = client.StartSession(new ClientSessionOptions());
    }

    public void AddOperation(Task operation) => _operations.Add(operation);

    public void CleanOperations() => _operations.Clear();

    public async Task SaveChanges()
    {
        _logger.Information("Before starting transaction");
        _session.StartTransaction();
        try
        {
            foreach (Task operation in _operations)
            {
                operation.Start();
            }

            _logger.Information("After operations");
            await Task.WhenAll(_operations);
        }
        catch (Exception ex)
        {
            _logger.Error("Error with DB operations!!!" + ex.Message);
        }

        await _session.CommitTransactionAsync();
        _logger.Information("After commit");
        _operations.Clear();
    }
}
