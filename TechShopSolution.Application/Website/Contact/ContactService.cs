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
using Microsoft.EntityFrameworkCore;

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
        public async Task<PagedResult<FeedbackViewModel>> GetFeedbackPaging(GetFeedbackPagingRequets request)
        {
            var query = from t in _context.Feedbacks
                        select t;

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.name.Contains(request.Keyword) || x.email.Contains(request.Keyword)
                || x.phone.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = query.OrderBy(m => m.create_at)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new FeedbackViewModel()
                {
                    id = a.id,
                    content = a.content,
                    create_at = a.create_at,
                    email = a.email,
                    isRead = a.isRead,
                    phone = a.phone,
                    name = a.name,
                    title = a.title,
                }).ToListAsync();


            var pageResult = new PagedResult<FeedbackViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = await data,
            };
            return pageResult;
        }
        public async Task<ApiResult<FeedbackViewModel>> GetById(int id)
        {
            var result = await _context.Feedbacks.FindAsync(id);
            if (result == null)
            {
                return new ApiErrorResult<FeedbackViewModel>("Feedback này không tồn tại");
            }
            var feedback = new FeedbackViewModel()
            {
                create_at = result.create_at,
                title = result.title,
                content = result.content,
                name = result.name,
                phone = result.phone,
                isRead = result.isRead,
                email = result.email,
                id = result.id,
            };
            return new ApiSuccessResult<FeedbackViewModel>(feedback);
        }
        public async Task<ApiResult<bool>> ChangeFeedbackStatus(int id)
        {
            try
            {
                var result = await _context.Feedbacks.FindAsync(id);
                if (result != null)
                {
                    if (result.isRead)
                        return new ApiErrorResult<bool>("Bạn đã đọc feedback này rồi");
                    else result.isRead = true;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy feedback này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> Delete(int id)
        {
            try
            {
                var result = await _context.Feedbacks.FindAsync(id);
                if (result != null)
                {
                    _context.Feedbacks.Remove(result);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>($"Feedback này không tồn tại");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }


    }
}
