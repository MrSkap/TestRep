using MongoDB.Driver;

namespace HistoryRepositoryDB;

public class UnitOfWork : IUnitOfWork
{
    public IDisposable Session => _session;
    private IClientSessionHandle _session { get; }
    private List<Task> _operations;

    public UnitOfWork(IMongoClient client)
    {
        _operations = new List<Task>();
        _session = client.StartSession(new ClientSessionOptions());
    }

    public void AddOperation(Task operation) => _operations.Add(operation);

    public void CleanOperations() => _operations.Clear();

    public async Task SaveChanges()
    {
        _session.StartTransaction();
        foreach (Task operation in _operations)
        {
            operation.Start();
        }

        await Task.WhenAll(_operations);
        await _session.CommitTransactionAsync();
        _operations.Clear();
    }
}
