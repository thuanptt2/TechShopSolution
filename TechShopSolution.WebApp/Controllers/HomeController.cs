using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.WebApp.Models;

namespace TechShopSolution.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categorytApiClient;

        public HomeController(ILogger<HomeController> logger, IProductApiClient productApiClient, ICategoryApiClient categorytApiClient)
        {
            _logger = logger;
            _productApiClient = productApiClient;
            _categorytApiClient = categorytApiClient;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                FeaturedProducts = await _productApiClient.GetFeaturedProducts(20),
                BestSellerProducts = await _productApiClient.GetBestSellerProducts(20),
                ProductWithCate1 = await _productApiClient.GetProductsByCategory(1, 8),
                ProductWithCate2 = await _productApiClient.GetProductsByCategory(2, 8),
            };
            var cate1 = await _categorytApiClient.GetById(1);
            var cate2 = await _categorytApiClient.GetById(2);
            ViewBag.Cate1 = cate1.ResultObject.cate_slug;
            ViewBag.Cate2 = cate2.ResultObject.cate_slug;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
                TempData["result"] = null;
            }
            return View(viewModel);
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
