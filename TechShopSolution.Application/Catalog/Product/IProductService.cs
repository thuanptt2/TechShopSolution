using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Product;
using Microsoft.AspNetCore.Http;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.Application.Catalog.Product
{
    public interface IProductService
    {
        Task<ApiResult<bool>> Create(ProductCreateRequest request);
        Task<ApiResult<bool>> Update(ProductUpdateRequest request);
        Task<ApiResult<bool>> Delete(int productID);
        Task<ApiResult<bool>> DeleteImage(int id, string fileName);
        Task<ApiResult<ProductViewModel>> GetById(int id);
        Task<ApiResult<ProductViewModel>> GetBySlug(string slug);
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);
        Task<PagedResult<ProductViewModel>> GetAllPagingWithMainImage(GetProductPagingRequest request);
        Task<PagedResult<ProductViewModel>> GetPublicProducts(GetPublicProductPagingRequest request);
        Task<bool> isValidSlug(string Code, string slug);
        Task<List<ImageListResult>> GetImagesByProductID(int id);
        Task<ApiResult<bool>> OffFeatured(int id);
        Task<ApiResult<bool>> OffBestSeller(int id);
        Task<ApiResult<bool>> ChangeStatus(int id);
        Task<List<ProductViewModel>> GetFeaturedProduct(int take);
        Task<List<ProductViewModel>> GetBestSellerProduct(int take);
        Task<List<ProductViewModel>> GetProductsByCategory(int id, int take);
        Task<List<ProductViewModel>> GetProductsRelated(int id, int take);
    }
}
