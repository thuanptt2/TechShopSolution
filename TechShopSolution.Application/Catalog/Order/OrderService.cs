using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.Data.EF;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Sales;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using TechShopSolution.Application.Common;

namespace TechShopSolution.Application.Catalog.Order
{
    public class OrderService : IOrderService
    {
        private readonly TechShopDBContext _context;
        private readonly IStorageService _storageService;
        public OrderService(TechShopDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<ApiResult<string>> Create(CheckoutRequest request)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(request.Order.cus_id);
                if (customer.address == "")
                    customer.address = request.Order.address_receiver;
                var coupon = await _context.Coupons.FindAsync(request.Order.coupon_id);
                if (coupon != null)
                {
                    if (coupon.quantity != null)
                    {
                        if (coupon.quantity == 0)
                            return new ApiErrorResult<string>("Mã giảm giá bạn sử dụng đã được dùng hết");
                        else coupon.quantity = coupon.quantity - 1;
                    }
                }
                var order = new TechShopSolution.Data.Entities.Order
                {
                    address_receiver = request.Order.address_receiver,
                    coupon_id = request.Order.coupon_id,
                    create_at = DateTime.Now,
                    transport_fee = request.Order.transport_fee == null ? 0 : (decimal)request.Order.transport_fee,
                    cus_id = request.Order.cus_id,
                    discount = request.Order.discount,
                    isPay = false,
                    payment_id = (int)request.Order.payment_id,
                    name_receiver = request.Order.name_receiver,
                    note = request.Order.note,
                    phone_receiver = request.Order.phone_receiver,
                    status = 0,
                    total = request.Order.total,
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in request.OrderDetails)
                {
                    var detail = new TechShopSolution.Data.Entities.OrderDetail
                    {
                        order_id = order.id,
                        product_id = item.product_id,
                        promotion_price = item.promotion_price,
                        quantity = item.quantity,
                        unit_price = item.unit_price,
                    };
                    _context.OrDetails.Add(detail);
                    var product = await _context.Products.FindAsync(item.product_id);
                    if (product != null)
                    {
                        if (product.instock != null)
                        {
                            if (product.instock == 0)
                                return new ApiErrorResult<string>("Một sản phẩm trong giỏ hàng của bạn đã hết hạn vui lòng bỏ sản phẩm ra khỏi giỏ hàng.");
                            else product.instock = product.instock - item.quantity;
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<string>(order.id.ToString());
            }
            catch
            {
                return new ApiErrorResult<string>("Tạo đơn đặt hàng thất bại, quý khách vui lòng thử lại sau");
            }
        }
        public PagedResult<OrderViewModel> GetAllPaging(GetOrderPagingRequest request)
        {
            var query = from o in _context.Orders
                        join od in _context.OrDetails on o.id equals od.order_id
                        select new { o , od};

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.o.id.ToString().Contains(request.Keyword) || x.o.name_receiver.Contains(request.Keyword));
            }


            var data = query.AsEnumerable()
                   .GroupBy(g => g.o);

            int totalRow = data.Count();

            List<OrderViewModel> result = data.OrderByDescending(x => x.Key.create_at)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new OrderViewModel()
                {
                    id = a.Key.id,
                    create_at = a.Key.create_at,
                    cus_id = a.Key.cus_id,
                    name_receiver = a.Key.name_receiver,
                    discount = a.Key.discount,
                    isPay = a.Key.isPay,
                    status = a.Key.status,
                    total = a.Key.total,
                    transport_fee = a.Key.transport_fee

                }).ToList();

            foreach(var item in result)
            {
                var tranport = _context.Transports.Where(x => x.order_id == item.id).FirstOrDefault();
                if (tranport != null)
                {
                    item.ship_status = tranport.ship_status;
                }
                else item.ship_status = 0;
            }

            var pageResult = new PagedResult<OrderViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = result,
            };
            return pageResult;
        }
        public async Task<ApiResult<OrderDetailViewModel>> Detail(int id)
        {
            var query = from od in _context.OrDetails
                        join p in _context.Products on od.product_id equals p.id
                        join o in _context.Orders on od.order_id equals o.id
                        where o.id == id
                        select new { p, o, od };


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
                cancel_at = a.o.cancel_at,
                update_at = a.o.cancel_at,
                address_receiver = a.o.address_receiver,
                coupon_id = a.o.coupon_id,
                payment_id = a.o.payment_id,
                phone_receiver = a.o.phone_receiver,
                total = a.o.total,
                transport_fee = a.o.transport_fee

            }).FirstOrDefault();

            var transport = _context.Transports.Where(x => x.order_id == DataOrder.id).FirstOrDefault();
            if (transport != null)
            {
                DataOrder.ship_status = transport.ship_status;
            }
            else DataOrder.ship_status = 0;

            if (DataOrder == null)
            {
                return new ApiErrorResult<OrderDetailViewModel>("Đơn hàng không tồn tại");
            }
            var customer = await _context.Customers.FindAsync(DataOrder.cus_id);
            DataOrder.cus_name = customer.name;
            DataOrder.cus_email = customer.email;
            DataOrder.cus_phone = customer.phone;

            List<OrderDetailModel> Details = query.Select(a => new OrderDetailModel()
            {
                order_id = a.od.order_id,
                product_id = a.od.product_id,
                product_image = GetBase64StringForImage(_storageService.GetFileUrl(a.p.image)),
                product_name = a.p.name,
                promotion_price = a.od.promotion_price,
                quantity = a.od.quantity,
                unit_price = a.od.unit_price
            }).ToList();

            var model = new OrderDetailViewModel();
            model.Order = DataOrder;
            model.Details = Details;
            return new ApiSuccessResult<OrderDetailViewModel>(model);
        }
        public async Task<ApiResult<string>> PaymentConfirm(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return new ApiErrorResult<string>("Không tìm thấy đơn hàng này trong CSDL");
            if(order.isPay)
                return new ApiErrorResult<string>("Đơn hàng này đã được thanh toán rồi, không thể thanh toán lại");
            order.isPay = true;
            _context.SaveChanges();
            return new ApiSuccessResult<string>("Thanh toán đơn hàng thành công");
        }
        public async Task<ApiResult<string>> CancelOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return new ApiErrorResult<string>("Không tìm thấy đơn hàng này trong CSDL");
            if (order.status == -1)
                return new ApiErrorResult<string>("Đơn hàng này đã bị hủy trước đó, không thể hủy lại");
            var isValid = await _context.Transports.AnyAsync(x => x.order_id == id);
            if(isValid)
                return new ApiErrorResult<string>("Đơn hàng này đã được tạo đơn vận chuyển, không thể hủy");
            else
            {
                order.status = -1;
                var details = await _context.OrDetails.Where(x => x.order_id == order.id).ToListAsync();
                foreach (var item in details)
                {
                    var product = _context.Products.Find(item.product_id);
                    if(product != null)
                    {
                        if(product.instock != null)
                        {
                            product.instock += item.quantity;
                        }
                    }
                }
                _context.SaveChanges();
                return new ApiSuccessResult<string>("Hủy đơn hàng thành công");
            }
        }
        protected static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

    }
}
