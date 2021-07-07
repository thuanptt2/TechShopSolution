using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.Data.EF;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Transport;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TechShopSolution.Application.Common;

namespace TechShopSolution.Application.Catalog.Transport
{
    public class TransportService : ITransportService
    {
        private readonly TechShopDBContext _context;
        private readonly IStorageService _storageService;
        public TransportService(TechShopDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<ApiResult<bool>> ChangeStatus(int id)
        {
            try
            {
                var result = await _context.Transporters.FindAsync(id);
                if (result != null)
                {
                    if (result.isActive)
                        result.isActive = false;
                    else result.isActive = true;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy đơn vị vận chuyển này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> Create(TransporterCreateRequest request)
        {
            try
            {
                var transporter = new TechShopSolution.Data.Entities.Transporter
                {
                    create_at = DateTime.Now,
                    link = request.link,
                    isActive = request.isActive,
                    name = request.name,
                    image = await this.SaveFile(request.image),
                };
                _context.Transporters.Add(transporter);
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
                var result = await _context.Transporters.FindAsync(id);
                if (result != null)
                {
                    result.isDelete = true;
                    result.delete_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>($"Đơn vị vận chuyển này không tồn tại");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
        public async Task<ApiResult<TransporterViewModel>> GetById(int id)
        {
            var result = await _context.Transporters.FindAsync(id);
            if (result == null)
            {
                return new ApiErrorResult<TransporterViewModel>("Đơn vị vận chuyển này không tồn tại");
            }
            var transporter = new TransporterViewModel()
            {
                create_at = result.create_at,
                name = result.name,
                isActive = result.isActive,
                id = result.id,
                update_at = result.update_at,
                image = GetBase64StringForImage(_storageService.GetFileUrl(result.image)),
                link = result.link
            };
            return new ApiSuccessResult<TransporterViewModel>(transporter);
        }
        public async Task<ApiResult<bool>> Update(TransporterUpdateRequest request)
        {
            try
            {
                var result = await _context.Transporters.FindAsync(request.id);
                if (result != null)
                {
                    result.isActive = request.isActive;
                    result.name = request.name;
                    result.update_at = DateTime.Now;
                    result.link = request.link;
                    if(request.image != null)
                    {
                        await _storageService.DeleteFileAsync(result.image);
                        result.image = await this.SaveFile(request.image);
                    }

                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy phương thức đơn vị vận chuyển này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<List<TransporterViewModel>> GetAll()
        {
            var query = from t in _context.Transporters
                        where t.isDelete == false && t.isActive == true
                        select t;

            var data = query
                .Select(a => new TransporterViewModel()
                {
                    id = a.id,
                    name = a.name,
                    isActive = a.isActive,
                    create_at = a.create_at,
                    image = a.image,
                    link = a.link,
                    update_at = a.update_at,
                }).ToListAsync();

            return await data;
        }
        public async Task<PagedResult<TransporterViewModel>> GetAllPaging(GetTransporterPagingRequest request)
        {
            var query = from t in _context.Transporters
                        where t.isDelete == false
                        select t;

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.name.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = query.OrderByDescending(m => m.create_at)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new TransporterViewModel()
                {
                    id = a.id,
                    name = a.name,
                    isActive = a.isActive,
                    image = GetBase64StringForImage(_storageService.GetFileUrl(a.image)),
                    create_at = a.create_at,
                    link = a.link,
                }).ToListAsync();

           
            var pageResult = new PagedResult<TransporterViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = await data,
            };
            return pageResult;
        }
        protected static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
        public async Task<ApiResult<bool>> CreateShippingOrder(CreateTransportRequest request)
        {
            try
            {
                var transport = new TechShopSolution.Data.Entities.Transport
                {
                    create_at = DateTime.Now,
                    cod_price = request.cod_price,
                    lading_code = request.lading_code,
                    order_id = request.order_id,
                    ship_status = 1,
                    transporter_id = request.transporter_id,
                };
                _context.Transports.Add(transport);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch
            {
                return new ApiErrorResult<bool>("Thêm thất bại");
            }
        }

    }
}
