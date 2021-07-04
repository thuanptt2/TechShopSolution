using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Sales;

namespace TechShopSolution.Application.Catalog.Order
{
    public interface IOrderService
    {
        Task<ApiResult<string>> Create(CheckoutRequest request);
        PagedResult<OrderViewModel> GetAllPaging(GetOrderPagingRequest request);
    }
}
