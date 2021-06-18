using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Category;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.ApiIntegration
{
    public interface ICategoryApiClient
    {
        Task<PagedResult<CategoryViewModel>> GetCategoryPagings(GetCategoryPagingRequest request);
        Task<ApiResult<bool>> CreateCategory(UpdateCategoryRequest request);
        Task<List<CategoryViewModel>> GetAllCategory();
        Task<ApiResult<bool>> UpdateCategory(UpdateCategoryRequest request);
        Task<bool> isValidSlug(int id, string slug);
        Task<ApiResult<CategoryViewModel>> GetById(int id);
        Task<ApiResult<bool>> Delete(int cateID);
        Task<ApiResult<bool>> ChangeStatus(int Id);
    }
}
