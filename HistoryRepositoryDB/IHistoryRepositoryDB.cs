using ServiseEntities;
using Microsoft.Extensions.Options;
namespace HistoryRepositoryDB;

public interface IHistoryRepositoryDB
{
    public Task SetStatus(ServiceStatus service);
    public Task<ServiceStatus> GetServiceStatus(string serviceName);
    public Task SetOrAddServiceStatuses(List<ServiceStatus> serviceHistory);
    public Task<List<ServiceStatus>> GetServiceStatuses(string serviceName, int offset, int take);

}
