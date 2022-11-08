using ServiseEntities;

namespace Services;

public interface IServiceStatusCollector
{
    public void ChangeServiceStatus(string serviceName, Health status);

    public Health? GetServiceStatus(string serviceName);

    public void AddServiceHistory(string serviceName, List<ServiceStatus> history);
    public List<ServiceStatus> GetServiceHistory(string serviceName, HistoryRequestParameters parameters);
}
