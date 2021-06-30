using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Coupon;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.Application.Catalog.Coupon
{
    public interface ICouponService
    {
        Task<ApiResult<bool>> Create(CouponCreateRequest request);
        Task<ApiResult<bool>> Update(CouponUpdateRequest request);
        Task<ApiResult<bool>> ChangeStatus(int id);
        Task<ApiResult<bool>> Delete(int brandID);
        Task<ApiResult<CouponViewModel>> GetById(int brandId);
        Task<PagedResult<CouponViewModel>> GetAllPaging(GetCouponPagingRequest request);
        Task<bool> isValidSlug(int id, string code);
    }
}
