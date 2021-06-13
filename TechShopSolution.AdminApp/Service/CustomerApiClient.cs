using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Location;

namespace TechShopSolution.AdminApp.Service
{
    public class CustomerApiClient : ICustomerApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public CustomerApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ApiResult<bool>> CreateCustomer(CustomerCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respone = await client.PostAsync($"/api/Customer",httpContent);
            var result = await respone.Content.ReadAsStringAsync();
            if(respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<bool>> Delete(int cusID)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.DeleteAsync($"/api/customer/{cusID}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<CustomerViewModel>> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/customer/{id}");
            var body = await respone.Content.ReadAsStringAsync();
            if(respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<CustomerViewModel>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<CustomerViewModel>>(body);
        }
        public async Task<PagedResult<CustomerViewModel>> GetCustomerPagings(GetCustomerPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/customer/paging?Keyword={request.Keyword}&pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}");
            var body = await respone.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<PagedResult<CustomerViewModel>>(body);
            return customer;
        }
        public async Task<ApiResult<bool>> UpdateAddress(CustomerUpdateAddressRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respone = await client.PutAsync($"/api/Customer/UpdateAddress/{request.Id}", httpContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<bool>> ChangeStatus(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/customer/changestatus/{id}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<bool>> UpdateCustomer(CustomerUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respone = await client.PutAsync($"/api/Customer/{request.Id}", httpContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<bool> VerifyEmail(string email)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/customer?email={email}");
            return respone.IsSuccessStatusCode;
        }
        public async Task<ApiResult<List<ProvinceModel>>> LoadProvince()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/Location/LoadProvince");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<ProvinceModel>>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<List<ProvinceModel>>>(result);
        }
        public async Task<ApiResult<List<DistrictModel>>> LoadDistrict(int provinceID)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/Location/LoadDistrict?provinceID={provinceID}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<DistrictModel>>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<List<DistrictModel>>>(result);
        }
        public async  Task<ApiResult<List<WardModel>>> LoadWard(int districtID)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/Location/LoadWard?districtID={districtID}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<List<WardModel>>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<List<WardModel>>>(result);
        }
    }
}
