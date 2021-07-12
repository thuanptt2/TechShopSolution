using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Sales;
using TechShopSolution.ViewModels.Transport;

namespace TechShopSolution.ApiIntegration
{
    public class TransportApiClient : ITransportApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public TransportApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<ApiResult<bool>> ChangeStatus(int Id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/transport/changestatus/{Id}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<bool>> CreateTransporter(TransporterCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();
            if (request.image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Image", request.image.FileName);
            }
            requestContent.Add(new StringContent(request.name.ToString()), "name");
            requestContent.Add(new StringContent(request.link.ToString()), "link");
            requestContent.Add(new StringContent(request.isActive.ToString()), "isActive");

            var respone = await client.PostAsync($"/api/transport", requestContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<bool>> CreateShippingOrder(CreateTransportRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respone = await client.PostAsync($"/api/transport/createshipping", httpContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<bool>> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.DeleteAsync($"/api/transport/{id}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<TransporterViewModel>> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/transport/{id}");
            var body = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<TransporterViewModel>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<TransporterViewModel>>(body);
        }
        public async Task<PagedResult<TransporterViewModel>> GetTransporterPagings(GetTransporterPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/transport/paging?Keyword={request.Keyword}&pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}");
            var body = await respone.Content.ReadAsStringAsync();
            var transporter = JsonConvert.DeserializeObject<PagedResult<TransporterViewModel>>(body);
            return transporter;
        }
        public async Task<List<TransporterViewModel>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/transport/all");
            var body = await respone.Content.ReadAsStringAsync();
            var transporter = JsonConvert.DeserializeObject<List<TransporterViewModel>>(body);
            return transporter;
        }
        public async Task<ApiResult<bool>> UpdateTransporter(TransporterUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();
            if (request.image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "image", request.image.FileName);
            }
            requestContent.Add(new StringContent(request.name.ToString()), "name");
            requestContent.Add(new StringContent(request.link.ToString()), "link");
            requestContent.Add(new StringContent(request.isActive.ToString()), "isActive");
            requestContent.Add(new StringContent(request.id.ToString()), "id");

            var respone = await client.PutAsync($"/api/transport", requestContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<bool>> UpdateLadingCode(UpdateLadingCodeRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var respone = await client.PutAsync($"/api/transport/newladingcode", httpContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<string>> CancelShippingOrder(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/transport/canceltransport/{id}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<string>>(result);
        }
        public async Task<PagedResult<TransportViewModel>> GetTransportPagings(GetTransportPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/transport/pagingtransport?Keyword={request.Keyword}&pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}");
            var body = await respone.Content.ReadAsStringAsync();
            var transporter = JsonConvert.DeserializeObject<PagedResult<TransportViewModel>>(body);
            return transporter;
        }
        public async Task<ApiResult<OrderDetailViewModel>> Detail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/transport/detail/{id}");
            var body = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<OrderDetailViewModel>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<OrderDetailViewModel>>(body);
        }
        public async Task<ApiResult<string>> ConfirmDoneShip(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/transport/ConfirmDoneShip/{id}");
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<string>>(result);
        }
    }
}
