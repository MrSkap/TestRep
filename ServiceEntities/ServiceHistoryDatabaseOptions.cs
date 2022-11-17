namespace ServiseEntities;

public class ServiceHistoryDatabaseOptions
{
    public string ConnctionString { get; set; } = null;
    public string DatabaseName { get; set; } = null;
    public string AllServicesHistoryCollectionName { get; set; } = null;
    public string LastServicesStatusCollectionName { get; set; } = null;
}
