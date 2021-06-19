using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.Category;

namespace TechShopSolution.WebApp.Controllers.Components
{
    public class SlideBarViewComponent : ViewComponent

    {
        private readonly ICategoryApiClient _categorytApiClient;

        public SlideBarViewComponent(ICategoryApiClient categorytApiClient)
        { 
            _categorytApiClient = categorytApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categorytApiClient.GetAllCategory();
            return View(items);
        }

    }
}
