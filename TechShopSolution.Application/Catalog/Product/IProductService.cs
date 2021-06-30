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
        PagedResult<ProductViewModel> GetAllPaging(GetProductPagingRequest request);
        PagedResult<ProductViewModel> GetAllPagingWithMainImage(GetProductPagingRequest request);
        PagedResult<ProductViewModel> GetPublicProducts(GetPublicProductPagingRequest request);
        Task<bool> isValidSlug(string Code, string slug);
        Task<List<ImageListResult>> GetImagesByProductID(int id);
        Task<ApiResult<bool>> OffFeatured(int id);
        Task<ApiResult<bool>> OffBestSeller(int id);
        Task<ApiResult<bool>> ChangeStatus(int id);
        Task<PublicProductsViewModel> GetFeaturedProduct(int take);
        Task<PublicProductsViewModel> GetBestSellerProduct(int take);
        Task<PublicProductsViewModel> GetProductsByCategory(int id, int take);
        List<ProductViewModel> GetProductsRelated(int idBrand, int take);
    }
}
