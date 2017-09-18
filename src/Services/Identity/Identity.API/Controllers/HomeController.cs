using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Hello reminders-on-containers");
        }
    }
}