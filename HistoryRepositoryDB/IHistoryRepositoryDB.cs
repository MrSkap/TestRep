using ServiseEntities;
using Microsoft.Extensions.Options;
namespace HistoryRepositoryDB;

public interface IHistoryRepositoryDB
{
    public List<ServiceStatus> GetHistory();
    public void SetHistory();
}
