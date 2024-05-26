using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using WEBSV5TOT.Models;

namespace WEBSV5TOT.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,banthuky")]
    [Area("Admin")]
    public class ActivitiesController : Controller
    {
        private readonly Sv5totContext db;
        public ActivitiesController(Sv5totContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var lst = db.Activities.ToList();
            return View(lst);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Models.Activity at)
        {
            db.Activities.Add(at);
            db.SaveChanges();
            return Json(new { status = true });
        }
        public IActionResult Edit(int id)
        {
            var act = db.Activities.SingleOrDefault(x => x.Id == id);
            return View(act);
        }
        [HttpPost]
        public IActionResult Edit(Models.Activity activity)
        {
            var act = db.Activities.SingleOrDefault(x=>x.Id==activity.Id);
            act = activity;
            db.SaveChanges();
            return Json(new { status = true });
        }
        public IActionResult Delete(int id)
        {
            var act = db.Activities.SingleOrDefault(x => x.Id ==id);
            db.Activities.Remove(act);
            db.SaveChanges();
            return Json(new { status = true });
        }
    }
}
