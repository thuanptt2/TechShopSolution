using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Category;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.AdminApp.Service
{
    public interface ICategoryApiClient
    {
        Task<PagedResult<CategoryViewModel>> GetCategoryPagings(GetCategoryPagingRequest request);
        Task<ApiResult<bool>> CreateCategory(UpdateCategoryRequest request);
    }
}
