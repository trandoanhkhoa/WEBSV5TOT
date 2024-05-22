using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using WEBSV5TOT.Models;

namespace WEBSV5TOT.Controllers
{
    [Authorize(Roles = "admin,banthuky,canbohoi,sinhvien")]
    public class HomeController : Controller
    {
        private readonly Sv5totContext db;

        public HomeController(Sv5totContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var lst = db.Activities.ToList();
            return View(lst);
        }
        public IActionResult DSSV5TOT()
        {
            ViewBag.Donvi = db.Parts.ToList();
            return View();
        }

    }
}