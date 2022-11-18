using ServiseEntities;
using Microsoft.Extensions.Options;
namespace HistoryRepositoryDB;

public interface IHistoryRepository
{
    //public Task SetStatus(ServiceStatus service);
    //public Task<ServiceStatus?> GetServiceStatus(string serviceName);
    public Task AddServiceStatuses(List<ServiceStatus> serviceHistory);
    public Task<List<ServiceStatus>> GetServiceStatuses(string serviceName, int offset, int take);

}
