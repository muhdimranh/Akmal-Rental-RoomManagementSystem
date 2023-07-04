using Microsoft.AspNetCore.Mvc;

namespace Viho.Controllers
{
    public class SamplePageController : Controller
    {
        public IActionResult SamplePage()
        {
            return View();
        }
    }
}
