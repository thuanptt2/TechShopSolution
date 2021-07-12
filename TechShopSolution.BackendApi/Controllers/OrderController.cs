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
        [HttpGet("paging")]
        public IActionResult GetAllPaging([FromQuery] GetOrderPagingRequest requet)
        {
            var customer = _orderService.GetAllPaging(requet);
            return Ok(customer);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _orderService.Detail(id);
            return Ok(result);
        }
        [HttpGet("paymentconfirm/{id}")]
        public async Task<IActionResult> PaymentConfirm(int id)
        {
            var result = await _orderService.PaymentConfirm(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("cancelorder")]
        public async Task<IActionResult> Cancelorder(OrderCancelRequest request)
        {
            var result = await _orderService.CancelOrder(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("confirm/{id}")]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var result = await _orderService.ConfirmOrder(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("UpdateReceiveAddress")]
        public async Task<IActionResult> UpdateAddress([FromBody] OrderUpdateAddressRequest request)
        {
            var result = await _orderService.UpdateAddress(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("CustomerOrders/{id}")]
        public async Task<IActionResult> GetCustomerOrders(int id)
        {
            var result = await _orderService.GetCustomerOrders(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("OrderDetail/{id}")]
        public IActionResult DetailOrder(int id)
        {
            var result = _orderService.GetDetailOrder(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
