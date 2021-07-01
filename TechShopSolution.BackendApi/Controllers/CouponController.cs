using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Catalog.Coupon;
using TechShopSolution.ViewModels.Catalog.Coupon;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetCouponPagingRequest requet)
        {
            var customer = await _couponService.GetAllPaging(requet);
            return Ok(customer);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _couponService.GetById(id);
            return Ok(result);
        }
        [HttpGet("code")]
        public IActionResult GetByCode(string code)
        {
            var result = _couponService.GetByCode(code);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CouponCreateRequest request)
        {
            var result = await _couponService.Create(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CouponUpdateRequest request)
        {
            var result = await _couponService.Update(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _couponService.ChangeStatus(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _couponService.Delete(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> isValidCode(int id, string code)
        {
            if (await _couponService.isValidCode(id, code))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
