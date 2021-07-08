using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Transport;

namespace TechShopSolution.ApiIntegration
{
    public interface ITransportApiClient
    {
        Task<PagedResult<TransporterViewModel>> GetTransporterPagings(GetTransporterPagingRequest request);
        Task<ApiResult<bool>> CreateTransporter(TransporterCreateRequest request);
        Task<ApiResult<bool>> UpdateTransporter(TransporterUpdateRequest request);
        Task<ApiResult<bool>> ChangeStatus(int Id);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<TransporterViewModel>> GetById(int id);
        Task<List<TransporterViewModel>> GetAll();
        Task<ApiResult<bool>> CreateShippingOrder(CreateTransportRequest request);
        Task<ApiResult<bool>> UpdateLadingCode(UpdateLadingCodeRequest request);
        Task<ApiResult<string>> CancelShippingOrder(int id);
    }
}
