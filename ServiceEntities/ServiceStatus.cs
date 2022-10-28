using ServiceEntities;

namespace ServiseEntities;

public record ServiceStatus
{
    public string Name { get; init; } = "name";
    public Health Health { get; set; } = Health.Degraded;

    public ServiceStatus(string name, Health health)
    {
        Name = name;
        Health = health;
    }
}
