using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Catalog.Customer;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Catalog.Product;

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
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterRequest request)
        {
            var result = await _customerService.Register(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CustomerUpdateRequest request)
        {
            var result = await _customerService.Update(request.Id, request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("public/{id}")]
        public async Task<IActionResult> UpdatePublic([FromBody] CustomerPublicUpdateRequest request)
        {
            var result = await _customerService.UpdatePublic(request.Id, request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("UpdateAddress/{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] CustomerUpdateAddressRequest request)
        {
            var result = await _customerService.UpdateAddress(id, request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _customerService.ChangeStatus(id);
            if (!result.IsSuccess)
                return BadRequest(result);
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
        [HttpGet("orderlatest")]
        public IActionResult GetLatestOrder(int id, int take)
        {
            var result = _customerService.GetLatestOrder(id, take);
            return Ok(result);
        }
        [HttpPost("rating")]
        public async Task<IActionResult> RatingProduct(ProductRatingRequest request)
        {
            var result = await _customerService.RatingProduct(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("favorite")]
        public async Task<IActionResult> FavoriteProduct(int cus_id, int product_id)
        {
            var result = await _customerService.FavoriteProduct(cus_id, product_id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("unfavorite")]
        public async Task<IActionResult> UnFavoriteProduct(int cus_id, int product_id)
        {
            var result = await _customerService.UnFavoriteProduct(cus_id, product_id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("favoriteproducts")]
        public IActionResult GetPublicProducts(GetFavoriteProductsPagingRequest request)
        {
            var products = _customerService.GetFavoriteProduct(request);
            if (products.Items == null)
                return BadRequest("Không có sản phẩm nào");
            return Ok(products);
        }
    }
}
