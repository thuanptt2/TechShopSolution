using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.ViewModels.Sales;
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
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(
                          CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        [Route("dang-nhap")]
        public async Task<IActionResult> Login(LoginRequest request, string ReturnUrl)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _adminApiClient.AuthenticateCustomer(request);
            if (result.IsSuccess)
            {
                var adminPrincipal = this.ValidateToken(result.ResultObject);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(180),
                    IsPersistent = false
                };
                HttpContext.Session.SetString("Token", result.ResultObject);
                await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            adminPrincipal,
                            authProperties);
                if(!string.IsNullOrEmpty(ReturnUrl))
                {
                    if(ReturnUrl.Equals("/dang-nhap"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(ReturnUrl);
                } else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [Route("dang-ky")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("dang-ky")]
        public async Task<IActionResult> Register(CustomerRegisterRequest request, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _customerApiClient.Register(request);
            if (result.IsSuccess)
            {
                var token = await _adminApiClient.AuthenticateCustomer(new LoginRequest { Email = request.email, Password = request.password, Remeber_me = true });

                var adminPrincipal = this.ValidateToken(token.ResultObject);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = false
                };
                HttpContext.Session.SetString("Token", token.ResultObject);
                await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            adminPrincipal,
                            authProperties);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");

            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [Authorize]
        [Route("tai-khoan")]
        public async Task<IActionResult> Detail()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }
            var id = User.FindFirst(ClaimTypes.Sid).Value;

            var result = await _customerApiClient.GetById(int.Parse(id));
            if (!result.IsSuccess || result.ResultObject == null)
            {
                ModelState.AddModelError("", result.Message);
                return RedirectToAction("Index", "Home");
            }
            var updateRequest = new CustomerPublicUpdateRequest()
            {
                Id = int.Parse(id),
                name = result.ResultObject.name,
                birthday = result.ResultObject.birthday,
                address = result.ResultObject.address,
                email = result.ResultObject.email,
                sex = result.ResultObject.sex,
                phone = result.ResultObject.phone,
            };
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(updateRequest);
        }
        [HttpPost]
        [Route("tai-khoan")]
        [Authorize]
        public async Task<IActionResult> Detail(CustomerPublicUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request); 
            }
            var result = await _customerApiClient.UpdateCustomerPublic(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật tài khoản thành công";
                return RedirectToAction("Detail","Account");
            }
            ModelState.AddModelError("", "Cập nhật thất bại");
            return View(request);
        }
        [Authorize]
        public async Task<IActionResult> UpdateAddress(int id)
        {
            var result = await _customerApiClient.GetById(id);
            if (!result.IsSuccess || result.ResultObject == null)
                ModelState.AddModelError("", result.Message);
            var updateAddressRequest = new CustomerUpdateAddressRequest()
            {
                Id = id,
                City = null,
                District = null,
                House = null,
                Ward = null
            };
            return View(updateAddressRequest);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateAddress(CustomerUpdateAddressRequest request)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Detail));
            var result = await _customerApiClient.UpdateAddress(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật địa chỉ thành công";
                return RedirectToAction(nameof(Detail));
            }
            TempData["result"] = "Cập nhật địa chỉ thất bại";
            return RedirectToAction(nameof(Detail));
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(string email, int Id)
        {
            if (await _customerApiClient.VerifyEmail(email) == false)
            {
                return Json($"Email {email} đã được sử dụng.");
            }
            return Json(true);
        }
        public async Task<JsonResult> LoadProvince()
        {
            try
            {
                var result = await _customerApiClient.LoadProvince();
                if (result == null || !result.IsSuccess)
                {
                    return null;
                }
                return Json(new
                {
                    data = result.ResultObject,
                    status = true
                });
            }
            catch
            {
                return null;
            }
        }
        public async Task<JsonResult> LoadDistrict(int provinceID)
        {
            try
            {
                var result = await _customerApiClient.LoadDistrict(provinceID);
                if (result == null || !result.IsSuccess)
                {
                    return null;
                }
                return Json(new
                {
                    data = result.ResultObject,
                    status = true
                });
            }
            catch
            {
                return null;
            }
        }
        public async Task<JsonResult> LoadWard(int districtID)
        {
            try
            {
                var result = await _customerApiClient.LoadWard(districtID);
                if (result == null || !result.IsSuccess)
                {
                    return null;
                }
                return Json(new
                {
                    data = result.ResultObject,
                    status = true
                });
            }
            catch
            {
                return null;
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
        [Authorize]
        [Route("tai-khoan/san-pham-yeu-thich")]
        [HttpGet]
        public async Task<IActionResult> FavoriteProducts(int pageIndex = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }
            var id = User.FindFirst(ClaimTypes.Sid).Value;
            var products = await _customerApiClient.GetFavoriteProducts(new GetFavoriteProductsPagingRequest()
            {
                cus_id = int.Parse(id),
                PageIndex = pageIndex,
                PageSize = 8,
            });
            ViewBag.PageResult = products;
            return View(products);
        }
     
    }
}
