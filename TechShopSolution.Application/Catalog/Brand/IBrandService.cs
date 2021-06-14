using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Brand;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.Application.Catalog.Brand
{
    public interface IBrandService
    {
        Task<ApiResult<bool>> Create(BrandCreateRequest request);
        Task<ApiResult<bool>> Update(int id, BrandUpdateRequest request);
        Task<ApiResult<bool>> ChangeStatus(int id);
        Task<ApiResult<bool>> Delete(int brandID);
        Task<ApiResult<BrandViewModel>> GetById(int brandId);
        Task<PagedResult<BrandViewModel>> GetAllPaging(GetBrandPagingRequest request);
        Task<bool> isValidSlug(int id, string slug);
    }
}
