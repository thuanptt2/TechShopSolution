using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Sales;
using TechShopSolution.ViewModels.System;

namespace TechShopSolution.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAdminApiClient _adminApiClient;
        private readonly ICustomerApiClient _customerApiClient;
        private readonly IConfiguration _configuration;
        public OrderController(IAdminApiClient adminApiClient, IConfiguration configuration, ICustomerApiClient customerApiClient)
        {
            _adminApiClient = adminApiClient;
            _configuration = configuration;
            _customerApiClient = customerApiClient;
        }


        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> OrderTracking()
        {
            var id = User.FindFirst(ClaimTypes.Sid).Value;

            var result = await _customerApiClient.GetCustomerOrders(int.Parse(id));
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Model = result.ResultObject;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> OrderDetail(int id)
        {
            var result = await _customerApiClient.GetOrderDetail(id);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Model = result.ResultObject;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMsg = TempData["error"];
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmReceive(int transport_id, int order_id)
        {
            var result = await _customerApiClient.ConfirmDoneShip(transport_id);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("OrderDetail", new { id = order_id });
            }
            TempData["result"] = result.ResultObject;
            return RedirectToAction("OrderDetail", new { id = order_id });
        }
        [HttpGet]
        public IActionResult OrderCancelReason(int id)
        {
            var request = new OrderCancelRequest()
            {
                Id = id
            };
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> OrderCancelReason(OrderCancelRequest request)
        {
            var result = await _customerApiClient.CancelOrder(request);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("OrderDetail", new { id = request.Id });
            }
            TempData["result"] = result.ResultObject;
            return RedirectToAction("OrderDetail", new { id = request.Id });
        }
    }
}
