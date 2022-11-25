using HistoryRepositoryDB;
using ServiseEntities;

namespace Services;

public class ServiceHistoryCollector : IServiceHistoryCollector
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMongoDbInitializer _initializer;
    private readonly ILastServiceStatusRepository _lastServiceStatusRepository;
    private readonly IHistoryRepository _historyRepository;

    public ServiceHistoryCollector(IUnitOfWork unitOfWork, IMongoDbInitializer initializer)
    {
        _unitOfWork = unitOfWork;
        _initializer = initializer;
        _lastServiceStatusRepository = unitOfWork.GetLastServiceStatusRepository();
        _historyRepository = unitOfWork.GetHistoryRepository();
    }

    public async Task<List<ServiceStatus>?> GetServiceHistory(string serviceName, HistoryRequestParameters parameters)
    {
        await _initializer.Initialize();
        return await _historyRepository.GetServiceStatuses(serviceName, parameters.Offset, parameters.Take);
    }

    public async Task SetOrAddServiceHistory(List<ServiceStatus> history)
    {
        await _initializer.Initialize();
        await _lastServiceStatusRepository.SetServiceStatus(
            history.OrderByDescending(el => el.TimeOfStatusUpdate).First());
        await _historyRepository.SetOrAddServiceStatuses(history);
        await _unitOfWork.SaveChanges();
    }

    public async Task<ServiceStatus?> GetServiceStatus(string serviceName)
    {
        await _initializer.Initialize();
        return await _lastServiceStatusRepository.GetServiceStatus(serviceName);
    }

    public async Task SetOrAddServiceStatus(ServiceStatus serviceStatus)
    {
        await _initializer.Initialize();
        await _historyRepository.SetStatus(serviceStatus);
        await _lastServiceStatusRepository.SetServiceStatus(serviceStatus);
        await _unitOfWork.SaveChanges();
    }

    public async Task<List<ServiceStatus>?> GetLastServicesStatus()
    {
        await _initializer.Initialize();
        return await _lastServiceStatusRepository.GetAllServicesStatus();
    }
}
