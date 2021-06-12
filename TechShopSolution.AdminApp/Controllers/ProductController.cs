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
