using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.System;

namespace TechShopSolution.WebApp.Controllers
{
    public class AccountController : Controller
    {
       
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginRequest request)
        //{
        //    //try
        //    //{
        //    //    if (!ModelState.IsValid)
        //    //        return View(request);
        //    //    var token = await _adminApiClient.Authenticate(request);

        //    //    var adminPrincipal = this.ValidateToken(token);
        //    //    var authProperties = new AuthenticationProperties
        //    //    {
        //    //        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
        //    //        IsPersistent = false
        //    //    };
        //    //    HttpContext.Session.SetString("Token", token);
        //    //    await HttpContext.SignInAsync(
        //    //                CookieAuthenticationDefaults.AuthenticationScheme,
        //    //                adminPrincipal,
        //    //                authProperties);

        //    //    return RedirectToAction("Index", "Home");
        //    //}
        //    //catch
        //    //{
        //    //    ModelState.AddModelError("", "Sai thông tin đăng nhập");
        //    //    return View(request);
        //    //}
        //}
    }
}
