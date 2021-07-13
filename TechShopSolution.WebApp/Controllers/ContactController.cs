using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Website.Contact;

namespace TechShopSolution.WebApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactApiClient _contactApiClient;
        public ContactController(IContactApiClient contactApiClient)
        {
            _contactApiClient = contactApiClient;
        }
        public async Task<IActionResult> Index()
        {
            var contact = await _contactApiClient.GetcontactInfos();
            return View(contact.ResultObject);
        }
    }
}
