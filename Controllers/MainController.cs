using Microsoft.AspNetCore.Mvc;
namespace Controllers;
public class MainController: Controller
{
    private Dictionary<string, Health> services = new Dictionary<string, Health>();
    [HttpGet("api/health/{service}")]
    public IActionResult GetServiceHealth(Health health)
    {
        return Ok();
    }

    [HttpPost("api/health/{service}")]
    public IActionResult PostServiceHealth(Health health)
    {
        return Ok();
    }
}
