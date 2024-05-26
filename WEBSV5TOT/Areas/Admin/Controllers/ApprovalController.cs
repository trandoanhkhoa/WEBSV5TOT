using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WEBSV5TOT.Models;
using WEBSV5TOT.ViewModel;

namespace WEBSV5TOT.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,banthuky")]
    [Area("Admin")]
    public class ApprovalController : Controller
    {
        private readonly Sv5totContext db;
        private readonly IWebHostEnvironment _environment;
        public ApprovalController(Sv5totContext db, IWebHostEnvironment environment)
        {
            this.db = db;
            _environment = environment; 
        }
        public IActionResult Index()
        {
            var lst = db.Student5Goods
                .Where(student => student.ProofPictures.Any(picture => picture.Student5GoodId == student.Id))
                .ToList(); ;
            ViewBag.ProofPictures = db.ProofPictures.ToList();
            ViewBag.Users = db.Users.ToList();
            ViewBag.Activities = db.Activities.ToList();
            return View(lst);
        }

        public IActionResult _ImageList(int id)
        {
            List<ProofPicture> images = db.ProofPictures.Where(p => p.Student5GoodId == id).ToList();

            return PartialView("_ImageList", images);
        }
        [HttpGet]
        public ActionResult GetImage(string fileName)
        {
            // Đường dẫn tệp hình ảnh
            string imagePath = Path.Combine(_environment.ContentRootPath, "Proofs", fileName);

            // Kiểm tra xem tệp có tồn tại không
            if (System.IO.File.Exists(imagePath))
            {
                // Trả về hình ảnh
                return File(imagePath, "image/jpeg"); // Thay đổi "image/jpeg" thành kiểu MIME của hình ảnh của bạn
            }
            else
            {
                return Json(new { status = false });
            }
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var sv5t = db.Student5Goods.Where(x => x.Id == id).SingleOrDefault();
            if(sv5t != null)
            {
                sv5t.Approval = true;
                db.SaveChanges();
            }
                
            return Json(new { status = true });
        }
        [HttpPost]
        public IActionResult DisApprove(int id)
        {
            var sv5t = db.Student5Goods.Where(x => x.Id == id).SingleOrDefault();
            if (sv5t != null)
            {
                sv5t.Approval = false;
                db.SaveChanges();
            }

            return Json(new { status = true });
        }
    }
}
