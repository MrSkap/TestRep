using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ServiseEntities;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Health
{
    Healthy,
    Unhealthy,
    Degraded,
}
