﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.Data.EF;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Sales;

namespace TechShopSolution.Application.Catalog.Order
{
    public class OrderService : IOrderService
    {
        private readonly TechShopDBContext _context;
        public OrderService(TechShopDBContext context)
        {
            _context = context;
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
                    if(coupon.quantity != null)
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
                    isPay = request.Order.payment_id == 1 ? true : false,
                    payment_id = (int)request.Order.payment_id,
                    isShip = false,
                    name_receiver = request.Order.name_receiver,
                    note = request.Order.note,
                    phone_receiver = request.Order.phone_receiver,
                    status = true,
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
                }
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<string>(order.id.ToString());
            }
            catch
            {
                return new ApiErrorResult<string>("Tạo đơn đặt hàng thất bại, quý khách vui lòng thử lại sau");
            }
        }
    }
}