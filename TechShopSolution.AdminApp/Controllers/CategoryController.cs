using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.AdminApp.Service;
using TechShopSolution.ViewModels.Catalog.Category;

namespace TechShopSolution.AdminApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        public CategoryController(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetCategoryPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };
            var data = await _categoryApiClient.GetCategoryPagings(request);
            var tree = await OrderCateToTree(data.Items);
            data.Items = tree.Skip((request.PageIndex - 1) * request.PageSize)
                             .Take(request.PageSize).ToList();
            data.TotalRecords = tree.Count();


            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }
        public async Task<List<CategoryViewModel>> OrderCateToTree(List<CategoryViewModel> lst, int parent_id = 0, int level = 0)
        {
            List<CategoryViewModel> result = new List<CategoryViewModel>();
            foreach (CategoryViewModel cate in lst)
            {
                if (cate.parent_id == parent_id)
                {
                    CategoryViewModel tree = new CategoryViewModel();
                    tree = cate;
                    tree.level = level;
                    tree.cate_name = String.Concat(Enumerable.Repeat("|————", level)) + tree.cate_name;

                    result.Add(tree);
                    List<CategoryViewModel> child = await OrderCateToTree(lst, cate.id, level + 1);
                    child.OrderByDescending(m => m.create_at);
                    result.AddRange(child);
                }
            }
            return result;
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var cate_tree = await _categoryApiClient.GetAllCategory();
            ViewBag.ListCate =  await OrderCateToTree(cate_tree);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _categoryApiClient.CreateCategory(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Thêm loại sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

    }
}
