using ServiseEntities;

namespace Services;

public class ServicesStatusCollector : IServiceStatusCollector
{
    private readonly Dictionary<string, Health> _services = new();
    private readonly Dictionary<string, IEnumerable<ServiceStatus>> _servicesHistory = new();

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

    public void SetServiceHistory(string serviceName, IEnumerable<ServiceStatus> history)
    {
        if (_servicesHistory.ContainsKey(serviceName))
        {
            _servicesHistory[serviceName] = _servicesHistory[serviceName].Concat(history);
        }
        else
        {
            _servicesHistory.Add(serviceName, history);
        }
    }

    public IEnumerable<ServiceStatus>? GetServiceHistory(string serviceName) => _servicesHistory.ContainsKey(serviceName) ? _servicesHistory[serviceName] : null;
}
