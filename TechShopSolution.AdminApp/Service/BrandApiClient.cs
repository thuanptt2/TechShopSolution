using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Brand;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.AdminApp.Service
{
    public class BrandApiClient : IBrandApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public BrandApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<ApiResult<bool>> ChangeStatus(int Id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/brand/changestatus/{Id}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> CreateBrand(BrandCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respone = await client.PostAsync($"/api/brand", httpContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> Delete(int brandID)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.DeleteAsync($"/api/brand/{brandID}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<BrandViewModel>> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/brand/{id}");
            var body = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<BrandViewModel>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<BrandViewModel>>(body);
        }

        public async Task<PagedResult<BrandViewModel>> GetBrandPagings(GetBrandPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/brand/paging?Keyword={request.Keyword}&pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}");
            var body = await respone.Content.ReadAsStringAsync();
            var brand = JsonConvert.DeserializeObject<PagedResult<BrandViewModel>>(body);
            return brand;
        }

        public async Task<bool> isValidSlug(int id, string slug)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/brand?slug={slug}&id={id}");
            return respone.IsSuccessStatusCode;
        }

        public async Task<ApiResult<bool>> UpdateBrand(BrandUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respone = await client.PutAsync($"/api/brand/{request.id}", httpContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
    }
}
