using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.Application.DTO;
using Microsoft.AspNetCore.Http;

namespace TechShopSolution.Application.Catalog.Product
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productID);
        Task<bool> UpdatePrice(int productID, decimal newPrice, decimal newPromotionPrice);
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetPublicProductPagingRequest request);
        Task<int> AddImages(int productId, IFormFile file);
        Task<int> RemoveImages(int productId, IFormFile file);
        Task<int> UpdateImage(int productId, IFormFile file);
    }
}
