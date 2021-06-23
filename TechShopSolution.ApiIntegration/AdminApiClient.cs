using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.System;

namespace TechShopSolution.ApiIntegration
{
    public class AdminApiClient : IAdminApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public AdminApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.PostAsync("https://localhost:5001/api/admin/authenticate", httpContent);
            var token = await respone.Content.ReadAsStringAsync();
            return token;
        }
        public async Task<string> AuthenticateCustomer(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.PostAsync("https://localhost:5001/api/admin/authenticatecustomer", httpContent);
            var token = await respone.Content.ReadAsStringAsync();
            return token;
        }
    }
}
