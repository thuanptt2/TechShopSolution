using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Website;
using TechShopSolution.ViewModels.Website.Slide;

namespace TechShopSolution.ApiIntegration
{
    public interface ISlideApiClient
    {
        Task<ApiResult<bool>> ChangeStatus(int Id);
        Task<ApiResult<bool>> CreateSlide(SlideCreateRequest request);
        Task<ApiResult<bool>> DeleteSlide(int id);
        Task<ApiResult<SlideViewModel>> GetById(int id);
        Task<PagedResult<SlideViewModel>> GetSlidePagings(PagingRequestBase request);
        Task<List<SlideViewModel>> GetAll();
        Task<ApiResult<bool>> UpdateSlide(SlideUpdateRequest request);
        Task<ApiResult<bool>> DisplayOrder(int slide_id, int display_position);
    }
}
