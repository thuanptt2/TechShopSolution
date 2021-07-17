using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;

namespace TechShopSolution.WebApp.Controllers.Components
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly IContactApiClient _contactApiClient;

        public FooterViewComponent(IContactApiClient contactApiClient)
        {
            _contactApiClient = contactApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _contactApiClient.GetcontactInfos();
            return View(items.ResultObject);
        }

    }
}
