using System.Text.Json.Serialization;

namespace ServiseEntities;
[Serializable]
public record ServiceStatus
{
    public string Name { get; init; } = "name";
    public Health Health { get; set; } = Health.Degraded;
    public DateTimeOffset TimeOfStatusUpdate{ get; set; }

    public ServiceStatus(string name, Health health)
    {
        Name = name;
        Health = health;
        TimeOfStatusUpdate = DateTimeOffset.Now;
    }

    public ServiceStatus(string name, Health health, DateTimeOffset time)
    {
        Name = name;
        Health = health;
        TimeOfStatusUpdate = time;
    }
    public ServiceStatus(){}
}
