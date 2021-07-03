using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Sales;

namespace TechShopSolution.ApiIntegration
{
    public class OrderApiClient : IOrderApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public OrderApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<ApiResult<string>> CreateOrder(CheckoutRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respone = await client.PostAsync($"/api/order", httpContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<string>>(result);
        }
    }
}
