using Microsoft.AspNetCore.Mvc;

namespace WebBlog.API.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
