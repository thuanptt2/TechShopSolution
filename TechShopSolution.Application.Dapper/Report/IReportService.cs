using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.Application.Dapper.Report
{
    public interface IReportService
    {
        Task<ApiResult<List<RevenueReportViewModel>>> GetReportAsync(string fromDate, string toDate);
        Task<ApiResult<List<RevenueByMonthReportViewModel>>> GetReportByMonthAsync(string fromDate, string toDate);
    }
}
