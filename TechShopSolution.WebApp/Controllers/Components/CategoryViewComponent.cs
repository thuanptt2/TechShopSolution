using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.Category;

namespace TechShopSolution.WebApp.Controllers.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryApiClient _categorytApiClient;
        public CategoryViewComponent(ICategoryApiClient categorytApiClient)
        {
            _categorytApiClient = categorytApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categorytApiClient.GetAllCategory();
            List<CategoryViewModel> parents = items.Where(x => x.parent_id == 0).OrderBy(x => x.cate_name).ToList();
            ViewBag.Parents = parents;
            return View(items);
        }
    }
}
