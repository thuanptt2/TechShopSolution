using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Sales;
using TechShopSolution.ViewModels.Transport;

namespace TechShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderApiClient _orderApiClient;
        private readonly ITransportApiClient _transportApiClient;

        public OrderController(IOrderApiClient orderApiClient, ITransportApiClient transportApiClient)
        {
            _orderApiClient = orderApiClient;
            _transportApiClient = transportApiClient;
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
        public IActionResult CancelOrder(int id)
        {
            var request = new OrderCancelRequest()
            {
                Id = id
            };
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> CancelOrder(OrderCancelRequest request)
        {
            var result = await _orderApiClient.CancelOrder(request);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Detail", new { id = request.Id });
            }
            TempData["result"] = result.ResultObject;
            return RedirectToAction("Detail", new { id = request.Id });
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
        [HttpGet]
        public IActionResult UpdateReceiveAddress(int id)
        {
            var updateAddressRequest = new OrderUpdateAddressRequest()
            {
                Id = id,
                City = null,
                District = null,
                House = null,
                Ward = null
            };
            return View(updateAddressRequest);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateReceiveAddress(OrderUpdateAddressRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _orderApiClient.UpdateAddress(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật địa chỉ giao hàng thành công";
                return RedirectToAction(nameof(Detail), new { id = request.Id });
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> CancelShippingOrder(int transport_id, int order_id)
        {
            var result = await _transportApiClient.CancelShippingOrder(transport_id);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Detail", new { id = order_id });
            }
            TempData["result"] = result.ResultObject;
            return RedirectToAction("Detail", new { id = order_id });
        }
        [HttpGet]
        public IActionResult UpdateLadingCode(int id, string lading_code, int order_id)
        {
            var request = new UpdateLadingCodeRequest()
            {
                Id = id,
                New_LadingCode = lading_code,
                order_id = order_id,
            };
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLadingCode(UpdateLadingCodeRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _transportApiClient.UpdateLadingCode(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật mã vận đơn thành công";
                return RedirectToAction("Detail", "Order", new { id = request.order_id });
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
    }
}
