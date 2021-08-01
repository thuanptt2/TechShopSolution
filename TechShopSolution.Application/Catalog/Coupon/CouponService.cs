using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.Data.EF;
using TechShopSolution.ViewModels.Catalog.Coupon;
using TechShopSolution.ViewModels.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace TechShopSolution.Application.Catalog.Coupon
{
    public class CouponService : ICouponService
    {
        private readonly TechShopDBContext _context;
        public CouponService(TechShopDBContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> ChangeStatus(int id)
        {
            try
            {
                var coupon = await _context.Coupons.FindAsync(id);
                if (coupon != null)
                {
                    if (coupon.isActive)
                        coupon.isActive = false;
                    else coupon.isActive = true;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy mã giảm giá này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> Create(CouponCreateRequest request)
        {
            try
            {
                
                var coupon = new TechShopSolution.Data.Entities.Coupon
                {
                    code = request.code,
                    value = request.value,
                    type = request.type,
                    name = request.name,
                    end_at = request.end_at,
                    start_at = request.start_at,
                    isActive = request.isActive,
                    quantity = request.quantity,
                };
                if (!string.IsNullOrWhiteSpace(request.max_price))
                    coupon.max_price = double.Parse(request.max_price);
                else coupon.max_price = null;

                if (!string.IsNullOrWhiteSpace(request.min_order_value))
                    coupon.min_order_value = double.Parse(request.min_order_value);
                else coupon.min_order_value = null;

                _context.Coupons.Add(coupon);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch
            {
                return new ApiErrorResult<bool>("Thêm thất bại");
            }
        }
        public async Task<ApiResult<bool>> Delete(int id)
        {
            try
            {
                var coupon = await _context.Coupons.FindAsync(id);
                if (coupon != null)
                {
                    if(await _context.Orders.AnyAsync(x=>x.coupon_id == id))
                        return new ApiErrorResult<bool>($"Bạn chỉ có thể xóa mã giảm giá chưa được ai sử dụng");
                    _context.Coupons.Remove(coupon);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>($"Mã giảm giá không tồn tại");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
        public async Task<PagedResult<CouponViewModel>> GetAllPaging(GetCouponPagingRequest request)
        {
            var query = from coupon in _context.Coupons
                        select coupon;

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.name.Contains(request.Keyword) || x.code.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = query.OrderByDescending(m => m.start_at)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new CouponViewModel()
                {
                    id = a.id,
                    code = a.code,
                    name = a.name,
                    max_price = a.max_price,
                    min_order_value = a.min_order_value,
                    type = a.type,
                    value = a.value,
                    quantity = a.quantity,
                    start_at = a.start_at,
                    end_at = a.end_at,
                    isActive = a.isActive,
                }).ToListAsync();

            var pageResult = new PagedResult<CouponViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = await data,
            };
            return pageResult;
        }
        public async Task<ApiResult<CouponViewModel>> GetById(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return new ApiErrorResult<CouponViewModel>("Mã giảm giá không tồn tại");
            }
            var couponn = new CouponViewModel()
            {
                code = coupon.code,
                end_at = coupon.end_at,
                start_at = coupon.start_at,
                type = coupon.type,
                max_price = coupon.max_price,
                min_order_value = coupon.min_order_value,
                value = coupon.value,
                quantity = coupon.quantity,
                name = coupon.name,
                id = coupon.id,
                isActive = coupon.isActive
            };
            return new ApiSuccessResult<CouponViewModel>(couponn);
        }
        public async Task<ApiResult<CouponViewModel>> UseCoupon(string code, int cus_id)
        {
            var coupon = _context.Coupons.Where(x=> x.code.Equals(code)).FirstOrDefault();
            if (coupon == null)
            {
                return new ApiErrorResult<CouponViewModel>("Mã giảm giá không tồn tại");
            }
            var isUse = await _context.Orders.AnyAsync(x => x.cus_id == cus_id && x.coupon_id == coupon.id);
            if(isUse)
            {
                return new ApiErrorResult<CouponViewModel>("Bạn đã sử dụng mã này rồi");
            }

            var couponn = new CouponViewModel()
            {
                code = coupon.code,
                end_at = coupon.end_at,
                start_at = coupon.start_at,
                type = coupon.type,
                value = coupon.value,
                max_price = coupon.max_price,
                min_order_value = coupon.min_order_value,
                quantity = coupon.quantity,
                name = coupon.name,
                id = coupon.id,
                isActive = coupon.isActive
            };
            return new ApiSuccessResult<CouponViewModel>(couponn);
        }
        public async Task<ApiResult<bool>> Update(CouponUpdateRequest request)
        {
            try
            {
                var Coupon = await _context.Coupons.FindAsync(request.id);
                if (Coupon != null)
                {
                    Coupon.name = request.name;
                    Coupon.isActive = request.isActive;
                    Coupon.quantity = request.quantity;

                    if (!string.IsNullOrWhiteSpace(request.max_price))
                        Coupon.max_price = double.Parse(request.max_price);
                    else Coupon.max_price = null;

                    if (!string.IsNullOrWhiteSpace(request.min_order_value))
                        Coupon.min_order_value = double.Parse(request.min_order_value);
                    else Coupon.min_order_value = null;

                    Coupon.start_at = request.start_at;
                    Coupon.end_at = request.end_at;

                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy mã giảm giá này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<bool> isValidCode(int id, string code)
        {
            if (await _context.Coupons.AnyAsync(x => x.code.Equals(code) && x.id != id))
                return false;
            return true;
        }
    }
}
