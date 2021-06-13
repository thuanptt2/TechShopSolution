using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.AdminApp.Service
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetProductPagings(GetProductPagingRequest request);
        Task<ApiResult<bool>> CreateProduct(ProductCreateRequest request);
        Task<ApiResult<bool>> UpdateProduct(ProductUpdateRequest request);
        Task<ApiResult<bool>> ChangeStatus(int Id);
        Task<ApiResult<bool>> Delete(int cusID);
        Task<ApiResult<bool>> DeleteImage(int id, string fileName);
        Task<ApiResult<ProductViewModel>> GetById(int id);
        Task<bool> isValidSlug(string slug);
        Task<List<ImageListResult>> GetImageByProductID(int id);
    }
}
