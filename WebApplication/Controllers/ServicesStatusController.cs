using Microsoft.AspNetCore.Mvc;
using Services;
using ServiseEntities;

namespace WebApplication.Controllers;

[ApiController]
[Route("api/health/{service}")]
public class ServicesStatusController : ControllerBase
{
    private readonly IServiceStatusCollector _collector;

    public ServicesStatusController(IServiceStatusCollector collector) => _collector = collector;

    [HttpGet]
    public Health? GetServiceStatus(string service) => _collector.GetServiceStatus(service);

    [HttpPost]
    public void SetServiceStatus(string service, Health health)
        => _collector.ChangeServiceStatus(service, health);

    [Route("/api/health/history/{service}")]
    [HttpGet]
    public List<ServiceStatus> GetServiceStatusHistory(string service, [FromQuery]HistoryRequestParameters parameters)
        => _collector.GetServiceHistory(service, parameters);

    [Route("/api/health/history/{service}")]
    [HttpPost]
    public void SetServiceStatusHistory(string service, List<ServiceStatus> history)
        => _collector.AddServiceHistory(service, history);

    [Route("/api/health")]
    [HttpGet]
    public List<ServiceStatus> GetAllServices()
    {
        return _collector.GetServicesStatus();
    }
}
