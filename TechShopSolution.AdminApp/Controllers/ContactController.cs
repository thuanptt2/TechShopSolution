using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Website.Contact;

namespace TechShopSolution.AdminApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactApiClient _contactApiClient;
        public ContactController(IContactApiClient contactApiClient)
        {
            _contactApiClient = contactApiClient;
        }
        public async Task<IActionResult> Detail()
        {
            var contact = await _contactApiClient.GetcontactInfos();
            var updateModel = new ContactUpdateRequest()
            {
                adress = contact.ResultObject.adress,
                imageBase64 = contact.ResultObject.company_logo,
                email = contact.ResultObject.email,
                company_name = contact.ResultObject.company_name,
                fax = contact.ResultObject.fax,
                hotline = contact.ResultObject.hotline,
                id = contact.ResultObject.id,
                phone = contact.ResultObject.phone,
                social_fb = contact.ResultObject.social_fb,
                social_instagram = contact.ResultObject.social_instagram,
                social_twitter = contact.ResultObject.social_twitter,
                social_youtube = contact.ResultObject.social_youtube
            };
            return View(updateModel);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(ContactUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View("Detail",request);
            var result = await _contactApiClient.UpdateContact(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("Detail");
            }
            ModelState.AddModelError("", result.Message);
            return View("Detail", request);
        }
    }
}
