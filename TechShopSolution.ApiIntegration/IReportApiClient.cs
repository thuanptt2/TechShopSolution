using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Report;

namespace TechShopSolution.ApiIntegration
{
    public interface IReportApiClient
    {
        Task<ApiResult<List<RevenueReportViewModel>>> GetRevenueReport(GetRevenueRequest request);
        Task<ApiResult<List<RevenueByMonthReportViewModel>>> GetRevenueByMonthReport(GetRevenueRequest request);
    }
}
