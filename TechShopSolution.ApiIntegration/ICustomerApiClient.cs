using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Location;
using TechShopSolution.ViewModels.Sales;

namespace TechShopSolution.ApiIntegration
{
    public interface ICustomerApiClient
    {
        Task<PagedResult<CustomerViewModel>> GetCustomerPagings(GetCustomerPagingRequest request);
        Task<ApiResult<bool>> CreateCustomer(CustomerCreateRequest request);
        Task<ApiResult<bool>> Register(CustomerRegisterRequest request);
        Task<bool> VerifyEmail(string email);
        Task<ApiResult<bool>> UpdateCustomer(CustomerUpdateRequest request);
        Task<ApiResult<bool>> UpdateCustomerPublic(CustomerPublicUpdateRequest request);
        Task<ApiResult<bool>> UpdateAddress(CustomerUpdateAddressRequest request);
        Task<ApiResult<bool>> ChangeStatus(int Id);
        Task<ApiResult<bool>> Delete(int cusID);
        Task<ApiResult<CustomerViewModel>> GetById(int id);
        Task<ApiResult<List<ProvinceModel>>> LoadProvince();
        Task<ApiResult<List<DistrictModel>>> LoadDistrict(int provinceID);
        Task<ApiResult<List<WardModel>>> LoadWard(int districtID);
        Task<List<OrderViewModel>> GetLatestOrder(int id, int take);
        Task<ApiResult<List<OrderPublicViewModel>>> GetCustomerOrders(int id);
        Task<ApiResult<OrderPublicViewModel>> GetOrderDetail(int id);
        Task<ApiResult<string>> ConfirmDoneShip(int id);
        Task<ApiResult<string>> CancelOrder(OrderCancelRequest request);

    }
}
