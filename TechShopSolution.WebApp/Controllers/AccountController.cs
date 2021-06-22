using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.System;

namespace TechShopSolution.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAdminApiClient _adminApiClient;
        private readonly ICustomerApiClient _customerApiClient;
        private readonly IConfiguration _configuration;
        public AccountController(IAdminApiClient adminApiClient, IConfiguration configuration, ICustomerApiClient customerApiClient)
        {
            _adminApiClient = adminApiClient;
            _configuration = configuration;
            _customerApiClient = customerApiClient;
        }
        [Route("dang-nhap")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(CustomerRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _customerApiClient.Register(request);
            if (result.IsSuccess)
            {
                await Login(new LoginRequest { Email = request.email, Password = request.password, Remeber_me = true });

            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        //[HttpPost]
        //public IActionResult Register()
        //{
        //    return View();
        //}
 
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(string email, int Id)
        {
            if (await _customerApiClient.VerifyEmail(email) == false)
            {
                return Json($"Email {email} đã được sử dụng.");
            }
            return Json(true);
        }
        [HttpPost]
        [Route("dang-nhap")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(request);
                var token = await _adminApiClient.AuthenticateCustomer(request);

                var adminPrincipal = this.ValidateToken(token);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = false
                };
                HttpContext.Session.SetString("Token", token);
                await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            adminPrincipal,
                            authProperties);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("", "Sai thông tin đăng nhập");
                return View(request);
            }
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                          CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
