using ServiseEntities;

namespace HistoryRepositoryDB;

public interface ILastServiceStatusRepository
{
    public Task<ServiceStatus?> GetServiceStatus(string serviceName);
    public Task SetServiceStatus(ServiceStatus serviceStatus);
    public Task<List<ServiceStatus>?> GetAllServicesStatus();
}
