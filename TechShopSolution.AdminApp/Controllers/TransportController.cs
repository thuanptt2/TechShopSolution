using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Transport;

namespace TechShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class TransportController : Controller
    {
        private readonly ITransportApiClient _transportApiClient;
        public TransportController(ITransportApiClient transportApiClient)
        {
            _transportApiClient = transportApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetTransporterPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _transportApiClient.GetTransporterPagings(request);
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
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _transportApiClient.ChangeStatus(id);
            if (result == null)
            {
                ModelState.AddModelError("Cập nhật thất bại", result.Message);
            }
            if (result.IsSuccess)
            {
                TempData["result"] = "Thay đổi trạng thái thành công";
                return RedirectToAction("Index");
            }
            return View("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _transportApiClient.GetById(id);
            if (!result.IsSuccess || result.ResultObject == null)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index");
            }
            var updateRequest = new TransporterUpdateRequest()
            {
                id = id,
                isActive = result.ResultObject.isActive,
                name = result.ResultObject.name,
                link = result.ResultObject.link,
                imageBase64 = result.ResultObject.image
            };
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(updateRequest);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _transportApiClient.Delete(id);
            if (result.IsSuccess)
            {
                TempData["result"] = "Xóa thành công";
                return RedirectToAction("Index");
            }
            TempData["error"] = result.Message;
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(TransporterUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _transportApiClient.UpdateTransporter(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật thành công";
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(TransporterCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _transportApiClient.CreateTransporter(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Thêm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> CreateShippingOrder(int id, decimal cod_price, string receive_address)
        {
            ViewBag.Transporter = await _transportApiClient.GetAll();
            
            var request = new CreateTransportRequest()
            {
                order_id = id,
                to_address = receive_address,
                from_address = "12 Đường 41, Phường 10, Quận 6, TP Hồ Chí Minh",
                cod_price = null
            };
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> CreateShippingOrder(CreateTransportRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Transporter = await _transportApiClient.GetAll();
                return View(request);
            }
            var result = await _transportApiClient.CreateShippingOrder(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Tạo đơn vận chuyển thành công";
                return RedirectToAction("Detail", "Order", new { id = request.order_id });
            }
            TempData["error"] = result.Message;
            return RedirectToAction("Detail", "Order", new { id = request.order_id });
        }
        [HttpGet]
        public IActionResult UpdateLadingCode(int id, string lading_code)
        {
            var request = new UpdateLadingCodeRequest()
            {
                Id = id,
                New_LadingCode = lading_code,
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
                return RedirectToAction("Detail", "Transport", new { id = request.Id});
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> CancelShippingOrder(int id)
        {
            var result = await _transportApiClient.CancelShippingOrder(id);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Detail", new { id = id });
            }
            TempData["result"] = result.ResultObject;
            return RedirectToAction("Detail", new { id = id });
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _transportApiClient.Detail(id);
            if (!result.IsSuccess || result.ResultObject == null)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("ListTransport");
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
        public async Task<IActionResult> ConfirmDoneShip(int transport_id)
        {
            var result = await _transportApiClient.ConfirmDoneShip(transport_id);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Detail", new { id = transport_id });
            }
            TempData["result"] = result.ResultObject;
            return RedirectToAction("Detail", new { id = transport_id });
        }
        public async Task<IActionResult> ListTransport(string keyword, int pageIndex = 1, int pageSize = 20)
        {
            var request = new GetTransportPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _transportApiClient.GetTransportPagings(request);
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

    }
}
