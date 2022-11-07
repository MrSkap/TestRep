using ServiseEntities;

namespace Services;

public class ServicesStatusCollector : IServiceStatusCollector
{
    private readonly Dictionary<string, Health> _services = new();
    private readonly Dictionary<string, List<ServiceStatus>> _servicesHistory = new();

    public void ChangeServiceStatus(string serviceName, Health status)
    {
        if (_services.ContainsKey(serviceName))
        {
            _services[serviceName] = status;
        }
        else
        {
            _services.Add(serviceName, status);
        }
    }

    public Health? GetServiceStatus(string serviceName)
    {
        if (_services.ContainsKey(serviceName))
        {
            return _services[serviceName];
        }

        return null;
    }

    public void AddServiceHistory(string serviceName, List<ServiceStatus> history)
    {
        if (_servicesHistory.ContainsKey(serviceName))
        {
            _servicesHistory[serviceName]?.AddRange(history);
        }
        else
        {
            _servicesHistory[serviceName] = history;
        }
    }

    public List<ServiceStatus> GetServiceHistory(string serviceName) => _servicesHistory.GetValueOrDefault(serviceName, new List<ServiceStatus>());
}
