using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Catalog.Customer;
using TechShopSolution.ViewModels.Catalog.Customer;

namespace TechShopSolution.BackendApi.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery]GetCustomerPagingRequest requet)
        {
            var products = await _customerService.GetAllPaging(requet);
            return Ok(products);
        }
        [HttpPost("Add-customer")]
        public async Task<IActionResult> Create([FromForm] CustomerCreateRequest request)
        {
            var customer = await _customerService.Create(request);
            if (customer==false)
                return BadRequest();
            return Ok();
        }
    }
}
