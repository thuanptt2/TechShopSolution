using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.AdminApp.Controllers
{
    public class SlideController : Controller
    {
        private readonly ISlideApiClient _slideApiClient;
        public SlideController(ISlideApiClient slideApiClient)
        {
            _slideApiClient = slideApiClient;
        }
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10)
        {
            var request = new PagingRequestBase()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _slideApiClient.GetSlidePagings(request);
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
