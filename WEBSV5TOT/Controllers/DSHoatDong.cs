using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IO;
using WEBSV5TOT.Models;

namespace WEBSV5TOT.Controllers
{
    public class DSHoatDong : Controller
    {
        private readonly Sv5totContext db;
        private readonly IWebHostEnvironment _environment;
        public DSHoatDong(Sv5totContext db, IWebHostEnvironment environment)
        {
            this.db = db;
            this._environment = environment;
        }
        public IActionResult Index()
        {
            int curruserid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var lst = db.Student5Goods.Where(x => x.UserId == curruserid).Include(x => x.Activity).ToList();
            var criteriasQuerry = db.Student5Goods
                                  .Where(s5g => s5g.UserId == curruserid)
                                  .Where(s5g => s5g.Approval == true)
                                  .Join(db.Activities,
                                        s5g => s5g.ActivityId,
                                        act => act.Id,
                                        (s5g, act) => new { act.Type })
                                  .GroupBy(x => x.Type)
                                  .Select(g => g.Key);
            ViewBag.CriteriasSucceed = criteriasQuerry.ToList();
            ViewBag.Criterias = new List<string> { "Đạo đức tốt", "Học tập tốt", "Thể lực tốt", "Tình nguyện tốt", "Hội nhập tốt" };
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
                var act = db.Activities.SingleOrDefault(x=>x.Id== idActivity);
                if(act != null) act.Quantity += 1;
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
        [HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, int Student5GoodId)
        {
            var s5g = db.Student5Goods.Where(x => x.Id == Student5GoodId).Single();
            var userId = s5g.UserId;
            var activityId = s5g.ActivityId;
            string uploadPath = Path.Combine(_environment.ContentRootPath, "Proofs");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            int index = 0;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    string newFileName = $"file{index}_{userId}_{activityId}_{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";
                    string filePath = Path.Combine(uploadPath, newFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Lưu thông tin file vào cơ sở dữ liệu nếu cần thiết
                    var proof = new ProofPicture
                    {
                        FileName = newFileName,
                        InputDate = DateTime.Now,
                        Student5GoodId = Student5GoodId
                    };
                    db.ProofPictures.Add(proof);
                }
                index++;
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GetImages(int id)
        {
            var lst = db.ProofPictures.Where(x => x.Student5GoodId == id).Select(x => x.FileName).ToList();
            if (lst.Count > 0)
            {
                // Trả về hình ảnh
                return Json(new { status = true, list = lst }); ; // Thay đổi "image/jpeg" thành kiểu MIME của hình ảnh của bạn
            }
            else
            {
                return Json(new { status = false });
            }
        }
    }
    
}


