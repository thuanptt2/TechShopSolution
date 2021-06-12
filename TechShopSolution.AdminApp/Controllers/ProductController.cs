using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechShopSolution.AdminApp.Service;
using TechShopSolution.ViewModels.Catalog.Product;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace TechShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IHostingEnvironment _environment;
        public ProductController(IProductApiClient productApiClient, IHostingEnvironment environment)
        {
            _productApiClient = productApiClient;
            _environment = environment;
        }
        public async Task<IActionResult> Index(string keyword, int? CategoryID, int? BrandID, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetProductPagingRequest()
            {
                Keyword = keyword,
                BrandID = BrandID,
                CategoryID = CategoryID,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _productApiClient.GetProductPagings(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _productApiClient.CreateProduct(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {

            var result = await _productApiClient.GetById(id);
            if (!result.IsSuccess || result.ResultObject == null)
            {
                ModelState.AddModelError("", result.Message);
                return View("Index");
            }
            var updateRequest = new ProductUpdateRequest()
            {
                Id = id,
                Best_seller = result.ResultObject.best_seller,
                Brand_id = result.ResultObject.brand_id,
                Code = result.ResultObject.code,
                Descriptions = result.ResultObject.descriptions,
                Featured = result.ResultObject.featured,
                Instock = result.ResultObject.instock,
                IsActive = result.ResultObject.isActive,
                Meta_descriptions = result.ResultObject.meta_descriptions,
                Meta_keywords = result.ResultObject.meta_keywords,
                Meta_tittle = result.ResultObject.meta_tittle,
                Name = result.ResultObject.name,
                Promotion_price = result.ResultObject.promotion_price,
                Short_desc = result.ResultObject.short_desc,
                Slug = result.ResultObject.slug,
                Specifications = result.ResultObject.specifications,
                Unit_price = result.ResultObject.unit_price,
                Warranty = result.ResultObject.warranty
            };
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(updateRequest);
        }

        public async Task<string> sendListMoreImage(List<IFormFile> files)
        {
            var nameListImages = "";
            if (files != null)
            {
                foreach (IFormFile image in files)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\assets\ProductImage", image.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                       await image.CopyToAsync(fileStream);
                    }
                    nameListImages = nameListImages + image.FileName + ",";
                }
            }
            return nameListImages;
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> isValidSlug(string slug)
        {
            if (await _productApiClient.isValidSlug(slug) == false)
            {
                return Json($"Đường dẫn {slug} đã được sử dụng.");
            }
            return Json(true);
        }
    }
}
