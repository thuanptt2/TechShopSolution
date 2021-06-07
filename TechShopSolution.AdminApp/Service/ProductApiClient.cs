using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.AdminApp.Service
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<ApiResult<bool>> ChangeStatus(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> CreateProduct(ProductCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> Delete(int cusID)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<ProductViewModel>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<ProductViewModel>> GetProductPagings(GetProductPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var respone = await client.GetAsync($"/api/Product/paging?Keyword={request.Keyword}&CategoryID={request.CategoryID}&BrandID={request.BrandID}&pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}");
            var body = await respone.Content.ReadAsStringAsync();
            var customer = JsonConvert.DeserializeObject<PagedResult<ProductViewModel>>(body);
            return customer;
        }

        public Task<ApiResult<bool>> UpdateProduct(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
