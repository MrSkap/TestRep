using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ServiseEntities;
[Serializable]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Health
{
    Healthy,
    Unhealthy,
    Degraded,
}
