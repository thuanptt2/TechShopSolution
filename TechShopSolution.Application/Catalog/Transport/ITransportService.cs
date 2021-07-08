using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Transport;

namespace TechShopSolution.Application.Catalog.Transport
{
    public interface ITransportService
    {
        Task<ApiResult<bool>> ChangeStatus(int id);
        Task<ApiResult<bool>> Create(TransporterCreateRequest request);
        Task<ApiResult<bool>> Delete(int id);
        Task<ApiResult<TransporterViewModel>> GetById(int id);
        Task<ApiResult<bool>> Update(TransporterUpdateRequest request);
        Task<List<TransporterViewModel>> GetAll();
        Task<PagedResult<TransporterViewModel>> GetAllPaging(GetTransporterPagingRequest request);
        Task<ApiResult<bool>> CreateShippingOrder(CreateTransportRequest request);
        Task<ApiResult<bool>> UpdateLadingCode(UpdateLadingCodeRequest request);
        Task<ApiResult<string>> CancelShippingOrder(int id);
    }
}
