using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Report;

namespace TechShopSolution.ApiIntegration
{
    public class ReportApiClient : IReportApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public ReportApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<ApiResult<List<RevenueReportViewModel>>> GetRevenueReport(GetRevenueRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respone = await client.GetAsync($"/api/report/Revenue?FromDate={request.fromDate}&ToDate={request.toDate}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<RevenueReportViewModel>>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<List<RevenueReportViewModel>>>(result);
        }
    }
}
