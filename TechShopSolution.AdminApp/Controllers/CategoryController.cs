using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
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
            List<CategoryViewModel> tree = new List<CategoryViewModel>();
            if (keyword!=null)
            {
                data.Items = data.Items.Skip((request.PageIndex - 1) * request.PageSize)
                                 .Take(request.PageSize).ToList();
            } else {
                tree = await OrderCateToTree(data.Items);
                if(tree!=null)
                {
                    data.Items = tree.Skip((request.PageIndex - 1) * request.PageSize)
                                 .Take(request.PageSize).ToList();
                    data.TotalRecords = tree.Count();
                }
            }
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }
        public async Task<List<CategoryViewModel>> OrderCateToTree(List<CategoryViewModel> lst, int parent_id = 0, int level = 0)
        {
            if(lst!= null)
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
                        result.AddRange(child);
                    }
                }
                return result;
            }
            return null;
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
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var result = await _categoryApiClient.GetById(id);
            if (!result.IsSuccess || result.ResultObject == null)
            {
                ModelState.AddModelError("", result.Message);
                return View("Index");
            }
            var updateRequest = new UpdateCategoryRequest()
            {
                id = id,
                cate_name = result.ResultObject.cate_name,
                cate_slug = result.ResultObject.cate_slug,
                isActive = result.ResultObject.isActive,
                meta_descriptions = result.ResultObject.meta_descriptions,
                meta_keywords = result.ResultObject.meta_keywords,
                meta_title = result.ResultObject.meta_title,
                parent_id = result.ResultObject.parent_id,
            };
           
            var cate_tree = await _categoryApiClient.GetAllCategory();

            // Không hiển thị lại loại này trong danh sách
            var thisCate = cate_tree.Find(x => x.id == result.ResultObject.id);
            if (thisCate!=null)
                cate_tree.Remove(thisCate);

            // Load danh sách category theo dạng tree
            ViewBag.ListCate = await OrderCateToTree(cate_tree);
            return View(updateRequest);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _categoryApiClient.UpdateCategory(request);
            if (result.IsSuccess)
            {
                TempData["result"] = "Cập nhật loại sản phẩm thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryApiClient.Delete(id);
            if (result == null)
            {
                ModelState.AddModelError("", result.Message);
            }
            if (result.IsSuccess)
            {
                TempData["result"] = "Xóa loại sản phẩm thành công";
                return RedirectToAction("Index");
            }
            return View("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _categoryApiClient.ChangeStatus(id);
            if (result == null)
            {
                ModelState.AddModelError("Cập nhật thất bại", result.Message);
            }
            if (result.IsSuccess)
            {
                TempData["result"] = "Thay đổi trạng thái thành công";
                return RedirectToAction("Index");
            }
            return View("Index");
        }
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> isValidSlug(int id, string cate_slug)
        {
            if (await _categoryApiClient.isValidSlug(id, cate_slug) == false)
            {
                return Json($"Đường dẫn {cate_slug} đã được sử dụng.");
            }
            return Json(true);
        }
    }
}
