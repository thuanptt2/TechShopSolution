using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.Product;
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
            string[] CateIDs = new string[] { "1", "2", "3", "4" , "5" };
            List<PublicCayegoyProductsViewModel> CategoryProducts = new List<PublicCayegoyProductsViewModel>(); 
            for(int i = 0; i < CateIDs.Length; i++)
            {
                var data = await _productApiClient.GetHomeProducts(int.Parse(CateIDs[i]), 8);
                CategoryProducts.Add(data);
            }
            var viewModel = new HomeViewModel
            {
                FeaturedProducts = await _productApiClient.GetFeaturedProducts(20),
                BestSellerProducts = await _productApiClient.GetBestSellerProducts(20),
                ListCategoryProducts = CategoryProducts,
            };
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
