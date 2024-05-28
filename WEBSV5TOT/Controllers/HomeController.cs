using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using WEBSV5TOT.Models;
using WEBSV5TOT.ViewModel;

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
            var lst = db.Activities.OrderByDescending(x=>x.Id).ToList();
            return View(lst);
        }
        public IActionResult DSSV5TOT()
        {
            ViewBag.Donvi = db.Parts.ToList();
            ViewBag.Lop = db.Classes.ToList();
            var result = db.Student5Goods
            .Join(db.Users,              // Join với bảng Users
                s5t => s5t.UserId,
                us => us.Id,
                (s5t, us) => new { s5t, us })
            .Where(x=>x.s5t.Approval==true)
            .Join(db.Activities,         // Join với bảng Activities
                combined => combined.s5t.ActivityId,
                act => act.Id,
            (combined, act) => new { combined.us, act.Type })
            
            .GroupBy(x => x.us.Id)            // Nhóm theo UserID
            .Select(group => new              // Chọn và đếm các loại hoạt động duy nhất
            {
                UserID = group.Key,
                TotalUniqueActivities = group.Select(x => x.Type).Distinct().Count()
            })
            .Where(x => x.TotalUniqueActivities >= 5) // Lọc ra các nhóm có ít nhất 5 loại hoạt động khác nhau
            .Join(db.Users,              // Join kết quả với bảng Users để lấy thông tin người dùng
                tmp => tmp.UserID,
                us => us.Id,
                (tmp, us) => us)  // Chỉ trả về đối tượng us thay vì anonymous type
            .ToList();  // Thực thi truy vấn và lấy kết quả dưới dạng list

            return View(result);

        }
        [HttpPost]
        public IActionResult SearchSV5TOT(SearchSV5totViewModel model)
        {
            var ds = db.Student5Goods.Include(x=>x.User).Include(x=>x.User.Part).ToList();
            ViewBag.Part = db.Parts.ToList();
            if(!string.IsNullOrEmpty( model.MSSV))
            {
                ds=db.Student5Goods.Where(x=>x.User.Mssv==model.MSSV).ToList();
            }
            if(!string.IsNullOrEmpty(model.Cap))
            {
                ds=db.Student5Goods.Where(x => x.Level==model.Cap).ToList();
            }
            if(model.Donvi!=null && model.Donvi!=0)
            {
                ds = db.Student5Goods.Where(x => x.User.Part.Id==model.Donvi).ToList();
            }
            if(model.Lop!=null && model.Lop!=0)
            {
                ds = db.Student5Goods.Where(x => x.User.Class.Id == model.Lop).ToList();
            }
            return PartialView("~/Views/Home/DS5TOT.cshtml",ds);
        }

    }
}