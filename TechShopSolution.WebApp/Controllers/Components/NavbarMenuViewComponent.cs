using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.Category;

namespace TechShopSolution.WebApp.Controllers.Components
{
    public class NavbarMenuViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categorytApiClient;
        private readonly IContactApiClient _contactApiClient;

        public NavbarMenuViewComponent(ICategoryApiClient categorytApiClient, IContactApiClient contactApiClient)
        {
            _categorytApiClient = categorytApiClient;
            _contactApiClient = contactApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categorytApiClient.GetAllCategory();
            var Contact = await _contactApiClient.GetcontactInfos();
            if(Contact.IsSuccess)
            {
                ViewBag.Company_Logo = Contact.ResultObject.company_logo;
            }
            List<CategoryViewModel> parents = items.Where(x => x.parent_id == 0).OrderBy(x => x.cate_name).ToList();
            ViewBag.Parents = parents;
            return View(items);
        }
    }
}
