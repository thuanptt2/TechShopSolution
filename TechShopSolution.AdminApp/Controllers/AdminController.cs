using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.AdminApp.Service;
using TechShopSolution.ViewModels.System;

namespace TechShopSolution.AdminApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminApiClient _adminApiClient;
        public AdminController(IAdminApiClient adminApiClient)
        {
            _adminApiClient = adminApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);
            var token = await _adminApiClient.Authenticate(request);
            return View(token);
        }
    }
}
