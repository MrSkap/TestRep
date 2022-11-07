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
    public ActionResult SetServiceStatus(string service, Health health)
    {
        _collector.ChangeServiceStatus(service, health);
        return Ok(1);
    }

    [Route("/api/health/history/{service}")]
    [HttpGet]
    public List<ServiceStatus> GetServiceStatusHistory(string service) => _collector.GetServiceHistory(service);

    [Route("/api/health/history/{service}")]
    [HttpPost]
    public ActionResult SerServiceStatusHistory(string service, List<ServiceStatus> history)
    {
        _collector.AddServiceHistory(service, history);
        return Ok(history);
    }
}
