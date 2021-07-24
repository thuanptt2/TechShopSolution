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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.System;
using System.Security.Cryptography;

namespace TechShopSolution.AdminApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminApiClient _adminApiClient;
        private readonly IConfiguration _configuration;
        public AdminController(IAdminApiClient adminApiClient, IConfiguration configuration)
        {
            _adminApiClient = adminApiClient;
            _configuration = configuration;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(request);
               
                var token = await _adminApiClient.Authenticate(request);

                var adminPrincipal = this.ValidateToken(token);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
                    IsPersistent = false
                };
                HttpContext.Session.SetString("Token", token);
                await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            adminPrincipal,
                            authProperties);

                if(!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

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
    }
    public class HashSaltWithRounds
    {
        int saltLength = 32;
        public byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        public string HashDataWithRounds(byte[] password, byte[] salt, int rounds)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, rounds))
            {
                return Convert.ToBase64String(rfc2898.GetBytes(32));
            }
        }
    }
}
