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
    }
}
