using Microsoft.AspNetCore.Mvc;

namespace IndieVault.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        [Route("/Error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}
