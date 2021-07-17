using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Website.Contact;

namespace TechShopSolution.Application.Website.Contact
{
    public interface IContactService
    {
        Task<ApiResult<bool>> Update(ContactUpdateRequest request);
        ApiResult<ContactViewModel> GetContactData();
        Task<ApiResult<bool>> CreateFeedback(FeedbackCreateRequest request);
        Task<ApiResult<bool>> ChangeFeedbackStatus(int id);
        Task<ApiResult<FeedbackViewModel>> GetById(int id);
        Task<PagedResult<FeedbackViewModel>> GetFeedbackPaging(GetFeedbackPagingRequets request);
        Task<ApiResult<bool>> Delete(int id);
    }
}
