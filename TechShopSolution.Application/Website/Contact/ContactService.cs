using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.Application.Common;
using TechShopSolution.Data.EF;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Website.Contact;
using System.Linq;

namespace TechShopSolution.Application.Website.Contact
{
    public class ContactService : IContactService
    {
        private readonly TechShopDBContext _context;
        private readonly IStorageService _storageService;
        public ContactService(TechShopDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public ApiResult<ContactViewModel> GetContactData()
        {
            var query = from ct in _context.Contacts
                        select ct;
           
            var infos = query.Select(a => new ContactViewModel()
            {
                adress = a.adress,
                company_name = a.company_name,
                company_logo = GetBase64StringForImage(_storageService.GetFileUrl(a.company_logo)),
                email = a.email,
                fax = a.fax,
                hotline = a.hotline,
                id = a.id,
                phone = a.phone,
                social_fb = a.social_fb,
                social_instagram = a.social_instagram,
                social_twitter = a.social_twitter,
                social_youtube = a.social_youtube,
            }).FirstOrDefault();
            return new ApiSuccessResult<ContactViewModel>(infos);
        }
        public async Task<ApiResult<bool>> Update(ContactUpdateRequest request)
        {
            try
            {
                var result = await _context.Contacts.FindAsync(request.id);
                if (result != null)
                {
                    if (request.company_logo != null)
                    {
                        await _storageService.DeleteFileAsync(result.company_logo);
                        result.company_logo = await this.SaveFile(request.company_logo);
                    }
                    result.adress = request.adress;
                    result.company_name = request.company_name;
                    result.email = request.email;
                    result.fax = request.fax;
                    result.hotline = request.hotline;
                    result.phone = request.phone;
                    result.social_fb = request.social_fb;
                    result.social_instagram = request.social_instagram;
                    result.social_twitter = request.social_twitter;
                    result.social_youtube = request.social_youtube;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Dữ liệu liên hệ trống");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        protected static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
        public async Task<ApiResult<bool>> CreateFeedback(FeedbackCreateRequest request)
        {
            try
            {
                var feedback = new TechShopSolution.Data.Entities.Feedback
                {
                    create_at = DateTime.Now,
                    content = request.content,
                    email = request.email,
                    isRead = false,
                    name = request.name,
                    phone = request.phone,
                    title = request.title
                };
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch
            {
                return new ApiErrorResult<bool>("Gửi feedback thất bại, quý khách vui lòng thử lại sau");
            }
        }
    }
}
