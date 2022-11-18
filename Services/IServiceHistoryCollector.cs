using ServiseEntities;

namespace Services;

public interface IServiceHistoryCollector
{
    public Task<List<ServiceStatus>?> GetServiceHistory(string serviceName, HistoryRequestParameters parameters);
    public Task SetOrAddServiceHistory(List<ServiceStatus> history);
    public Task<ServiceStatus?> GetServiceStatus(string serviceName);
    public Task SetOrAddServiceStatus(ServiceStatus serviceStatus);
    public Task<List<ServiceStatus>?> GetLastServicesStatus();
}
