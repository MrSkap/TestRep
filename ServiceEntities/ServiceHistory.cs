using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
namespace ServiseEntities;

public class ServiceHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public List<ServiceStatus> History { get; set; }
}
