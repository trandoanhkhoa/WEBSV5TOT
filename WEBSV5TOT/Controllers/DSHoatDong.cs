using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
            int tmp = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var lst = db.Student5Goods.Where(x => x.UserId == tmp).Include(x => x.Activity).ToList();
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
        [HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, int Student5GoodId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
                    string newFileName = $"file_{Student5GoodId}_{DateTime.Now:yyyyMMddHHmmssfff}_{index}{fileExtension}";
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
            return RedirectToAction(nameof(Index));
        }
    }
}


