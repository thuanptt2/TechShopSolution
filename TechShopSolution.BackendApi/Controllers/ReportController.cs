using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Application.Dapper.Report;
using TechShopSolution.ViewModels.Report;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet("Revenue")]
        public async Task<IActionResult> GetRevenueReport([FromQuery]GetRevenueRequest request) 
        {
            var result = await _reportService.GetReportAsync(request.fromDate, request.toDate);
            if (result.IsSuccess)
                return Ok(result);
            else return BadRequest(result);
        }
    }
}
