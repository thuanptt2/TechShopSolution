using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.Coupon;

namespace TechShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class CouponController : Controller
    {
        private readonly ICouponApiClient _couponApiClient;
        public CouponController(ICouponApiClient couponApiClient)
        {
            _couponApiClient = couponApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetCouponPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _couponApiClient.GetCouponPagings(request);
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
        public async Task<IActionResult> ChangeStatus(int id, string pageIndex)
        {
            var result = await _couponApiClient.ChangeStatus(id);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
            }
            TempData["result"] = "Thay đổi trạng thái thành công";
            return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _couponApiClient.GetById(id);
            if (!result.IsSuccess || result.ResultObject == null)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index");
            }
            var updateRequest = new CouponUpdateRequest()
            {
                id = id,
                value = result.ResultObject.value,
                type = result.ResultObject.type,
                code = result.ResultObject.code,
                max_price = result.ResultObject.max_price.ToString(),
                min_order_value = result.ResultObject.min_order_value.ToString(),
                end_at = result.ResultObject.end_at.ToString("dd/MM/yyyy"),
                isActive = result.ResultObject.isActive,
                name = result.ResultObject.name,
                quantity = result.ResultObject.quantity,
                start_at = result.ResultObject.start_at.ToString("dd/MM/yyyy"),
            };
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(updateRequest);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CouponUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _couponApiClient.UpdateCoupon(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật mã giảm giá thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id, string pageIndex)
        {
            var result = await _couponApiClient.Delete(id);
            if (result.IsSuccess)
            {
                TempData["result"] = "Xóa mã giảm giá thành công";
                return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
            }
            TempData["error"] = result.Message;
            return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
        }
        public async Task<IActionResult> Create(CouponCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _couponApiClient.CreateCoupon(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Thêm mã giảm giá thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> isValidCode(int id, string code)
        {
            if (await _couponApiClient.isValidCode(id, code) == false)
            {
                return Json($"Mã {code} đã được sử dụng.");
            }
            return Json(true);
        }
    }
}
