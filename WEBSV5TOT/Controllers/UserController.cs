using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WEBSV5TOT.Models;
using WEBSV5TOT.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace WEBSV5TOT.Controllers
{
    public class UserController : Controller
    {
        private readonly Sv5totContext db;
        public UserController(Sv5totContext db)
        {
            this.db = db;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel model, string ReturnUrl = "")
        {
            var user = db.Users.Include(x=>x.Role)
                .SingleOrDefault(x => x.Username == model.UserName && x.Password == model.Password);
            if (user == null) // Tài khoản không tồn tại
            {
                return Json(new { status = false, mess = "Username or password not correct" });
            }
            else
            {
                //Phân quyền cookies cho người dùng 

                var claims = new List<Claim>();
                claims?.Add(new Claim(ClaimTypes.Name, user.Username));
                claims?.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                if (user.Role.Id == 1) // Admin // KTT
                {
                    claims?.Add(new Claim(ClaimTypes.Role, "admin"));
                }
                else if (user.Role.Id == 2) // Normal
                {
                    claims?.Add(new Claim(ClaimTypes.Role, "banthuky"));
                }
                else if (user.Role.Id == 3) /// supplier
                {
                    claims?.Add(new Claim(ClaimTypes.Role, "canbohoi"));
                }
                else if (user.Role.Id == 4) /// supplier
                {
                    claims?.Add(new Claim(ClaimTypes.Role, "sinhvien"));
                }
                else
                {

                    return Json(new { status = false, mess = "Not Authorize" });
                }
                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal);
               
                return Json(new { status = true, mess = "", url = ReturnUrl });
            }

        }

        public async Task<IActionResult> Logout()
        { 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("Loggin");
        }

    }
}
