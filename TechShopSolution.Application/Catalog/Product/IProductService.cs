using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Product;
using Microsoft.AspNetCore.Http;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Website.Dashboard;

namespace TechShopSolution.Application.Catalog.Product
{
    public interface IProductService
    {
        Task<ApiResult<bool>> Create(ProductCreateRequest request);
        Task<ApiResult<bool>> Update(ProductUpdateRequest request);
        Task<ApiResult<bool>> Delete(int productID);
        Task<ApiResult<bool>> DeleteImage(int id, string fileName);
        Task<ApiResult<ProductViewModel>> GetById(int id);
        Task<ApiResult<ProductViewModel>> GetPublicProductDetail(string slug, int? cus_id);
        PagedResult<ProductOverViewModel> GetAllPaging(GetProductPagingRequest request);
        PagedResult<ProductOverViewModel> GetPublicProducts(GetPublicProductPagingRequest request);
        List<RatingViewModel> GetRatingsProduct(string slug);
        Task<PublicCayegoyProductsViewModel> GetHomeProductByCategory(int id, int take);
        Task<bool> isValidSlug(string Code, string slug);
        Task<ApiResult<bool>> OffFeatured(int id);
        Task<ApiResult<bool>> OffBestSeller(int id);
        Task<ApiResult<bool>> ChangeStatus(int id);
        Task<PublicProductsViewModel> GetFeaturedProduct(int take);
        Task<PublicProductsViewModel> GetBestSellerProduct(int take);
        Task<PublicProductsViewModel> GetProductsByCategory(int id, int take);
        List<ProductOverViewModel> GetProductsRelated(int product_id, int take);
        List<ProductRankingViewModel> GetProductViewRanking(int take);
        List<ProductRankingViewModel> GetProductMostSalesRanking(int take);
        List<ProductRankingViewModel> GetProductFavoriteRanking(int take);
    }
}
