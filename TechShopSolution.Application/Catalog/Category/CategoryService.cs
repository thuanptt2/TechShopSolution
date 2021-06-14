using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Data.EF;
using TechShopSolution.ViewModels.Catalog.Category;
using TechShopSolution.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace TechShopSolution.Application.Catalog.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly TechShopDBContext _context;
        public CategoryService(TechShopDBContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<CategoryViewModel>> GetAllPaging(GetCategoryPagingRequest request)
        {
            var query = from cate in _context.Categories
                        where cate.isDelete == false
                        select cate;

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.cate_name.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = query.Select(a => new CategoryViewModel()
                {
                    id = a.id,
                    cate_name = a.cate_name,
                    cate_slug = a.cate_slug,
                    meta_descriptions = a.meta_descriptions,
                    meta_keywords = a.meta_keywords,
                    meta_title = a.meta_title,
                    parent_id = a.parent_id,
                    isActive = a.isActive,
                }).ToListAsync();

            var pageResult = new PagedResult<CategoryViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = await data,
            };
            return pageResult;
        }
        public async Task<ApiResult<bool>> Create(CreateCategoryRequest request)
        {
            try
            {
                var cate = new TechShopSolution.Data.Entities.Category
                {
                    cate_name = request.cate_name,
                    meta_descriptions = request.meta_descriptions,
                    meta_keywords = request.meta_keywords,
                    meta_title = request.meta_title,
                    cate_slug = request.cate_slug,
                    ProductInCategory = new List<Data.Entities.CategoryProduct>(),
                    isActive = request.isActive,
                    isDelete = false,
                    create_at = DateTime.Now
                };
                _context.Categories.Add(cate);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch
            {
                return new ApiErrorResult<bool>("Thêm thất bại");
            }
        }

    }
}
