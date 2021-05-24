using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.System;
using TechShopSolution.ViewModels.System;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpPost("dang-nhap")]
        [AllowAnonymous]
        public IActionResult Authencicate([FromForm] LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                string resultToken = _adminService.Authenticate(request);
                if (!string.IsNullOrEmpty(resultToken))
                {
                    return Ok( new { token = resultToken });
                }
                else return BadRequest("Email hoặc mật khẩu không đúng");
            }
            return BadRequest(ModelState);
        }

    }
}
