namespace ServiseEntities;

public record HistoryRequestParameters
{
    public int Take { get; init; } = 10;
    public int Offset { get; init; } = 10;
}
