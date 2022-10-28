using ServiseEntities;

namespace Services;

public interface IServiceStatusCollector
{
    public void ChangeServiceStatus(string serviceName, Health status);

    public Health? GetServiceStatus(string serviceName);
}
