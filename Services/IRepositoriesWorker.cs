using ServiseEntities;

namespace Services;

public interface IRepositoriesWorker
{
    public Task<List<ServiceStatus>> GetHistory(string serviceName, int offset, int take);
    public Task AddHistory(List<ServiceStatus> history);
    public Task AddServiceStatus(ServiceStatus status);
    public Task<List<ServiceStatus>> GetLastServicesStatus();
    public Task<ServiceStatus?> GetServiceStatus(string serviceName);
}
