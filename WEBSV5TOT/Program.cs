using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WEBSV5TOT.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Build code runtime
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//Connect with db
builder.Services.AddDbContext<Sv5totContext>(options =>
{
    options.UseSqlServer("Data Source=MSI;Initial Catalog=SV5TOT;Integrated Security=True;TrustServerCertificate=True;");
});
//authorize user
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
    option =>
    {
        option.LoginPath = "/User/Login";
        option.AccessDeniedPath = "/";
    });
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseSession();
app.UseAuthorization();



//Vinh add cho phan DownloadFile
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=User}/{action=Login}/{id?}");

    endpoints.MapControllerRoute(
            name: "showImages",
            pattern: "admin/Approval/_ImageList/{id}",
            defaults: new { controller = "Approval", action = "_ImageList" });


});


app.Run();
