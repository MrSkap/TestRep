namespace ServiseEntities;

public class ServiceHistoryDatabaseOptions
{
    public const string ConfigurationKey = "ServiceHistoryDatabase";
    public string ConnectionString { get; set; } = null;
    public string DatabaseName { get; set; } = null;
}
