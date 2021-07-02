using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.PaymentMethod;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.Application.Catalog.PaymentMethod
{
    public interface IPaymentMethodService
    {
        Task<ApiResult<bool>> ChangeStatus(int id);
        Task<ApiResult<bool>> Create(PaymentCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<PaymentViewModel>> GetById(int id);
        Task<ApiResult<bool>> Update(PaymentUpdateRequest request);
        Task<PagedResult<PaymentViewModel>> GetAllPaging(GetPaymentPagingRequest request);
    }
}
