using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.Application.Catalog.Customer
{
    public interface ICustomerService
    {
        Task<ApiResult<bool>> Create(CustomerCreateRequest request);
        Task<ApiResult<bool>> Update(int id, CustomerUpdateRequest request);
        Task<ApiResult<bool>> UpdateAddress(int id, CustomerUpdateAddressRequest request);
        Task<ApiResult<bool>> Delete(int cusID);
        Task<ApiResult<CustomerViewModel>> GetById(int CustomerId);
        Task<PagedResult<CustomerViewModel>> GetAllPaging(GetCustomerPagingRequest request);
        Task<bool> VerifyEmail(string email);
    }
}
