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
        private readonly IBrandApiClient _brandApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categorytApiClient, IBrandApiClient brandApiClient)
        {
            _productApiClient = productApiClient;
            _categorytApiClient = categorytApiClient;
            _brandApiClient = brandApiClient;
        }
        [Route("san-pham/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var product = await _productApiClient.GetBySlug(slug);
            string[] CateId = product.ResultObject.CateID.Split(",");
            var Category = await _categorytApiClient.GetById(int.Parse(CateId[0]));
            var Brand = await _brandApiClient.GetById(product.ResultObject.brand_id);
            
            return View(new ProductDetailViewModel()
            {
                Product = product.ResultObject,
                Category = Category.ResultObject,
                Brand = Brand.ResultObject,
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
            ViewBag.PageResult = products;
            return View(new ProductCategoryViewModel() { 
                Category = Category.ResultObject,
                Products = products
            });
        }
    }
}
