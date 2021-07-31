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
        Task<ApiResult<IEnumerable<RevenueReportViewModel>>> GetReportAsync(string fromDate, string toDate);
    }
}
