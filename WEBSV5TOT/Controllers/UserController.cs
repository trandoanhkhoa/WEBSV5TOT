using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WEBSV5TOT.Models;
using WEBSV5TOT.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

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
        public async Task<IActionResult> Login(UserViewModel model, string ReturnUrl = "/Home/Index")
        {
            var user = db.Users.Include(x => x.Role)
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
            return View("Login");
        }

        [HttpPost]
        public IActionResult SendOTP(UserViewModel us)
        {
            var user = db.Users.SingleOrDefault(x => x.Mssv == us.MSSV);
            if(user != null)
            {
                int otp = SendMailLeave(user);
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false, });
            } 
                
        }
        public IActionResult ResetPassword(UserViewModel us)
        {
            var usr = db.Users.SingleOrDefault(x=>x.Mssv==us.MSSV);
            if(usr != null)
            {
                if (usr.Otp == us.otp)
                {
                    usr.Password = us.PasswordNew;
                    usr.Otp = "";
                    db.SaveChanges();
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });
                } 
                    
                
            }
            else
            {
                return Json(new { status = false});
            } 
                
        }
        public int SendMailLeave(User us)
        {
            Random rd = new Random();
            int otp = rd.Next(100000, 999999);
            var user = db.Users.SingleOrDefault(x => x.Mssv == us.Mssv);
            if(user != null)
            {
                user.Otp = otp.ToString();
                db.SaveChanges();
            }
            string Mailbody = $@"<div style=""font-family: Helvetica,Arial,sans-serif;min-                                  width:1000px;overflow:auto;line-height:2"">
                           <div style=""margin:30px auto;width:70%;padding:10px 0"">
    <div style=""border-bottom:1px solid #eee"">
      <div style=""display:flex; align-items:center"">
          <img src=""https://lib.hcmue.edu.vn/sites/default/files/Logo%20HCMUP.png""  style=""width:130px""/>
          <img src=""https://youth.ueh.edu.vn/wp-content/uploads/2022/10/Thap-1.png""  style=""width:150px""/>
        </div>
    </div>
    <p style=""font-size:1.1em"">Gửi đến {us.FullName}</p>
    <p>Đây là mã OTP thiết lập mật khẩu, vui lòng không chia sẽ mã OTP cho bất kì ai !. OTP sẽ có hiệu lực trong 1 phút</p>
    <h2 style=""background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;"">{otp}</h2>
    <p style=""font-size:0.9em;"">Gửi từ,<br />Hội sinh viên./</p>
    <hr style=""border:none;border-top:1px solid #eee"" />
    <div style=""float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300"">
      <p>Hội sinh viên trường Đại học Sư Phạm TP.HCM</p>
      <p>Trường Đại Học Sư Phạm TP.HCM</p>
    </div>
  </div>
</div>";


            string subject = "MÃ OTP THIẾT LẬP TÀI KHOẢN " + us.Mssv;
            string mailTitle = "SINH VIÊN 5 TỐT - MÃ OTP THIẾT LẬP TÀI KHOẢN";
            string fromEmail = "trankhoa192837@gmail.com";
            string fromEmailPassword = "nnpbsdgxqcrdxcyj";

            //Email Content
            MailMessage mailMessage = new MailMessage(new MailAddress(fromEmail, mailTitle), new MailAddress(us.Email));
            mailMessage.Subject = subject;
            mailMessage.Body = Mailbody;
            mailMessage.IsBodyHtml = true;

            //Server Details
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            //Credentials
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
            credential.UserName = fromEmail;
            credential.Password = fromEmailPassword;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;
            smtp.Send(mailMessage);
            return otp;
        }
        public IActionResult InforUser()
        {
            int UserID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = db.Users.Include(x=>x.Part).SingleOrDefault(x=>x.Id==UserID);
            ViewBag.Part = db.Parts.ToList();
            ViewBag.Class = db.Classes.ToList();
            return View(user);
        }    
    }
}
