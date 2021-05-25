using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.System;

namespace TechShopSolution.AdminApp.Service
{
    public interface IAdminApiClient
    {
        Task<string> Authenticate(LoginRequest request);
    }
}
