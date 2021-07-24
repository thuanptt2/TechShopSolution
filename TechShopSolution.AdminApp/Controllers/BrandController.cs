using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.Brand;

namespace TechShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        private readonly IBrandApiClient _brandApiClient;
        public BrandController(IBrandApiClient brandApiClient)
        {
            _brandApiClient = brandApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 15)
        {
            var request = new GetBrandPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _brandApiClient.GetBrandPagings(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMsg = TempData["error"];
            }
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _brandApiClient.CreateBrand(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Thêm thương hiệu thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _brandApiClient.GetById(id);
            if (!result.IsSuccess || result.ResultObject == null)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index");
            }
            var updateRequest = new BrandUpdateRequest()
            {
                id = id,
                brand_name = result.ResultObject.brand_name,
                brand_slug = result.ResultObject.brand_slug,
                isActive = result.ResultObject.isActive,
                meta_descriptions = result.ResultObject.meta_descriptions,
                meta_keywords = result.ResultObject.meta_keywords,
                meta_title = result.ResultObject.meta_title
            };
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(updateRequest);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BrandUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _brandApiClient.UpdateBrand(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật thương hiệu thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> ChangeStatus(int id, string pageIndex)
        {
            var result = await _brandApiClient.ChangeStatus(id);
            if (result.IsSuccess)
            {
                TempData["result"] = "Thay đổi trạng thái thành công";

                return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
            }
            ModelState.AddModelError("Cập nhật thất bại", result.Message);
            return View("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1});
        }
        public async Task<IActionResult> Delete(int id, string pageIndex)
        {
            
            var result = await _brandApiClient.Delete(id);
            if (result.IsSuccess)
            {
                TempData["result"] = "Xóa thương hiệu thành công";
                return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
            }
            TempData["error"] = result.Message;
            return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> isValidSlug(int id, string brand_slug)
        {
            if (await _brandApiClient.isValidSlug(id, brand_slug) == false)
            {
                return Json($"Đường dẫn {brand_slug} đã được sử dụng.");
            }
            return Json(true);
        }
    }
}
