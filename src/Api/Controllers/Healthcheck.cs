using Microsoft.AspNetCore.Mvc;
using Application;

namespace SiteWatchman.Controllers
{
    [Route("api/[controller]")]
    public class HealthcheckController : ControllerBase
    {
       [HttpGet]
       public IActionResult Get()
       {
           var factory = new Healthcheck();
           return new ObjectResult(factory.GetMe());
       }
    }
}
