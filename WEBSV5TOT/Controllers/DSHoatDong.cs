using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WEBSV5TOT.Models;

namespace WEBSV5TOT.Controllers
{
    public class DSHoatDong : Controller
    {
        private readonly Sv5totContext db;
        public DSHoatDong(Sv5totContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var lst = db.Student5Goods.Include(x => x.Activity).ToList();
            return View(lst);
        }
        public IActionResult Create(int idActivity)
        {
            int tmp = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var hd = db.Student5Goods.SingleOrDefault(x => x.ActivityId == idActivity && x.UserId == tmp);
            //Sinh vien da dang ki hoat dong này
            if (hd != null)
            {
                return Json(new { status = false });
            }
            else
            {
                var actiadd = new Student5Good();
                actiadd.ActivityId = idActivity;
                actiadd.UserId = tmp;
                actiadd.Approval = false;
                actiadd.Year = DateTime.Now;
                db.Student5Goods.Add(actiadd);
                db.SaveChanges();
                return Json(new { status = true });
            } 
                
        }
    }
}
