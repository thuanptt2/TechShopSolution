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
        Task<ApiResult<bool>> CreateOrder(CheckoutRequest request);
    }
}
