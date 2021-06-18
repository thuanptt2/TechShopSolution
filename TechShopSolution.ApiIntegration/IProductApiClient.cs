using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Brand;
using TechShopSolution.ViewModels.Catalog.Category;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ApiIntegration
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetProductPagings(GetProductPagingRequest request);
        Task<ApiResult<bool>> CreateProduct(ProductCreateRequest request);
        Task<ApiResult<bool>> UpdateProduct(ProductUpdateRequest request);
        Task<ApiResult<bool>> ChangeStatus(int Id);
        Task<ApiResult<bool>> OffBestSeller(int Id);
        Task<ApiResult<bool>> OffFeautured(int Id);
        Task<ApiResult<bool>> Delete(int cusID);
        Task<ApiResult<bool>> DeleteImage(int id, string fileName);
        Task<ApiResult<ProductViewModel>> GetById(int id);
        Task<bool> isValidSlug(string Code, string slug);
        Task<List<ImageListResult>> GetImageByProductID(int id);
        Task<List<CategoryViewModel>> GetAllCategory();
        Task<List<BrandViewModel>> GetAllBrand();
        Task<List<ProductViewModel>> GetFeaturedProducts(int take);
        Task<List<ProductViewModel>> GetBestSellerProducts(int take);
        Task<List<ProductViewModel>> GetProductsByCategory(int id, int take);
    }
}
