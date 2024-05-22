using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBSV5TOT.Models;

namespace WEBSV5TOT.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly Sv5totContext db;
        public UserController(Sv5totContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var lstuser = db.Users.Include(x=>x.Role).ToList();
            return View(lstuser);
        }
        public IActionResult Create()
        {
            ViewBag.Role = db.Roles.OrderByDescending(x=>x.Id).ToList();
            ViewBag.Donvi = db.Parts.ToList();
            ViewBag.Lop = db.Classes.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return Json(new { status = true });
        }
        public IActionResult Edit(int id) {
            ViewBag.Role = db.Roles.OrderByDescending(x => x.Id).ToList();
            ViewBag.Donvi = db.Parts.ToList();
            ViewBag.Lop = db.Classes.ToList();
            var user = db.Users.SingleOrDefault(x=>x.Id== id);  
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
            var us = db.Users.SingleOrDefault(x=>x.Id == user.Id);
            us = user;
            db.SaveChanges();   
            return Json(new { status = true }); 
        }
        [HttpPost]
        public IActionResult Delete(int id) {
            var us = db.Users.SingleOrDefault(x => x.Id == id);
            db.Users.Remove(us);
            db.SaveChanges();
            return Json(new { status = true });
        }
    }
}
