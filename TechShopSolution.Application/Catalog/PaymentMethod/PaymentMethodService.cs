using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.Data.EF;
using TechShopSolution.ViewModels.Catalog.Coupon;
using System.Linq;
using TechShopSolution.ViewModels.Catalog.PaymentMethod;
using TechShopSolution.ViewModels.Common;
using Microsoft.EntityFrameworkCore;

namespace TechShopSolution.Application.Catalog.PaymentMethod
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly TechShopDBContext _context;
        public PaymentMethodService(TechShopDBContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> ChangeStatus(int id)
        {
            try
            {
                var payment = await _context.PaymentMethods.FindAsync(id);
                if (payment != null)
                {
                    if (payment.isActive)
                        payment.isActive = false;
                    else payment.isActive = true;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy phương thức này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> Create(PaymentCreateRequest request)
        {
            try
            {
                var payment = new TechShopSolution.Data.Entities.PaymentMethod
                {
                    create_at = DateTime.Now,
                    description = request.description,
                    isActive = request.isActive,
                    name = request.name,
                };
                _context.PaymentMethods.Add(payment);
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
                var payment = await _context.PaymentMethods.FindAsync(id);
                if (payment != null)
                {
                    payment.isDelete = true;
                    payment.delete_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>($"Phương thức thanhh toán này không tồn tại");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
        public async Task<ApiResult<PaymentViewModel>> GetById(int id)
        {
            var payment = await _context.PaymentMethods.FindAsync(id);
            if (payment == null)
            {
                return new ApiErrorResult<PaymentViewModel>("Phương thức này không tồn tại");
            }
            var paymentt = new PaymentViewModel()
            {
                create_at = payment.create_at,
                name = payment.name,
                isActive = payment.isActive,
                isDelete = payment.isDelete,
                id = payment.id,
                delete_at = payment.delete_at,
                description = payment.description,
                update_at = payment.update_at
            };
            return new ApiSuccessResult<PaymentViewModel>(paymentt);
        }
        public async Task<ApiResult<bool>> Update(PaymentUpdateRequest request)
        {
            try
            {
                var payment = await _context.PaymentMethods.FindAsync(request.id);
                if (payment != null)
                {
                    payment.description = request.description;
                    payment.isActive = request.isActive;
                    request.name = request.name;
                    payment.update_at = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy phương thức này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<List<PaymentViewModel>> GetAll()
        {
            var query = from payment in _context.PaymentMethods
                        where payment.isDelete == false
                        select payment;

            var data = query
                .Select(a => new PaymentViewModel()
                {
                    id = a.id,
                    name = a.name,
                    isActive = a.isActive,
                    create_at = a.create_at,
                    delete_at = a.delete_at,
                    isDelete = a.isDelete,
                    description = a.description,
                    update_at = a.update_at,
                }).ToListAsync();

            return await data;

        }
        public async Task<PagedResult<PaymentViewModel>> GetAllPaging(GetPaymentPagingRequest request)
        {
            var query = from payment in _context.PaymentMethods
                        where payment.isDelete == false
                        select payment;

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.name.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = query.OrderByDescending(m => m.create_at)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new PaymentViewModel()
                {
                    id = a.id,
                    name = a.name,
                    isActive = a.isActive,
                    create_at = a.create_at,
                    delete_at = a.delete_at,
                    isDelete = a.isDelete,
                    description = a.description,
                    update_at = a.update_at,
                }).ToListAsync();

            var pageResult = new PagedResult<PaymentViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = await data,
            };
            return pageResult;
        }
    }
}
