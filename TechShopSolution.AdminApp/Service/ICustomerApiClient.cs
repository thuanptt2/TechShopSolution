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
        Task<ApiResult<bool>> CreateCustomer(CustomerCreateRequest request);
        Task<bool> VerifyEmail(string email);
        Task<ApiResult<bool>> UpdateCustomer(int id, CustomerUpdateRequest request);
        Task<ApiResult<bool>> UpdateAddress(int id, CustomerUpdateAddressRequest request);
        Task<ApiResult<CustomerViewModel>> GetById(int id);
    }
}
