using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechShopSolution.Data.EF;
using TechShopSolution.ViewModels.Catalog.Brand;
using TechShopSolution.ViewModels.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TechShopSolution.Application.Catalog.Brand
{
    public class BrandService : IBrandService
    {
        private readonly TechShopDBContext _context;
        public BrandService(TechShopDBContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> ChangeStatus(int id)
        {
            try
            {
                var brandExist = await _context.Brands.FindAsync(id);
                if (brandExist != null || brandExist.isDelete)
                {
                    if (brandExist.isActive)
                        brandExist.isActive = false;
                    else brandExist.isActive = true;
                    brandExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy thương hiệu này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> Create(BrandCreateRequest request)
        {
            try
            {
                var brand = new TechShopSolution.Data.Entities.Brand
                {
                    brand_name = request.brand_name,
                    meta_descriptions = request.meta_descriptions,
                    meta_keywords = request.meta_keywords,
                    meta_title = request.meta_title,
                    brand_slug = request.brand_slug,
                    Products = new List<Data.Entities.Product>(),
                    isActive = request.isActive,
                    isDelete = false,
                    create_at = DateTime.Now
                };
                _context.Brands.Add(brand);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch
            {
                return new ApiErrorResult<bool>("Thêm thất bại");
            }
        }
        public async Task<ApiResult<bool>> Delete(int brandID)
        {
            try
            {
                var brand = await _context.Brands.FindAsync(brandID);
                if (brand != null)
                {
                    brand.isDelete = true;
                    brand.delete_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>($"Thương hiệu không tồn tại");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
        public async Task<PagedResult<BrandViewModel>> GetAllPaging(GetBrandPagingRequest request)
        {
            var query = from brand in _context.Brands
                        where brand.isDelete == false
                        select brand;

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.brand_name.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = query.OrderByDescending(m => m.create_at)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new BrandViewModel()
                {
                    id = a.id,
                    brand_name = a.brand_name,
                    brand_slug = a.brand_slug,
                    meta_descriptions = a.meta_descriptions,
                    meta_keywords = a.meta_keywords,
                    meta_title = a.meta_title,
                    isActive = a.isActive,
                }).ToListAsync();

            var pageResult = new PagedResult<BrandViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = await data,
            };
            return pageResult;
        }
        public async Task<ApiResult<BrandViewModel>> GetById(int brandId)
        {
            var brandExist = await _context.Brands.FindAsync(brandId);
            if (brandExist == null || brandExist.isDelete)
            {
                return new ApiErrorResult<BrandViewModel>("Thương hiệu không tồn tại");
            }
            var brand = new BrandViewModel()
            {
                id = brandExist.id,
                brand_name = brandExist.brand_name,
                brand_slug = brandExist.brand_slug,
                create_at = brandExist.create_at,
                delete_at = brandExist.delete_at,
                update_at = brandExist.update_at,
                isActive = brandExist.isActive,
                isDelete = brandExist.isDelete,
                meta_descriptions = brandExist.meta_descriptions,
                meta_keywords = brandExist.meta_keywords,
                meta_title = brandExist.meta_title
            };
            return new ApiSuccessResult<BrandViewModel>(brand);
        }
        public async Task<ApiResult<bool>> Update(int id, BrandUpdateRequest request)
        {
            try
            {
                var brandExist = await _context.Brands.FindAsync(request.id);
                if (brandExist != null || brandExist.isDelete)
                {
                    brandExist.brand_name = request.brand_name;
                    brandExist.brand_slug = request.brand_slug;
                    brandExist.isActive = request.isActive;
                    brandExist.meta_descriptions  = request.meta_descriptions;
                    brandExist.meta_keywords = request.meta_keywords;
                    brandExist.meta_title = request.meta_title;
                    brandExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy thương hiệu này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<bool> isValidSlug(int id, string slug)
        {
            if (await _context.Brands.AnyAsync(x => x.brand_slug.Equals(slug) && x.id != id && x.isDelete == false))
                return false;
            return true;
        }
    }
}
