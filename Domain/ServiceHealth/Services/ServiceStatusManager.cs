using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class ServiceStatusManager:IServicesStatusManager
{
    private readonly Dictionary<string, Health> _services = new Dictionary<string, Health>();
    public void ChangeServiceStatus(string serviceName, Health health)
    {
        if (!_services.ContainsKey(serviceName))
            _services.Add(serviceName, health);
    }

    public Health GetServiceStatus(string serviceName)
    {
        return _services.ContainsKey(serviceName) ? _services[serviceName] : Health.degraded;
    }
}
