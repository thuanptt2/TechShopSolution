using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                ProductWithCate1 = await _productApiClient.GetProductsByCategory(1, 6),
                ProductWithCate2 = await _productApiClient.GetProductsByCategory(2, 6),
            };
            return View(viewModel);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productApiClient.GetById(id);
            string[] CateId = product.ResultObject.CateID.Split(",");
            var Category = await _categorytApiClient.GetById(int.Parse(CateId[0]));
            return View(new ProductDetailViewModel()
            {
                Product = product.ResultObject,
                Category = Category.ResultObject,
                ProductsRelated = await _productApiClient.GetProductsRelated(id, 6),
                ImageList = await _productApiClient.GetImageByProductID(id),
        });
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
