﻿using Microsoft.AspNetCore.Authentication;
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
using TechShopSolution.ViewModels.System;

namespace TechShopSolution.WebApp.Controllers
{
    [Authorize]
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
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [Route("dang-ky")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky")]
        public async Task<IActionResult> Register(CustomerRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _customerApiClient.Register(request);
            if (result.IsSuccess)
            {
                var token = await _adminApiClient.AuthenticateCustomer(new LoginRequest { Email = request.email, Password = request.password, Remeber_me = true });

                var adminPrincipal = this.ValidateToken(token.Message);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = false
                };
                HttpContext.Session.SetString("Token", token.Message);
                await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            adminPrincipal,
                            authProperties);
                return RedirectToAction("Index", "Home");

            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [Route("tai-khoan")]
        public async Task<IActionResult> Detail(string id)
        {
            int ID = int.Parse(id);
            var result = await _customerApiClient.GetById(ID);
            if (!result.IsSuccess || result.ResultObject == null)
            {
                ModelState.AddModelError("", result.Message);
                return RedirectToAction("Index", "Home");
            }
            var updateRequest = new CustomerPublicUpdateRequest()
            {
                Id = ID,
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
                return RedirectToAction("Detail","Account", new { id = request.Id.ToString()});
            }
            ModelState.AddModelError("", "Cập nhật thất bại");
            return View(request);
        }
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
        [HttpPost]
        public async Task<IActionResult> UpdateAddress(CustomerUpdateAddressRequest request)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Detail), new { id = request.Id.ToString() });
            var result = await _customerApiClient.UpdateAddress(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật địa chỉ thành công";
                return RedirectToAction(nameof(Detail), new { id = request.Id.ToString() });
            }
            TempData["result"] = "Cập nhật địa chỉ thất bại";
            return RedirectToAction(nameof(Detail), new { id = request.Id.ToString() });
        }
        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
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
                HttpContext.Session.SetString("Tokenuser", result.ResultObject);
                await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            adminPrincipal,
                            authProperties);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);

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
    }
}