using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.AdminApp.Models;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Report;

namespace TechShopSolution.AdminApp.Controllers
{ 
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IOrderApiClient _orderApiClient;
        private readonly IProductApiClient _productApiClient;
        private readonly IReportApiClient _reportApiClient;

        public HomeController(IOrderApiClient orderApiClient, IProductApiClient productApiClient, IReportApiClient reportApiClient)
        {
            _orderApiClient = orderApiClient;
            _productApiClient = productApiClient;
            _reportApiClient = reportApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var orderStatistics = await _orderApiClient.GetOrderStatistics();
            var viewRanking = await _productApiClient.GetProductViewRanking(10);
            var ratingRanking = await _productApiClient.GetProductMostSalesRanking(10);
            var favoriteRanking = await _productApiClient.GetProductFavoriteRanking(10);
            return View(new DashBoardViewModel() {
                OrderStatistics = orderStatistics.ResultObject,
                viewRanking = viewRanking,
                salesRanking = ratingRanking,
                favoriteRanking = favoriteRanking
            }); 
        }
        public async Task<IActionResult> GetRevenueReport(string fromDate = null, string toDate = null)
        {
            var result = await _reportApiClient.GetRevenueReport(new GetRevenueRequest { fromDate = fromDate, toDate = toDate });
            if(result.IsSuccess)
                return Ok(result.ResultObject);
            return BadRequest(result.Message);
        }
        public async Task<IActionResult> GetRevenueByMonthReport(string fromDate = null, string toDate = null)
        {
            var result = await _reportApiClient.GetRevenueByMonthReport(new GetRevenueRequest { fromDate = fromDate, toDate = toDate });
            if (result.IsSuccess)
                return Ok(result.ResultObject);
            return BadRequest(result.Message);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
