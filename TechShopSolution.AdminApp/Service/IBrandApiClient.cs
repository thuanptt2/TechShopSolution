using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Brand;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.AdminApp.Service
{
    public interface IBrandApiClient
    {
        Task<PagedResult<BrandViewModel>> GetBrandPagings(GetBrandPagingRequest request);
        Task<ApiResult<bool>> CreateBrand(BrandCreateRequest request);
        Task<ApiResult<bool>> UpdateBrand(BrandUpdateRequest request);
        Task<ApiResult<bool>> ChangeStatus(int Id);
        Task<ApiResult<bool>> Delete(int cusID);
        Task<ApiResult<BrandViewModel>> GetById(int id);
        Task<bool> isValidSlug(int id, string slug);

    }
}
