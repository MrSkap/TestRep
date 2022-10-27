using Domain.Entities;
namespace Domain.Interfaces;

public interface IServicesStatusManager
{
    public void ChangeServiceStatus(string serviceName, Health health);
    public Health GetServiceStatus(string serviceName);
}
