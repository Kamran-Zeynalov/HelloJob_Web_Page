using Microsoft.AspNetCore.Mvc;

namespace HelloJobBackEnd.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}
