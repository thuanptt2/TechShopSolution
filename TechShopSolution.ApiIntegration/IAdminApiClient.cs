using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.System;

namespace TechShopSolution.ApiIntegration
{
    public interface IAdminApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<ApiResult<string>> AuthenticateCustomer(LoginRequest request);
    }
}
