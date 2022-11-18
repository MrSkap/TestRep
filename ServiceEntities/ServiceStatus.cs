using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiseEntities;
public record ServiceStatus
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
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
