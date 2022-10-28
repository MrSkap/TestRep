using ServiceEntities;

namespace ServiseEntities;

public class ServiceStatus
{
    public string Name { get; set; } = "name";
    public Health Health { get; set; } = Health.Degraded;

    public ServiceStatus(string name, Health health)
    {
        Name = name;
        Health = health;
    }
}
