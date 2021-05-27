using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.AdminApp.Service;
using TechShopSolution.ViewModels.Catalog.Customer;

namespace TechShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerApiClient _customerApiClient;
        public CustomerController(ICustomerApiClient customerApiClient)
        {
            _customerApiClient = customerApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetCustomerPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _customerApiClient.GetCustomerPagings(request);
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CustomerCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);
            var result = await _customerApiClient.CreateCustomer(request);
            if(result)
            {
                return RedirectToAction("Index");
            }
            return View(Request);
        }
    }
}
