using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Location;
using TechShopSolution.ViewModels.Sales;

namespace TechShopSolution.Application.Catalog.Customer
{
    public interface ICustomerService
    {
        Task<ApiResult<bool>> Create(CustomerCreateRequest request);
        Task<ApiResult<bool>> Register(CustomerRegisterRequest request);
        Task<ApiResult<bool>> Update(int id, CustomerUpdateRequest request);
        Task<ApiResult<bool>> UpdatePublic(int id, CustomerPublicUpdateRequest request);
        Task<ApiResult<bool>> UpdateAddress(int id, CustomerUpdateAddressRequest request);
        Task<ApiResult<bool>> ChangeStatus(int id);
        Task<ApiResult<bool>> Delete(int cusID);
        Task<ApiResult<CustomerViewModel>> GetById(int CustomerId);
        Task<PagedResult<CustomerViewModel>> GetAllPaging(GetCustomerPagingRequest request);
        Task<bool> VerifyEmail(string email);
        List<OrderViewModel> GetLatestOrder(int id, int take);
        Task<ApiResult<bool>> RatingProduct(ProductRatingRequest request);
        Task<ApiResult<bool>> FavoriteProduct(int cus_id, int product_id);
        Task<ApiResult<bool>> UnFavoriteProduct(int cus_id, int product_id);
        public PagedResult<ProductOverViewModel> GetFavoriteProduct(GetFavoriteProductsPagingRequest request);
    }
}
