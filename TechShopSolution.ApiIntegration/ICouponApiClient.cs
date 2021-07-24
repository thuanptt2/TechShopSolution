using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Coupon;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ApiIntegration
{
    public interface ICouponApiClient
    {
        Task<PagedResult<CouponViewModel>> GetCouponPagings(GetCouponPagingRequest request);
        Task<ApiResult<bool>> CreateCoupon(CouponCreateRequest request);
        Task<ApiResult<bool>> UpdateCoupon(CouponUpdateRequest request);
        Task<ApiResult<bool>> ChangeStatus(int Id);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<CouponViewModel>> GetById(int id);
        Task<ApiResult<CouponViewModel>> UseCoupon(string code, int cus_id);
        Task<bool> isValidCode(int id, string code);
    }
}
