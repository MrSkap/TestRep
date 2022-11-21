using ServiseEntities;

namespace Services;

public class ServiceHistoryCollector : IServiceHistoryCollector
{
    private readonly IRepositoriesWorker _repositoriesWorker;
    public ServiceHistoryCollector(IRepositoriesWorker repositoriesWorker) => _repositoriesWorker = repositoriesWorker;

    public async Task<List<ServiceStatus>?> GetServiceHistory(string serviceName, HistoryRequestParameters parameters)
        => await _repositoriesWorker.GetHistory(serviceName, parameters.Offset, parameters.Take);

    public async Task SetOrAddServiceHistory(List<ServiceStatus> history) => await _repositoriesWorker.AddHistory(history);

    public async Task<ServiceStatus?> GetServiceStatus(string serviceName)
        => await _repositoriesWorker.GetServiceStatus(serviceName);

    public async Task SetOrAddServiceStatus(ServiceStatus serviceStatus) => await _repositoriesWorker.AddServiceStatus(serviceStatus);

    public async Task<List<ServiceStatus>?> GetLastServicesStatus()
        => await _repositoriesWorker.GetLastServicesStatus();
}
