namespace HistoryRepositoryDB;

public interface IUnitOfWork
{
    IDisposable Session { get; }
    void AddOperation(Task operation);
    void CleanOperations();
    Task SaveChanges();
}
