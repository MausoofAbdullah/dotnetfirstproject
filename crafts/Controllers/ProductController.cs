using Microsoft.AspNetCore.Mvc;

namespace crafts.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
