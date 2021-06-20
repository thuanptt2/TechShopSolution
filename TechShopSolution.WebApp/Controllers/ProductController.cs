using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.Product;
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
        [Route("san-pham/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var product = await _productApiClient.GetBySlug(slug);
            string[] CateId = product.ResultObject.CateID.Split(",");
            var Category = await _categorytApiClient.GetById(int.Parse(CateId[0]));
            return View(new ProductDetailViewModel()
            {
                Product = product.ResultObject,
                Category = Category.ResultObject,
                ProductsRelated = await _productApiClient.GetProductsRelated(product.ResultObject.id, 4),
                ImageList = await _productApiClient.GetImageByProductID(product.ResultObject.id),
            });
        }
        [Route("danh-muc/{slug}")]
        public async Task<IActionResult> Category(string slug, int page = 1)
        {
            var Category = await _categorytApiClient.GetBySlug(slug);
            List<int?> CateID = new List<int?>();
            CateID.Add(Category.ResultObject.id);
            var products = await _productApiClient.GetProductPagingsWithMainImage(new GetProductPagingRequest()
            {
                CategoryID = CateID,
                PageIndex = page,
                PageSize = 9,
            });
            return View(new ProductCategoryViewModel() { 
                Category = Category.ResultObject,
                Products = products
            });
        }
    }
}
