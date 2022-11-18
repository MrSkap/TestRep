using ServiseEntities;
using HistoryRepositoryDB;

namespace Services;

public class ServiceHistoryCollector : IServiceHistoryCollector
{
    private readonly ILastServiceStatusRepository _lastServiceStatusRepository;
    private readonly IHistoryRepository _historyRepository;

    public ServiceHistoryCollector(ILastServiceStatusRepository lastServiceStatusRepository, IHistoryRepository historyRepository)
    {
        _lastServiceStatusRepository = lastServiceStatusRepository;
        _historyRepository = historyRepository;
    }

    public async Task<List<ServiceStatus>?> GetServiceHistory(string serviceName, HistoryRequestParameters parameters)
        => await _historyRepository.GetServiceStatuses(serviceName, parameters.Offset, parameters.Take);

    public async Task SetOrAddServiceHistory(List<ServiceStatus> history)
    {
        Task[] tasks =
        {
            _historyRepository.AddServiceStatuses(history),
            _lastServiceStatusRepository.SetServiceStatus(
                history.OrderByDescending(el => el.TimeOfStatusUpdate).First()),
        };
        await Task.WhenAll(tasks);
    }

    public async Task<ServiceStatus?> GetServiceStatus(string serviceName)
        => await _lastServiceStatusRepository.GetServiceStatus(serviceName);

    public async Task SetOrAddServiceStatus(ServiceStatus serviceStatus)
    {
        Task[] tasks =
        {
            _historyRepository.SetStatus(serviceStatus),
            _lastServiceStatusRepository.SetServiceStatus(serviceStatus),
        };
        await Task.WhenAll(tasks);
    }

    public async Task<List<ServiceStatus>?> GetLastServicesStatus()
        => await _lastServiceStatusRepository.GetAllServicesStatus();
}
