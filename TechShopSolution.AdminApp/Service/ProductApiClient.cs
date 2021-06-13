using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
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
        private readonly IHostingEnvironment _environment;

        public ProductApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
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
            if (request.Image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Image", request.Image.FileName);
            }

            var provider = new PhysicalFileProvider(_environment.WebRootPath);
            var contents = provider.GetDirectoryContents(Path.Combine("assets", "ProductImage"));
            var objFiles = contents.OrderBy(m => m.LastModified);
            foreach (var item in contents.ToList())
            {
                byte[] bytes = System.IO.File.ReadAllBytes(item.PhysicalPath);
                ByteArrayContent byteArr = new ByteArrayContent(bytes);
                requestContent.Add(byteArr, "More_images", item.Name);
                File.Delete(item.PhysicalPath);
            }
            request.Code = await GenerateCode();

            requestContent.Add(new StringContent(request.Best_seller.ToString()), "Best_seller");
            requestContent.Add(new StringContent("1"), "Brand_id");
            requestContent.Add(new StringContent("1"), "CateID");
            requestContent.Add(new StringContent(request.Code.ToString()), "Code");
            requestContent.Add(new StringContent(request.Featured.ToString()), "Featured");
            requestContent.Add(new StringContent(request.Instock.ToString()), "Instock");
            requestContent.Add(new StringContent(request.IsActive.ToString()), "IsActive");
            requestContent.Add(new StringContent(request.Name.ToString()), "Name");
            requestContent.Add(new StringContent(request.Promotion_price.ToString()), "Promotion_price");
            requestContent.Add(new StringContent(request.Slug.ToString()), "Slug");
            requestContent.Add(new StringContent(request.Unit_price.ToString()), "Unit_price");
            requestContent.Add(new StringContent(request.Warranty.ToString()), "Warranty");
            if (request.Specifications != null)
            {
                requestContent.Add(new StringContent(request.Specifications.ToString()), "Specifications");
            }
            if (request.Meta_descriptions != null)
            {
                requestContent.Add(new StringContent(request.Meta_descriptions.ToString()), "Meta_descriptions");
            }
            if (request.Descriptions != null)
            {
                requestContent.Add(new StringContent(request.Descriptions.ToString()), "Descriptions");
            }
            if (request.Meta_keywords != null)
            {
                requestContent.Add(new StringContent(request.Meta_keywords.ToString()), "Meta_keywords");
            }
            if (request.Meta_tittle != null)
            {
                requestContent.Add(new StringContent(request.Meta_tittle.ToString()), "Meta_tittle");
            }
            if (request.Short_desc != null)
            {
                requestContent.Add(new StringContent(request.Short_desc.ToString()), "Short_desc");
            }
            var respone = await client.PostAsync($"/api/product", requestContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<string> GenerateCode()
        {
            string Code = "";
            return Code = await Task.Run(() =>
            {
                string month = DateTime.Now.Month.ToString();
                if (month.Length == 1) month = "0" + month;
                string date = DateTime.Now.Day.ToString();
                if (date.Length == 1) date = "0" + date;
                string hour = DateTime.Now.Hour.ToString();
                if (hour.Length == 1) hour = "0" + hour;
                string minute = DateTime.Now.Minute.ToString();
                if (minute.Length == 1) minute = "0" + minute;
                string second = DateTime.Now.Second.ToString();
                if (second.Length == 1) second = "0" + second;
                return DateTime.Now.Year.ToString().Substring(2, 2) + month + date + hour + minute + second;
            });
        }
        public Task<ApiResult<bool>> Delete(int cusID)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> DeleteImage(int id, string fileName)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.DeleteAsync($"/api/Product/DeleteImage?id={id}&fileName={fileName}");
            var body = await respone.Content.ReadAsStringAsync();
            if ( respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }

        public async Task<ApiResult<ProductViewModel>> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/product/{id}");
            var body = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<ProductViewModel>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<ProductViewModel>>(body);
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

        public async Task<bool> isValidSlug(string slug)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/product?slug={slug}");
            return respone.IsSuccessStatusCode;
        }
        public async Task<List<ImageListResult>> GetImageByProductID(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/product/image/{id}");
            var body = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
            {
                var result =  JsonConvert.DeserializeObject<List<ImageListResult>>(body);
                return result;
            }
            else return null;
               
        }
    }
}
