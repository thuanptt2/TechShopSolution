using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.PaymentMethod;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ApiIntegration
{
    public interface IPaymentApiClient
    {
        Task<PagedResult<PaymentViewModel>> GetPaymentPagings(GetPaymentPagingRequest request);
        Task<ApiResult<bool>> CreatePayment(PaymentCreateRequest request);
        Task<ApiResult<bool>> UpdatePayment(PaymentUpdateRequest request);
        Task<ApiResult<bool>> ChangeStatus(int Id);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<PaymentViewModel>> GetById(int id);
        Task<List<PaymentViewModel>> GetAll();
    }
}
