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
                    create_at = a.create_at,
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
                if (request.parent_id == null)
                    request.parent_id = 0;
                else request.parent_id = request.parent_id;
                var cate = new TechShopSolution.Data.Entities.Category
                {
                    cate_name = request.cate_name,
                    parent_id = request.parent_id,
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
        public async Task<List<CategoryViewModel>> GetAllCategory()
        {
            var query = from cate in _context.Categories
                        where cate.isDelete == false
                        select cate;
            if (query == null)
                return null;

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
         
            return await data;
        }
        public async Task<ApiResult<bool>> Delete(int cateID)
        {
            try
            {
                var cate = await _context.Categories.FindAsync(cateID);
                if (cate != null)
                {
                    cate.isDelete = true;
                    cate.delete_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>($"Loại sản phẩm không tồn tại");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
        public async Task<ApiResult<bool>> Update(UpdateCategoryRequest request)
        {
            try
            {
                if (request.parent_id == null)
                    request.parent_id = 0;
                else request.parent_id = request.parent_id;
                var cateExist = await _context.Categories.FindAsync(request.id);
                if (cateExist != null || cateExist.isDelete)
                {
                    cateExist.cate_name = request.cate_name;
                    cateExist.cate_slug = request.cate_slug;
                    cateExist.isActive = request.isActive;
                    cateExist.parent_id = request.parent_id;
                    cateExist.meta_descriptions = request.meta_descriptions;
                    cateExist.meta_keywords = request.meta_keywords;
                    cateExist.meta_title = request.meta_title;
                    cateExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy loại sản phẩm này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<bool> isValidSlug(int id, string slug)
        {
            if (await _context.Categories.AnyAsync(x => x.cate_slug.Equals(slug) && x.id != id && x.isDelete == false))
                return false;
            return true;
        }
        public async Task<ApiResult<bool>> ChangeStatus(int id)
        {
            try
            {
                var cateExist = await _context.Categories.FindAsync(id);
                if (cateExist != null || cateExist.isDelete)
                {
                    if (cateExist.isActive)
                        cateExist.isActive = false;
                    else cateExist.isActive = true;
                    cateExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy loại sản phẩm này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<CategoryViewModel>> GetById(int brandId)
        {
            var cateExist = await _context.Categories.FindAsync(brandId);
            if (cateExist == null || cateExist.isDelete)
            {
                return new ApiErrorResult<CategoryViewModel>("Loại sản phẩm không tồn tại");
            }
            var cate = new CategoryViewModel()
            {
                id = cateExist.id,
                cate_name = cateExist.cate_name,
                cate_slug = cateExist.cate_slug,
                create_at = cateExist.create_at,
                isActive = cateExist.isActive,
                parent_id = cateExist.parent_id,
                meta_descriptions = cateExist.meta_descriptions,
                meta_keywords = cateExist.meta_keywords,
                meta_title = cateExist.meta_title
            };
            return new ApiSuccessResult<CategoryViewModel>(cate);
        }
        public async Task<ApiResult<CategoryViewModel>> GetBySlug(string slug)
        {
            var cateExist = await _context.Categories.Where(x=> x.cate_slug.Equals(slug)).FirstOrDefaultAsync();
            if (cateExist == null || cateExist.isDelete)
            {
                return new ApiErrorResult<CategoryViewModel>("Loại sản phẩm không tồn tại");
            }
            var cate = new CategoryViewModel()
            {
                id = cateExist.id,
                cate_name = cateExist.cate_name,
                cate_slug = cateExist.cate_slug,
                create_at = cateExist.create_at,
                isActive = cateExist.isActive,
                parent_id = cateExist.parent_id,
                meta_descriptions = cateExist.meta_descriptions,
                meta_keywords = cateExist.meta_keywords,
                meta_title = cateExist.meta_title
            };
            return new ApiSuccessResult<CategoryViewModel>(cate);
        }

    }
}
