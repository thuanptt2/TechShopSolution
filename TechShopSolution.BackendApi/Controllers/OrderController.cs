using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Catalog.Order;
using TechShopSolution.ViewModels.Sales;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CheckoutRequest request)
        {
            var result = await _orderService.Create(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
