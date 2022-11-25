using MongoDB.Driver;

namespace HistoryRepositoryDB;

public interface IUnitOfWork
{
    public IClientSessionHandle Session { get; }
    IHistoryRepository GetHistoryRepository();
    ILastServiceStatusRepository GetLastServiceStatusRepository();
    Task SaveChanges();
}
