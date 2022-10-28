using ServiceEntities;

namespace ServiseEntities;

public class ServiseStatus
{
    public string Name { get; set; } = "name";
    public Health Health { get; set; } = Health.Degraded;
}
