namespace ServiseEntities;

public record ServiceStatusRequestParameters
{
    public int Take { get; set; } = 10;
    public int Offset { get; set; } = 10;
}
