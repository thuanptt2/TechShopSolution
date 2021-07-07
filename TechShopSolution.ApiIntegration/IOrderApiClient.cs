using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Sales;

namespace TechShopSolution.ApiIntegration
{
    public interface IOrderApiClient
    {
        Task<ApiResult<string>> CreateOrder(CheckoutRequest request);
        Task<PagedResult<OrderViewModel>> GetOrderPagings(GetOrderPagingRequest request);
        Task<ApiResult<OrderDetailViewModel>> GetById(int id);
        Task<ApiResult<string>> PaymentConfirm(int id);
        Task<ApiResult<string>> CancelOrder(int id);
        Task<ApiResult<string>> ConfirmOrder(int id);
        Task<ApiResult<bool>> UpdateAddress(OrderUpdateAddressRequest request);
    }
}
