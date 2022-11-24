using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services;
using ServiseEntities;

namespace WebApplication.Controllers;

[ApiController]
[Route("api/health/{service}")]
public class ServicesStatusController : ControllerBase
{
    private readonly IServiceHistoryCollector _collector;

    public ServicesStatusController(IServiceHistoryCollector collector) => _collector = collector;

    [HttpGet]
    public async Task<Health?> GetServiceStatus(string service) => (await _collector.GetServiceStatus(service))?.Health;

    [HttpPost]
    public async Task SetServiceStatus(string service, Health health, DateTimeOffset time)
        => await _collector.SetOrAddServiceStatus(new ServiceStatus(service, health, time));

    [Route("/api/health/history/{service}")]
    [HttpGet]
    public async Task<List<ServiceStatus>?> GetServiceStatusHistory(string service, [FromQuery] HistoryRequestParameters parameters)
        => await _collector.GetServiceHistory(service, parameters);

    [Route("/api/health/history/{service}")]
    [HttpPost]
    public async Task SetServiceStatusHistory(string service, List<ServiceStatus> history)
        => await _collector.SetOrAddServiceHistory(history);

    [Route("/api/health")]
    [HttpGet]
    public async Task<List<ServiceStatus>?> GetAllServices()
        => await _collector.GetLastServicesStatus();
}
