using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Sales;

namespace TechShopSolution.AdminApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderApiClient _orderApiClient;
        public OrderController(IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 20)
        {
            var request = new GetOrderPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _orderApiClient.GetOrderPagings(request);
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
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _orderApiClient.GetById(id);
            if (!result.IsSuccess || result.ResultObject == null)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index");
            }
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMsg = TempData["error"];
            }
            return View(result.ResultObject);
        }
        [HttpGet]
        public async Task<IActionResult> PaymentConfirm(int id)
        {
            var result = await _orderApiClient.PaymentConfirm(id);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Detail", new { id = id });
            }
            TempData["result"] = result.ResultObject;
            return RedirectToAction("Detail", new { id = id });
        }
        [HttpGet]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderApiClient.CancelOrder(id);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Detail", new { id = id });
            }
            TempData["result"] = result.ResultObject;
            return RedirectToAction("Detail", new { id = id });
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var result = await _orderApiClient.ConfirmOrder(id);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Detail", new { id = id });
            }
            TempData["result"] = result.ResultObject;
            return RedirectToAction("Detail", new { id = id });
        }

    }
}
