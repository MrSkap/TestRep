using System.Collections.Concurrent;
using ServiseEntities;

namespace Services;

public class ServicesStatusCollector : IServiceStatusCollector
{
    private readonly ConcurrentDictionary<string, List<ServiceStatus>> _servicesHistory = new();

    public void ChangeServiceStatus(string serviceName, Health status)
    {
        if (_servicesHistory.ContainsKey(serviceName))
        {
            _servicesHistory[serviceName].Add(new ServiceStatus(serviceName, status));
        }
        else
        {
            var list = new List<ServiceStatus> { new ServiceStatus(serviceName, status) };
            _servicesHistory.TryAdd(serviceName, list);
        }
    }

    public Health? GetServiceStatus(string serviceName)
    {
        if (_servicesHistory.ContainsKey(serviceName))
        {
            return _servicesHistory[serviceName].Last().Health;
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

    public List<ServiceStatus> GetServiceHistory(string serviceName, ServiceStatusRequestParameters parameters)
        => _servicesHistory.GetValueOrDefault(serviceName, new List<ServiceStatus>()).Skip(parameters.Offset).OrderByDescending(status => status.TimeOfStatusUpdate).Take(parameters.Take).ToList();
}
