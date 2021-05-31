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
        private readonly XDocument xmlDoc = XDocument.Load("wwwroot/assets/location/Provinces_Data.xml");
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
            if(!result.IsSuccess || result.ResultObject == null)
                ModelState.AddModelError("", result.Message);
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
        [HttpPost]
        public async Task<IActionResult> UpdateAddress(CustomerUpdateAddressRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _customerApiClient.UpdateAddress(request);
            if (result.IsSuccess)
                return RedirectToAction("Update","Customer", request);
            ModelState.AddModelError("", result.Message);
            return View();
        }
        public JsonResult LoadProvince()
        {
            try
            {
                var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
                var list = new List<ProvinceModel>();
                ProvinceModel province = null;
                foreach (var item in xElements)
                {
                    province = new ProvinceModel();
                    province.ID = int.Parse(item.Attribute("id").Value);
                    province.Name = item.Attribute("value").Value;
                    list.Add(province);

                }
                return Json(new
                {
                    data = list,
                    status = true
                });
            }
            catch
            {
                return null;
            }
        }
        public JsonResult LoadDistrict(string provinceName)
        {
            try
            {
                var xElement = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && x.Attribute("value").Value.Equals(provinceName));

                var list = new List<DistrictModel>();
                DistrictModel district = null;
                foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
                {
                    district = new DistrictModel();
                    district.ID = int.Parse(item.Attribute("id").Value);
                    district.Name = item.Attribute("value").Value;
                    district.ProvinceID = int.Parse(xElement.Attribute("id").Value);
                    list.Add(district);
                }
                return Json(new
                {
                    data = list,
                    status = true
                });
            } catch
            {
                return null;
            }
        }
        public JsonResult LoadWard(string districtName)
        {
            try
            {
                var xElement = xmlDoc.Element("Root").Elements("Item").Elements("Item")
               .Single(x => x.Attribute("type").Value == "district" && x.Attribute("value").Value.Equals(districtName));

                var list = new List<WardModel>();
                WardModel ward = null;
                foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "precinct"))
                {
                    ward = new WardModel();
                    ward.ID = int.Parse(item.Attribute("id").Value);
                    ward.Name = item.Attribute("value").Value;
                    ward.DistrictId = int.Parse(xElement.Attribute("id").Value);
                    list.Add(ward);
                }
                return Json(new
                {
                    data = list,
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
