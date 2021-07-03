using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Catalog.PaymentMethod;
using TechShopSolution.ViewModels.Catalog.PaymentMethod;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentService;

        public PaymentController(IPaymentMethodService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPaymentPagingRequest requet)
        {
            var payment = await _paymentService.GetAllPaging(requet);
            return Ok(payment);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var payment = await _paymentService.GetAll();
            return Ok(payment);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _paymentService.GetById(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentCreateRequest request)
        {
            var result = await _paymentService.Create(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PaymentUpdateRequest request)
        {
            var result = await _paymentService.Update(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _paymentService.ChangeStatus(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _paymentService.Delete(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
