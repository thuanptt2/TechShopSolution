using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.Utilities.Constants;
using TechShopSolution.ViewModels.Sales;
using TechShopSolution.WebApp.Models;

namespace TechShopSolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICouponApiClient _couponApiClient;

        public CartController(IProductApiClient productApiClient, ICouponApiClient couponApiClient)
        {
            _productApiClient = productApiClient;
            _couponApiClient = couponApiClient;
        }
        [Route("/gio-hang")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(CheckoutRequest request)
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetListItems()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            CartViewModel currentCart = new CartViewModel();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            return Ok(currentCart);
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _productApiClient.GetById(id);
            if (product.ResultObject == null)
                return BadRequest("Thêm vào giỏ hàng thất bại ! Sản phẩm không tồn tại hoặc đã bị xóa.");
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            var currentCart = new CartViewModel();
            currentCart.items = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            int quantity = 1;
            if (currentCart.items.Any(x => x.Id == product.ResultObject.id))
            {
                var item = currentCart.items.First(x => x.Id == id);
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
                }
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
        public async Task<IActionResult> UseCoupon(string code)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            if(session == null || session == "")
            {
                return BadRequest("Chưa có sản phẩm nào trong giỏ hàng.");
            }
            else
            {
                var result = await _couponApiClient.GetByCode(code);
                if (result.ResultObject == null)
                    return BadRequest("Mã giảm giá không tồn tại");
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
                if(result.ResultObject.quantity == 0)
                    return BadRequest("Mã này đã được sử dụng hết");

                CartViewModel currentCart = new CartViewModel();
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
                currentCart.coupon = new CouponViewModel
                {
                    code = result.ResultObject.code,
                    type = result.ResultObject.type,
                    value = result.ResultObject.value,
                };

                HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

                return Ok(currentCart);
            }
         
        }
    }
}
