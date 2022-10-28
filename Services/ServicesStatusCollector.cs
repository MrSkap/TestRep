using ServiceEntities;

namespace Services;

public class ServicesStatusCollector
{
    private readonly Dictionary<string, Health> _services = new();

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
}
