using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Catalog.Customer;
using TechShopSolution.ViewModels.Catalog.Customer;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetCustomerPagingRequest requet)
        {
            var products = await _customerService.GetAllPaging(requet);
            return Ok(products);
        }
        [HttpPost("them-khach-hang")]
        public async Task<IActionResult> Create([FromBody] CustomerCreateRequest request)
        {
            var customer = await _customerService.Create(request);
            if (customer == false)
                return BadRequest();
            return Ok(customer);
        }
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            if( await _customerService.VerifyEmail(email))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
