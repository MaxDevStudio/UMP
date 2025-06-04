using Microsoft.AspNetCore.Mvc;

namespace UMP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetItems()
        {
            var items = new[] { "App1", "App2", "App3" };
            return Ok(items);
        }
    }
}
