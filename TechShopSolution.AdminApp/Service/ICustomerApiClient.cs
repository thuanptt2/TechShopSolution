using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.AdminApp.Service
{
    public interface ICustomerApiClient
    {
        Task<PagedResult<CustomerViewModel>> GetCustomerPagings(GetCustomerPagingRequest request);
        Task<bool> CreateCustomer(CustomerCreateRequest request);
    }
}
