using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.WebApp.Models;

namespace TechShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categorytApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categorytApiClient)
        {
            _productApiClient = productApiClient;
            _categorytApiClient = categorytApiClient;
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
                ProductsRelated = await _productApiClient.GetProductsRelated(id, 4),
                ImageList = await _productApiClient.GetImageByProductID(id),
            });
        }
    }
}
