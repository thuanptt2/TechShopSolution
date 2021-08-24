using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.Utilities.Constants;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Sales;
using TechShopSolution.WebApp.Models;

namespace TechShopSolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICouponApiClient _couponApiClient;
        private readonly ICustomerApiClient _customerApiClient;
        private readonly IPaymentApiClient _paymentApiClient;
        private readonly IOrderApiClient _orderApiClient;

        public CartController(IProductApiClient productApiClient, ICouponApiClient couponApiClient,
            ICustomerApiClient customerApiClient, IPaymentApiClient paymentApiClient, IOrderApiClient orderApiClient)
        {
            _productApiClient = productApiClient;
            _couponApiClient = couponApiClient;
            _customerApiClient = customerApiClient;
            _paymentApiClient = paymentApiClient;
            _orderApiClient = orderApiClient;
        }
        [Route("/gio-hang")]
        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            CartViewModel currentCart = new CartViewModel();
            if (!string.IsNullOrEmpty(session))
            {
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
                foreach(var item in currentCart.items)
                {
                    var product = await _productApiClient.GetById(item.Id);
                    if(product.ResultObject == null)
                    {
                        item.isExist = false;
                    }
                    else 
                    {
                        if (product.ResultObject.instock == 0)
                        {
                            item.Instock = 0;
                        }
                        else
                        {
                            item.Instock = product.ResultObject.instock;
                        }
                        if (!product.ResultObject.isActive)
                        {
                            if (item.isActive)
                                item.isActive = false;
                        }
                        else
                        {
                            if (!item.isActive)
                                item.isActive = true;
                        }
                    }
                }
                HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            }
            if(currentCart.coupon != null)
            {
                ViewBag.CouponCode = currentCart.coupon.code;

                var result = await _couponApiClient.GetById(currentCart.coupon.id);
                if (!result.IsSuccess)
                {
                    ViewBag.CouponMessage = result.Message;
                }
                else
                {
                    if (result.ResultObject.start_at > DateTime.Today)
                    {
                        ViewBag.CouponMessage = "Mã này sẽ có hiệu lực lúc " + result.ResultObject.start_at + ". Hãy thử lại sau bạn nhé";
                    }
                    if (result.ResultObject.end_at < DateTime.Today)
                    {
                        ViewBag.CouponMessage = "Mã này đã hết hạn";
                    }
                    if (!result.ResultObject.isActive)
                        ViewBag.CouponMessage = "Mã này đã bị vô hiệu hóa";
                    if (result.ResultObject.quantity == 0)
                        ViewBag.CouponMessage = "Mã này đã được sử dụng hết";
                }
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        [Route("/gio-hang/thanh-toan")]
        public async Task<IActionResult> Checkout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }
            var id = User.FindFirst(ClaimTypes.Sid).Value;

            var customer = await _customerApiClient.GetById(int.Parse(id));
            if (customer.ResultObject != null)
            {
                ViewBag.CustomerAddress = customer.ResultObject.address;
            } else
            {
                TempData["error"] = customer.Message;
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }
            ViewBag.Payment = await _paymentApiClient.GetAll();
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            CartViewModel currentCart = new CartViewModel();
            List<CreateOrderDetailRequest> OrderDetail = new List<CreateOrderDetailRequest>();
            if (!string.IsNullOrWhiteSpace(session))
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            else {
                return RedirectToAction("Index", "Home");
            }
            string coupon_code = "";  decimal amount = 0; decimal total = 0; decimal discount = 0;

            foreach (var item in currentCart.items)
            {
                if (item.PromotionPrice > 0)
                    amount = item.Quantity * item.PromotionPrice;
                else amount = item.Quantity * item.Price;
                total += amount;
                var detail = new CreateOrderDetailRequest
                {
                    product_id = item.Id,
                    promotion_price = item.PromotionPrice,
                    quantity = item.Quantity,
                    image = item.Images,
                    name = item.Name,
                    slug = item.Slug,
                    unit_price = item.Price
                };
                OrderDetail.Add(detail);
            }

            if (currentCart.coupon != null)
            {
                coupon_code = currentCart.coupon.code;
                if (currentCart.coupon.min_order_value != null)
                {
                    if ((decimal)currentCart.coupon.min_order_value <= total)
                    {
                        if (currentCart.coupon.type.Equals("Phần trăm"))
                        {
                            if (currentCart.coupon.max_value != null)
                            {
                                discount = total * ((decimal)currentCart.coupon.value / 100);
                                if (discount > (decimal)currentCart.coupon.max_value)
                                    discount = (decimal)currentCart.coupon.max_value;
                            }
                            else discount = total * ((decimal)currentCart.coupon.value / 100);
                        }
                        else
                        {
                            if (currentCart.coupon.value >= (double)total)
                            {
                                discount = total;
                            }
                            else discount = (decimal)currentCart.coupon.value;
                        }
                    }
                }
                else
                {
                    if (currentCart.coupon.type.Equals("Phần trăm"))
                    {
                        if (currentCart.coupon.max_value != null)
                        {
                            discount = total * ((decimal)currentCart.coupon.value / 100);
                            if (discount > (decimal)currentCart.coupon.max_value)
                                discount = (decimal)currentCart.coupon.max_value;
                        }
                        else discount = total * ((decimal)currentCart.coupon.value / 100);
                    }
                    else
                    {
                        if (currentCart.coupon.value >= (double)total)
                        {
                            discount = total;
                        }
                        else discount = (decimal)currentCart.coupon.value;
                    }
                }
            }
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMsg = TempData["error"];
            }
            return View(new CheckoutRequest
            {
                Order = new CreteOrderRequest
                {
                    cus_id = int.Parse(id),
                    total = total,
                    coupon_code = coupon_code,
                    discount = discount,
                    name_receiver = !customer.IsSuccess ? "" : customer.ResultObject.name,
                    note = null,
                    address_receiver = !customer.IsSuccess ? "" : customer.ResultObject.address,
                    phone_receiver = !customer.IsSuccess ? "" : customer.ResultObject.phone,
                },
                OrderDetails = OrderDetail
            });
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutRequest request)
        {
      
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            CartViewModel currentCart = new CartViewModel();
            List<CreateOrderDetailRequest> OrderDetail = new List<CreateOrderDetailRequest>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);

            foreach (var item in currentCart.items)
            {
                var detail = new CreateOrderDetailRequest
                {
                    product_id = item.Id,
                    promotion_price = item.PromotionPrice,
                    quantity = item.Quantity,
                    image = item.Images,
                    name = item.Name,
                    slug = item.Slug,
                    unit_price = item.Price
                };
                OrderDetail.Add(detail);
            }
            request.OrderDetails = OrderDetail;
            var customer = await _customerApiClient.GetById(request.Order.cus_id);
            if (customer.ResultObject != null)
            {
                ViewBag.CustomerAddress = customer.ResultObject.address;
            }
            ViewBag.Payment = await _paymentApiClient.GetAll();
            if (request.Order.payment_id == 0)
                request.Order.payment_id = null;
            if (!ModelState.IsValid)
                return View(request);
            var result = await _orderApiClient.CreateOrder(request);
            if(result.IsSuccess)
            {
                TempData["result"] = "Đặt hàng thành công. Cảm ơn quý khách đã mua hàng của chúng tôi.";
                HttpContext.Session.Remove(SystemConstants.CartSession);
                var contentMailClient = sendMailToClient(int.Parse(result.ResultObject), request);
                var contentMailAdmin = sendMailToAdmin(int.Parse(result.ResultObject), request, customer.ResultObject);
                await SendMail(customer.ResultObject.email, "Đặt hàng thành công - Đơn hàng #" + result.ResultObject, contentMailClient);
                await SendMail("thuanneuwu@gmail.com", "Đơn hàng mới #" + result.ResultObject, contentMailAdmin);
                return RedirectToAction("Index","Home");
            }
            TempData["error"] = result.Message;
            return RedirectToAction("Checkout");

        }
        [HttpGet]
        public async Task<IActionResult> GetListItems()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            CartViewModel currentCart = new CartViewModel();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            if (currentCart.coupon != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var cus_id = User.FindFirst(ClaimTypes.Sid).Value;
                    var result = await _couponApiClient.UseCoupon(currentCart.coupon.code, int.Parse(cus_id));
                    if (!result.IsSuccess)
                    {
                        currentCart.coupon = null;
                    }
                    else
                    {
                        if (result.ResultObject.start_at > DateTime.Today || result.ResultObject.end_at < DateTime.Today || !result.ResultObject.isActive || result.ResultObject.quantity == 0)
                        {
                            currentCart.coupon = null;
                        }
                        else
                        {
                            currentCart.coupon = new CouponViewModel
                            {
                                code = result.ResultObject.code,
                                type = result.ResultObject.type,
                                value = result.ResultObject.value,
                                max_value = result.ResultObject.max_price,
                                min_order_value = result.ResultObject.min_order_value,
                                quantity = result.ResultObject.quantity
                            };
                        }
                    }
                }
            }
            return Ok(currentCart);
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _productApiClient.GetById(id);
            if (product.ResultObject == null)
                return BadRequest("Thêm vào giỏ hàng thất bại ! Sản phẩm không tồn tại hoặc đã bị xóa.");
            if (!product.ResultObject.isActive)
                return BadRequest("Thêm vào giỏ hàng thất bại ! Sản phẩm hiện tại đã ngừng kinh doanh.");
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            var currentCart = new CartViewModel();
            currentCart.items = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            int quantity = 1;
            if (currentCart.items.Any(x => x.Id == product.ResultObject.id))
            {
                var item = currentCart.items.First(x => x.Id == id);
                if (product.ResultObject.instock == item.Quantity)
                    return BadRequest("Sản phẩm chỉ còn lại " + product.ResultObject.instock + " cái, bạn không thể mua thêm được.");

                if (item.Quantity >= 5)
                    return BadRequest("Bạn chỉ được mua tối đa 5 sản phẩm, sản phẩm này đã có trong giỏ hàng của bạn.");
                else item.Quantity++;
            }
            else
            {
                var cartItem = new CartItemViewModel()
                {
                    Id = id,
                    Instock = product.ResultObject.instock,
                    Code = product.ResultObject.code,
                    Slug = product.ResultObject.slug,
                    Price = product.ResultObject.unit_price,
                    isExist = true,
                    isActive = true,
                    PromotionPrice = product.ResultObject.promotion_price,
                    Images = product.ResultObject.image,
                    Name = product.ResultObject.name,
                    Quantity = quantity
                };
                if (currentCart.items == null) currentCart.items = new List<CartItemViewModel>();
                currentCart.items.Add(cartItem);

            }
            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

            return Ok(currentCart);
        }
        public IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            CartViewModel currentCart = new CartViewModel();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            foreach (var item in currentCart.items)
            {
                if (item.Id == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.items.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                    break;
                }
            }
            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
        [Authorize]
        public async Task<IActionResult> UseCoupon(string code)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("Quý khách chưa đăng nhập");
            }
            var id = User.FindFirst(ClaimTypes.Sid).Value;

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            if (session == null || session == "")
            {
                return BadRequest("Chưa có sản phẩm nào trong giỏ hàng.");
            }
            else
            {
                var result = await _couponApiClient.UseCoupon(code, int.Parse(id));
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                else
                {
                    if (result.ResultObject.start_at > DateTime.Today)
                    {
                        return BadRequest("Mã này sẽ có hiệu lực lúc " + result.ResultObject.start_at + ". Hãy thử lại sau bạn nhé");
                    }
                    if (result.ResultObject.end_at < DateTime.Today)
                    {
                        return BadRequest("Mã này đã hết hạn");
                    }
                    if (!result.ResultObject.isActive)
                        return BadRequest("Mã này đã bị vô hiệu hóa");
                    if (result.ResultObject.quantity == 0)
                        return BadRequest("Mã này đã được sử dụng hết");
                }
                CartViewModel currentCart = new CartViewModel();
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
                if (result.ResultObject.min_order_value != null)
                {
                    decimal total = 0;
                    decimal amount = 0;
                    foreach (var item in currentCart.items)
                    {
                        if (item.PromotionPrice > 0)
                        {
                            amount = item.PromotionPrice * item.Quantity;
                        }
                        else
                        {
                            amount = item.Price * item.Quantity;
                        }
                        total += amount;
                    }
                    if (total < (decimal)result.ResultObject.min_order_value)
                    {
                        var value = (decimal)result.ResultObject.min_order_value - total;
                        var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
                        return BadRequest("Chưa đạt giá trị đơn hàng tối thiểu. Cần mua thêm " + String.Format(info, "{0:c}", value) + " để sử dụng mã này");
                    }
                }

                currentCart.coupon = new CouponViewModel
                {
                    id = result.ResultObject.id,
                    code = result.ResultObject.code,
                    type = result.ResultObject.type,
                    value = result.ResultObject.value,
                    max_value = result.ResultObject.max_price,
                    min_order_value = result.ResultObject.min_order_value,
                    quantity = result.ResultObject.quantity
                };

                HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

                return Ok(currentCart);
            }

        }
        public IActionResult CancelCoupon()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            if(!string.IsNullOrWhiteSpace(session))
            {
                CartViewModel currentCart = new CartViewModel();
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
                currentCart.coupon = null;
                HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            }
            return Ok("Hủy bỏ mã giảm giá thành công");
        }
        public async Task<JsonResult> LoadProvince()
        {
            try
            {
                var result = await _customerApiClient.LoadProvince();
                if (result == null || !result.IsSuccess)
                {
                    return null;
                }
                return Json(new
                {
                    data = result.ResultObject,
                    status = true
                });
            }
            catch
            {
                return null;
            }
        }
        public async Task<JsonResult> LoadDistrict(int provinceID)
        {
            try
            {
                var result = await _customerApiClient.LoadDistrict(provinceID);
                if (result == null || !result.IsSuccess)
                {
                    return null;
                }
                return Json(new
                {
                    data = result.ResultObject,
                    status = true
                });
            }
            catch
            {
                return null;
            }
        }
        public async Task<JsonResult> LoadWard(int districtID)
        {
            try
            {
                var result = await _customerApiClient.LoadWard(districtID);
                if (result == null || !result.IsSuccess)
                {
                    return null;
                }
                return Json(new
                {
                    data = result.ResultObject,
                    status = true
                });
            }
            catch
            {
                return null;
            }
        }
        public string sendMailToClient(int orderID, CheckoutRequest request)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            string contentMail = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\mail-template", "neworder.html"));
            contentMail = contentMail.Replace("{{order_id}}", orderID.ToString());
            contentMail = contentMail.Replace("{{total}}", String.Format(info, "{0:N0}", request.Order.total));
            contentMail = contentMail.Replace("{{discount}}", String.Format(info, "{0:N0}", request.Order.discount));
            contentMail = contentMail.Replace("{{ship_fee}}", String.Format(info, "{0:N0}", request.Order.transport_fee));
            contentMail = contentMail.Replace("{{address}}", String.Format(info, "{0:N0}", request.Order.address_receiver));
            var final_total = request.Order.total - request.Order.discount + request.Order.transport_fee;
            contentMail = contentMail.Replace("{{final_total}}", String.Format(info, "{0:N0}", final_total));
            return contentMail;
        }
        public string sendMailToAdmin(int orderID, CheckoutRequest request, CustomerViewModel cutomer)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            string contentMail = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\mail-template", "AdminOrderConfirm.html"));
            contentMail = contentMail.Replace("{{order_id}}", orderID.ToString());
            contentMail = contentMail.Replace("{{customer_name}}", request.Order.name_receiver);
            contentMail = contentMail.Replace("{{customer_email}}", cutomer.email);
            contentMail = contentMail.Replace("{{customer_phone}}", request.Order.phone_receiver);
            contentMail = contentMail.Replace("{{order_note}}", request.Order.note);
            contentMail = contentMail.Replace("{{order_time}}", DateTime.Now.ToString());
            contentMail = contentMail.Replace("{{total}}", String.Format(info, "{0:N0}", request.Order.total));
            contentMail = contentMail.Replace("{{discount}}", String.Format(info, "{0:N0}", request.Order.discount));
            contentMail = contentMail.Replace("{{ship_fee}}", String.Format(info, "{0:N0}", request.Order.transport_fee));
            contentMail = contentMail.Replace("{{address}}", String.Format(info, "{0:N0}", request.Order.address_receiver));
            var final_total = request.Order.total - request.Order.discount + request.Order.transport_fee;
            contentMail = contentMail.Replace("{{final_total}}", String.Format(info, "{0:N0}", final_total));
            return contentMail;
        }
        public async Task SendMail(string _to, string _subject, string _body)
        {
            MailMessage message = new MailMessage();
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.From = new MailAddress("Techshop Việt Nam <admin@techshopvn.xyz>");
            message.To.Add(new MailAddress(_to));
            message.Subject = _subject;
            message.Body = _body;

            using var smtpClient = new SmtpClient("mail.techshopvn.xyz", 587);
            smtpClient.EnableSsl = false;
            smtpClient.Credentials = new NetworkCredential("admin@techshopvn.xyz", "THANHTHUAn123");

            await smtpClient.SendMailAsync(message);
        }
    }
}
