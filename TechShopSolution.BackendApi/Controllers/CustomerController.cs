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
            var customer = await _customerService.GetAllPaging(requet);
            return Ok(customer);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _customerService.GetById(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateRequest request)
        {
            var result = await _customerService.Create(request);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CustomerUpdateRequest request)
        {
            var result = await _customerService.Update(request.Id, request);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpPut("UpdateAddress/{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] CustomerUpdateAddressRequest request)
        {
            var result = await _customerService.UpdateAddress(id, request);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            if (await _customerService.VerifyEmail(email))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.Delete(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
