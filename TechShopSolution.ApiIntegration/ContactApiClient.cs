using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Website.Contact;

namespace TechShopSolution.ApiIntegration
{
    public class ContactApiClient : IContactApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public ContactApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<ApiResult<bool>> UpdateContact(ContactUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var requestContent = new MultipartFormDataContent();
            if (request.company_logo != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.company_logo.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.company_logo.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "company_logo", request.company_logo.FileName);
            }
            requestContent.Add(new StringContent(request.adress.ToString()), "adress");
            requestContent.Add(new StringContent(request.company_name.ToString()), "company_name");
            requestContent.Add(new StringContent(request.email.ToString()), "email");
            requestContent.Add(new StringContent(request.id.ToString()), "id");
            requestContent.Add(new StringContent(request.phone.ToString()), "phone");
            if (!string.IsNullOrEmpty(request.hotline))
            {
                requestContent.Add(new StringContent(request.hotline.ToString()), "hotline");
            }
            if (!string.IsNullOrEmpty(request.fax))
            {
                requestContent.Add(new StringContent(request.fax.ToString()), "fax");
            }
            if (!string.IsNullOrEmpty(request.social_fb))
            {
                requestContent.Add(new StringContent(request.social_fb.ToString()), "social_fb");
            }
            if (!string.IsNullOrEmpty(request.social_instagram))
            {
                requestContent.Add(new StringContent(request.social_instagram.ToString()), "social_instagram");
            }
            if (!string.IsNullOrEmpty(request.social_twitter))
            {
                requestContent.Add(new StringContent(request.social_twitter.ToString()), "social_twitter");
            }
            if (!string.IsNullOrEmpty(request.social_youtube))
            {
                requestContent.Add(new StringContent(request.social_youtube.ToString()), "social_youtube");
            }

            var respone = await client.PutAsync($"/api/Contact", requestContent);
            var result = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            else return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<ContactViewModel>> GetcontactInfos()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var respone = await client.GetAsync($"/api/contact");
            var body = await respone.Content.ReadAsStringAsync();
            if (respone.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<ContactViewModel>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<ContactViewModel>>(body);
        }
    }
}
