using Microsoft.AspNetCore.Mvc;
using ServiceEntities;
using Services;

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
    public ActionResult CreateService(string service, Health health)
    {
        _collector.ChangeServiceStatus(service, health);
        return Ok();
    }
}
