using System.Collections.Concurrent;
using ServiseEntities;

namespace Services;

public class ServicesStatusCollector : IServiceStatusCollector
{
    private readonly ConcurrentDictionary<string, List<ServiceStatus>> _servicesHistory = new();

    public void ChangeServiceStatus(string serviceName, Health status)
        => _servicesHistory.GetOrAdd(serviceName, new List<ServiceStatus>()).Add(new ServiceStatus(serviceName, status));

    public Health? GetServiceStatus(string serviceName)
    {
        if (_servicesHistory.ContainsKey(serviceName))
        {
            return _servicesHistory[serviceName].Last().Health;
        }

        return null;
    }

    public void AddServiceHistory(string serviceName, List<ServiceStatus> history)
        => _servicesHistory.GetOrAdd(serviceName, new List<ServiceStatus>()).AddRange(history);

    public List<ServiceStatus> GetServiceHistory(string serviceName, HistoryRequestParameters parameters)
        => _servicesHistory.GetValueOrDefault(serviceName, new List<ServiceStatus>())
            .OrderByDescending(status => status.TimeOfStatusUpdate)
            .Skip(parameters.Offset)
            .Take(parameters.Take).ToList();
}
