using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Website.Slide;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Website;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlideController : ControllerBase
    {
        private readonly ISlideService _slideService;
        public SlideController(ISlideService slideService)
        {
            _slideService = slideService;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] PagingRequestBase requet)
        {
            var slides = await _slideService.GetAllPaging(requet);
            return Ok(slides);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var slides = await _slideService.GetPublicSlide();
            return Ok(slides);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var result = await _slideService.GetById(id);
            return Ok(result);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] SlideCreateRequest request)
        {
            var result = await _slideService.Create(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] SlideUpdateRequest request)
        {
            var result = await _slideService.Update(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _slideService.ChangeStatus(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _slideService.Delete(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
