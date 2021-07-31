using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.Application.Dapper.Report
{
    public class ReportService
    {
        private readonly IConfiguration _configuration;
        public ReportService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ApiResult<IEnumerable<RevenueReportViewModel>>> GetReportAsync(string fromDate, string toDate)
        {
            using (var sqlConnection = new SqlConnection(_configuration.GetConnectionString("TechShopSolutionDB")))
            {
                await sqlConnection.OpenAsync();
                var dynamicParameters = new DynamicParameters();
                var now = DateTime.Now;

                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                dynamicParameters.Add("@fromDate", string.IsNullOrEmpty(fromDate) ? firstDayOfMonth.ToString("MM/dd/yyyy") : fromDate);
                dynamicParameters.Add("@toDate", string.IsNullOrEmpty(toDate) ? lastDayOfMonth.ToString("MM/dd/yyyy") : toDate);

                try
                {
                    var result = await sqlConnection.QueryAsync<RevenueReportViewModel>("GetRevenueDaily", dynamicParameters, 
                        commandType: System.Data.CommandType.StoredProcedure);
                    return new ApiSuccessResult<IEnumerable<RevenueReportViewModel>>(result);
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
        }

    }
}
