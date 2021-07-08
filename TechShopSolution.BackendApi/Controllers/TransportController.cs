using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Catalog.Transport;
using TechShopSolution.ViewModels.Transport;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportController : ControllerBase
    {
        private readonly ITransportService _transportService;

        public TransportController(ITransportService transportService)
        {
            _transportService = transportService;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetTransporterPagingRequest requet)
        {
            var payment = await _transportService.GetAllPaging(requet);
            return Ok(payment);
        }
        [HttpGet("pagingtransport")]
        public async Task<IActionResult> GetPagingTransport([FromQuery] GetTransportPagingRequest request)
        {
            var payment = await _transportService.GetPagingTransport(request);
            return Ok(payment);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var payment = await _transportService.GetAll();
            return Ok(payment);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _transportService.GetById(id);
            return Ok(result);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] TransporterCreateRequest request)
        {
            var result = await _transportService.Create(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] TransporterUpdateRequest request)
        {
            var result = await _transportService.Update(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _transportService.ChangeStatus(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _transportService.Delete(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("createshipping")]
        public async Task<IActionResult> Create([FromBody] CreateTransportRequest request)
        {
            var result = await _transportService.CreateShippingOrder(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("newladingcode")]
        public async Task<IActionResult> UpdateLadingCode([FromBody] UpdateLadingCodeRequest request)
        {
            var result = await _transportService.UpdateLadingCode(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("canceltransport/{id}")]
        public async Task<IActionResult> Cancelorder(int id)
        {
            var result = await _transportService.CancelShippingOrder(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

    }
}
