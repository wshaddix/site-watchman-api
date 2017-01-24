using Microsoft.AspNetCore.Mvc;
using ClassLibrary;

namespace SiteWatchman.Controllers
{
    [Route("api/[controller]")]
    public class HealthcheckController : ControllerBase
    {
       [HttpGet]
       public IActionResult Get()
       {
           var factory = new Class1();
           return new ObjectResult(factory.GetMe());
       }
    }
}
