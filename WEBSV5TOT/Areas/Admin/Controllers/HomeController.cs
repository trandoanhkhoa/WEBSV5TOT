using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEBSV5TOT.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin, banthuky, canbohoi")]

    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
