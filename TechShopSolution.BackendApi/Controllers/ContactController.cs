using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Website.Contact;
using TechShopSolution.ViewModels.Website.Contact;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] ContactUpdateRequest request)
        {
            var result = await _contactService.Update(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateFeedback(FeedbackCreateRequest request)
        {
            var result = await _contactService.CreateFeedback(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetContactData()
        {
            var result = _contactService.GetContactData();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedback(int id)
        {
            var result = await _contactService.GetById(id);
            return Ok(result);
        }
        [HttpGet("feedback/changestatus/{id}")]
        public async Task<IActionResult> ChangeFeedbackStatus(int id)
        {
            var result = await _contactService.ChangeFeedbackStatus(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("feedback/paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetFeedbackPagingRequets requet)
        {
            var feedback = await _contactService.GetFeedbackPaging(requet);
            return Ok(feedback);
        }
        [HttpDelete("feedback/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contactService.Delete(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
