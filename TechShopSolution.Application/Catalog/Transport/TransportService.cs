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
using TechShopSolution.ViewModels.Sales;

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
                    if (await _context.Transports.AnyAsync(x => x.transporter_id == id))
                    {
                        result.isDelete = true;
                        result.delete_at = DateTime.Now;
                    }
                    else
                    {
                        await _storageService.DeleteFileAsync(result.image);
                        _context.Transporters.Remove(result);
                    }
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
                    image = a.image,
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
                image = result.image,
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
                else return new ApiErrorResult<bool>("Không tìm thấy đơn vị vận chuyển này");
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
       
        public async Task<ApiResult<OrderDetailViewModel>> Detail(int id)
        {
            var query = from tp in _context.Transports
                        join o in _context.Orders on tp.order_id equals o.id
                        join od in _context.OrDetails on o.id equals od.order_id
                        join p in _context.Products on od.product_id equals p.id
                        join t in _context.Transporters on tp.transporter_id equals t.id
                        where tp.id == id
                        select new { p, o, od, tp };


            TransportViewModel transport = query.Select(a => new TransportViewModel()
            {
                cod_price = a.tp.cod_price,
                ship_status = a.tp.ship_status,
                order_id = a.tp.order_id,
                id = a.tp.id,
                create_at = a.tp.create_at,
                from_address = a.tp.from_address,
                done_at = a.tp.done_at,
                lading_code = a.tp.lading_code,
                to_address = a.tp.to_address,
                transporter_id = a.tp.transporter_id,
                transporter_name = a.tp.Transporter.name,
                update_at = a.tp.update_at,
                cancel_at = a.tp.cancel_at,

            }).FirstOrDefault();

            if (transport == null)
            {
                return new ApiErrorResult<OrderDetailViewModel>("Đơn vận chuyển này không tồn tại");
            }

            OrderModel DataOrder = query.Select(a => new OrderModel()
            {
                id = a.o.id,
                create_at = a.o.create_at,
                cus_id = a.o.cus_id,
                name_receiver = a.o.name_receiver,
                discount = a.o.discount,
                isPay = a.o.isPay,
                status = a.o.status,
                note = a.o.note,
                cancel_reason = a.o.cancel_reason,
                cancel_at = a.o.cancel_at,
                update_at = a.o.cancel_at,
                address_receiver = a.o.address_receiver,
                coupon_id = a.o.coupon_id,
                payment_id = a.o.payment_id,
                phone_receiver = a.o.phone_receiver,
                total = a.o.total,
                transport_fee = a.o.transport_fee

            }).FirstOrDefault();

            var customer = await _context.Customers.FindAsync(DataOrder.cus_id);
            DataOrder.cus_name = customer.name;
            DataOrder.cus_email = customer.email;
            DataOrder.cus_phone = customer.phone;
            DataOrder.cus_address = customer.address;

            List<OrderDetailModel> Details = query.Select(a => new OrderDetailModel()
            {
                order_id = a.od.order_id,
                product_id = a.od.product_id,
                product_image = a.p.image,
                product_name = a.p.name,
                promotion_price = a.od.promotion_price,
                quantity = a.od.quantity,
                unit_price = a.od.unit_price
            }).ToList();

            var model = new OrderDetailViewModel();
            model.Order = DataOrder;
            model.Transport = transport;
            model.Details = Details;
            return new ApiSuccessResult<OrderDetailViewModel>(model);
        }
        public async Task<ApiResult<bool>> UpdateLadingCode(UpdateLadingCodeRequest request)
        {
            try
            {
                var result = await _context.Transports.Where(x=>x.id == request.Id).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.lading_code = request.New_LadingCode;
                    result.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy đơn vận chuyển" + request.Id);
            }
            catch
            {
                return new ApiErrorResult<bool>("Thêm mã vận đơn mới thất bại, hãy thử lại sau.");
            }
        }
        public async Task<PagedResult<TransportViewModel>> GetPagingTransport(GetTransportPagingRequest request)
        {
            var query = from t in _context.Transports
                        select t;

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.id.ToString().Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = query.OrderByDescending(m => m.create_at)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new TransportViewModel()
                {
                    id = a.id,
                    order_id = a.order_id,
                    transporter_id = a.transporter_id,
                    transporter_name = a.Transporter.name,
                    ship_status = a.ship_status,
                    cod_price = a.cod_price,
                    create_at = a.create_at
                }).ToListAsync();


            var pageResult = new PagedResult<TransportViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = await data,
            };
            return pageResult;
        }
        public async Task<ApiResult<bool>> CreateShippingOrder(CreateTransportRequest request)
        {
            try
            {
                var transport = new TechShopSolution.Data.Entities.Transport
                {
                    create_at = DateTime.Now,
                    cod_price = !string.IsNullOrWhiteSpace(request.cod_price) ? decimal.Parse(request.cod_price) : 0,
                    lading_code = request.lading_code,
                    order_id = request.order_id,
                    ship_status = 1,
                    transporter_id = request.transporter_id,
                };
                if (request.lading_code != null)
                {
                    transport.update_at = DateTime.Now;
                }
                _context.Transports.Add(transport);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch
            {
                return new ApiErrorResult<bool>("Tạo đơn vận chuyển thất bại, vui lòng thử lại");
            }
        }
        public async Task<ApiResult<string>> CancelShippingOrder(int id)
        {
            var transport = await _context.Transports.FindAsync(id);
            if (transport == null)
                return new ApiErrorResult<string>("Không tìm thấy đơn vận chuyển này trong CSDL");
            if(transport.ship_status == 2)
            {
                return new ApiErrorResult<string>("Đơn hàng này đã được vận chuyển thành công, không thể hủy");
            }
            else
            {
                transport.ship_status = -1;
                transport.cancel_at = DateTime.Now;
                _context.SaveChanges();
                return new ApiSuccessResult<string>("Hủy giao hàng thành công");
            }
        }
        public async Task<ApiResult<string>> ConfirmDoneShip(int id)
        {
            var transport = await _context.Transports.FindAsync(id);
            if (transport == null)
                return new ApiErrorResult<string>("Không tìm thấy đơn vận chuyển này trong CSDL");
            if (transport.ship_status == 2)
                return new ApiErrorResult<string>("Đơn vận chuyển này đã hoàn thành");
            if (transport.ship_status == -1)
                return new ApiErrorResult<string>("Đơn vận chuyển này đã bị hủy, không thể làm điều này");

            transport.ship_status = 2;
            transport.done_at = DateTime.Now;
            _context.SaveChanges();
            return new ApiSuccessResult<string>("Xác nhận thành công");
        }

    }
}
