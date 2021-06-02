using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TechShopSolution.AdminApp.Service;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Location;

namespace TechShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerApiClient _customerApiClient;
        public CustomerController(ICustomerApiClient customerApiClient)
        {
            _customerApiClient = customerApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetCustomerPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _customerApiClient.GetCustomerPagings(request);
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CustomerCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _customerApiClient.CreateCustomer(request);
            if(result.IsSuccess)
                return RedirectToAction("Index");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _customerApiClient.GetById(id);
            if (!result.IsSuccess || result.ResultObject == null)
            {
                ModelState.AddModelError("", result.Message);
                return View("Index");
            }
            var updateRequest = new CustomerUpdateRequest()
            {
                Id = id,
                name = result.ResultObject.name,
                address = result.ResultObject.address,
                birthday = result.ResultObject.birthday,
                email = result.ResultObject.email,
                password = result.ResultObject.password,
                sex = result.ResultObject.sex,
                phone = result.ResultObject.phone,
                status = result.ResultObject.status
            };
            return View(updateRequest);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CustomerUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _customerApiClient.UpdateCustomer(request);
            if (result.IsSuccess)
                return RedirectToAction("Index");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateAddress(int id)
        {
            var result = await _customerApiClient.GetById(id);
            if (!result.IsSuccess || result.ResultObject == null)
                ModelState.AddModelError("", result.Message);
            var updateAddressRequest = new CustomerUpdateAddressRequest()
            {
                Id = id,
                City = null,
                District = null,
                House = null,
                Ward = null
            };
            return View(updateAddressRequest);
        }
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress(CustomerUpdateAddressRequest request)
        {
            var result = await _customerApiClient.UpdateAddress(request);
            if(result == null)
                return Json(new { success = false, message = "Cập nhật thất bại" });
            if (result.IsSuccess)
                return Json(new { success = true, message = "Cập nhật thành công" });
            return Json(new { success = false, message = result.Message });
        }
        [HttpGet]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _customerApiClient.ChangeStatus(id);
            if (result == null)
            {
                ModelState.AddModelError("Cập nhật thất bại", result.Message);
            }
            if (result.IsSuccess)
                return RedirectToAction("Index");
            return View();

        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerApiClient.Delete(id);
            if (result == null)
            {
                ModelState.AddModelError("", result.Message);
            }
            if (result.IsSuccess)
                return RedirectToAction("Index");
            return View("Index");
        }

        public async Task<JsonResult> LoadProvince()
        {
            try
            {
                var result = await _customerApiClient.LoadProvince();
                if (result == null || !result.IsSuccess)
                {
                    return null;
                }
                return Json(new
                {
                    data = result.ResultObject,
                    status = true
                });
            }
            catch
            {
                return null;
            }
        }
        public async Task<JsonResult> LoadDistrict(int provinceID)
        {
            try
            {
                var result = await _customerApiClient.LoadDistrict(provinceID);
                if (result == null || !result.IsSuccess)
                {
                    return null;
                }
                return Json(new
                {
                    data = result.ResultObject,
                    status = true
                });
            }
            catch
            {
                return null;
            }
        }

        public async Task<JsonResult> LoadWard(int districtID)
        {
            try
            {
                var result = await _customerApiClient.LoadWard(districtID);
                if (result == null || !result.IsSuccess)
                {
                    return null;
                }
                return Json(new
                {
                    data = result.ResultObject,
                    status = true
                });
            }
            catch
            {
                return null;
            }
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(string email, int Id)
        {
            if (await _customerApiClient.VerifyEmail(email) == false)
            {
                return Json($"Email {email} đã được sử dụng.");
            }
            return Json(true);
        }
    }
}
