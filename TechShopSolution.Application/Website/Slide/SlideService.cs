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
using TechShopSolution.ViewModels.Website;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TechShopSolution.ViewModels.Website.Slide;

namespace TechShopSolution.Application.Website.Slide
{
    public class SlideService : ISlideService
    {
        private readonly TechShopDBContext _context;
        private readonly IStorageService _storageService;
        public SlideService(TechShopDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<ApiResult<bool>> ChangeStatus(int id)
        {
            try
            {
                var result = await _context.Slides.FindAsync(id);
                if (result != null)
                {
                    if (result.status)
                        result.status = false;
                    else result.status = true;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy Slide này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> Create(SlideCreateRequest request)
        {
            try
            {
                var slides = await _context.Slides.ToListAsync();
                var slide = new TechShopSolution.Data.Entities.Slide
                {
                    create_at = DateTime.Now,
                    link = request.link,
                    status = request.status,
                    display_order = slides.Count() + 1,
                    image = await this.SaveFile(request.image),
                };
                _context.Slides.Add(slide);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch
            {
                return new ApiErrorResult<bool>("Thêm thất bại");
            }
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public async Task<ApiResult<bool>> Delete(int id)
        {
            try
            {
                var result = await _context.Slides.FindAsync(id);
                if (result != null)
                {
                    if (result.image != null)
                    {
                        await _storageService.DeleteFileAsync(result.image);
                    }
                    var slides = await _context.Slides.OrderBy(x => x.display_order).ToListAsync();
                    var quantity = slides.Count();
                    for (int i = 0; i < slides.Count; i++)
                    {
                        if (slides[i].id == id)
                        {
                            for (int j = i + 1; j < quantity; j++)
                            {
                                slides[j].display_order = slides[j].display_order - 1;
                            }
                        }
                    }
                    _context.Slides.Remove(result);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>($"Slide này không tồn tại");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
        public async Task<PagedResult<SlideViewModel>> GetAllPaging(PagingRequestBase request)
        {
            var query = from t in _context.Slides
                        select t;

            int totalRow = await query.CountAsync();

            var data = query.OrderBy(m => m.display_order)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new SlideViewModel()
                {
                    id = a.id,
                    display_order = a.display_order,
                    status = a.status,
                    image = a.image,
                    create_at = a.create_at,
                    link = a.link,
                    update_at = a.update_at,
                }).ToListAsync();


            var pageResult = new PagedResult<SlideViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = await data,
            };
            return pageResult;
        }
        public async Task<ApiResult<SlideViewModel>> GetById(int id)
        {
            var result = await _context.Slides.FindAsync(id);
            if (result == null)
            {
                return new ApiErrorResult<SlideViewModel>("Slide này không tồn tại");
            }
            var slide = new SlideViewModel()
            {
                create_at = result.create_at,
                display_order = result.display_order,
                status = result.status,
                id = result.id,
                update_at = result.update_at,
                image = result.image,
                link = result.link
            };
            return new ApiSuccessResult<SlideViewModel>(slide);
        }
        public async Task<ApiResult<bool>> Update(SlideUpdateRequest request)
        {
            try
            {
                var result = await _context.Slides.FindAsync(request.id);
                if (result != null)
                {
                    result.status = request.status;
                    result.update_at = DateTime.Now;
                    result.link = request.link;
                    if(result.display_order != request.display_order)
                    {
                        await DisplayOrder(result.id, request.display_order);
                    }
                    if (request.image != null)
                    {
                        await _storageService.DeleteFileAsync(result.image);
                        result.image = await this.SaveFile(request.image);
                    }

                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy Slide này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task DisplayOrder(int slide_id, int display_position)
        {
            var slides = await _context.Slides.OrderBy(x=>x.display_order).ToListAsync();
            var quantity = slides.Count();
            for (int i = 0; i < slides.Count; i++)
            {
                if (slides[i].id == slide_id)
                {
                    if (slides[i].display_order > display_position)
                    {
                        for (  int j = display_position - 1; j < i; j++)
                        {
                            slides[j].display_order = slides[j].display_order + 1;
                        }
                        slides[i].display_order = display_position;
                    }
                    else if (slides[i].display_order < display_position)
                    {
                        if (display_position > quantity)
                            display_position = quantity;
                        for (int j = i; j < display_position; j++)
                        {
                            slides[j].display_order = slides[j].display_order - 1;
                        }
                        slides[i].display_order = display_position;
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task<List<SlideViewModel>> GetPublicSlide()
        {
            var query = from s in _context.Slides
                        where s.status == true
                        select s;

            var data = query.OrderBy(x=> x.display_order)
                .Select(a => new SlideViewModel()
                {
                    id = a.id,
                    display_order = a.display_order,
                    status = a.status,
                    create_at = a.create_at,
                    image = a.image,
                    link = a.link,
                    update_at = a.update_at,
                }).ToListAsync();

            return await data;
        }
    }
}
