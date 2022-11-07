using ServiseEntities;

namespace Services;

public interface IServiceStatusCollector
{
    public void ChangeServiceStatus(string serviceName, Health status);

    public Health? GetServiceStatus(string serviceName);

    public void SetServiceHistory(string serviceName, IEnumerable<ServiceStatus> history);
    public IEnumerable<ServiceStatus>? GetServiceHistory(string serviceName);
}
