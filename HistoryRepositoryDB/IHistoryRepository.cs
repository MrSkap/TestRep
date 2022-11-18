using ServiseEntities;
using Microsoft.Extensions.Options;
namespace HistoryRepositoryDB;

public interface IHistoryRepository
{
    public Task SetStatus(ServiceStatus service);
    public Task SetOrAddServiceStatuses(IEnumerable<ServiceStatus> serviceHistory);
    public Task<List<ServiceStatus>?> GetServiceStatuses(string serviceName, int offset, int take);

}
