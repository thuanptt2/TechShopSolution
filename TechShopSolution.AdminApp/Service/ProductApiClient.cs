using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<ApiResult<bool>> CreateProduct(ProductCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var requestContent = new MultipartFormDataContent();
            if(request.Image!=null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Image", request.Image.FileName);
            }

            //if (request.More_images != null)
            //{
            //    byte[] data;
            //    using (var br = new BinaryReader(img.OpenReadStream()))
            //    {
            //        data = br.ReadBytes((int)img.OpenReadStream().Length);
            //    }
            //    ByteArrayContent bytes = new ByteArrayContent(data);
            //    requestContent.Add(bytes, "More_Image", img.FileName);
            //}

            requestContent.Add(new StringContent(request.Best_seller.ToString()), "Best_seller");
            requestContent.Add(new StringContent(request.Brand_id.ToString()), "Brand_id");
            requestContent.Add(new StringContent(request.Code.ToString()), "Code");
            requestContent.Add(new StringContent(request.Descriptions.ToString()), "Descriptions");
            requestContent.Add(new StringContent(request.Featured.ToString()), "Featured");
            requestContent.Add(new StringContent(request.Instock.ToString()), "Instock");
            requestContent.Add(new StringContent(request.IsActive.ToString()), "IsActive");
            requestContent.Add(new StringContent(request.Meta_descriptions.ToString()), "Meta_descriptions");
            requestContent.Add(new StringContent(request.Meta_keywords.ToString()), "Meta_keywords");
            requestContent.Add(new StringContent(request.Meta_tittle.ToString()), "Meta_tittle");
            requestContent.Add(new StringContent(request.Name.ToString()), "Name");
            requestContent.Add(new StringContent(request.Promotion_price.ToString()), "Promotion_price");
            requestContent.Add(new StringContent(request.Short_desc.ToString()), "Short_desc");
            requestContent.Add(new StringContent(request.Slug.ToString()), "Slug");
            requestContent.Add(new StringContent(request.Specifications.ToString()), "Specifications");
            requestContent.Add(new StringContent(request.Unit_price.ToString()), "Unit_price");
            requestContent.Add(new StringContent(request.Warranty.ToString()), "Warranty");
            var respone = await client.PostAsync($"/api/product", requestContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
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
            var product = JsonConvert.DeserializeObject<PagedResult<ProductViewModel>>(body);
            return product;
        }

        public Task<ApiResult<bool>> UpdateProduct(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
