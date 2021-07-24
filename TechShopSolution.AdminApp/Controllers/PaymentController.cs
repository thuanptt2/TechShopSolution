using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.PaymentMethod;

namespace TechShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentApiClient _paymentApiClient;
        public PaymentController(IPaymentApiClient paymentApiClient)
        {
            _paymentApiClient = paymentApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetPaymentPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _paymentApiClient.GetPaymentPagings(request);
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
            var result = await _paymentApiClient.ChangeStatus(id);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
            }
            else
            {
                TempData["result"] = "Thay đổi trạng thái thành công";
                return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _paymentApiClient.GetById(id);
            if (!result.IsSuccess || result.ResultObject == null)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index");
            }
            var updateRequest = new PaymentUpdateRequest()
            {
                id = id,
                isActive = result.ResultObject.isActive,
                name = result.ResultObject.name,
                description = result.ResultObject.description,
            };
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(updateRequest);
        }
        public async Task<IActionResult> Delete(int id, string pageIndex)
        {
            var result = await _paymentApiClient.Delete(id);
            if (result.IsSuccess)
            {
                TempData["result"] = "Xóa phương thức thành công";
                return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
            }
            TempData["error"] = result.Message;
            return RedirectToAction("Index", new { pageIndex = !string.IsNullOrWhiteSpace(pageIndex) ? int.Parse(pageIndex) : 1 });
        }
        [HttpPost]
        public async Task<IActionResult> Update(PaymentUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _paymentApiClient.UpdatePayment(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật phương thức thành công";
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
        [HttpPost]
        public async Task<IActionResult> Create(PaymentCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _paymentApiClient.CreatePayment(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Thêm phương thức thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }    
    }
}
