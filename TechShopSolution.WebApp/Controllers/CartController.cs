﻿using Microsoft.AspNetCore.Http;
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

        public CartController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
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
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            return Ok(currentCart);
        }
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _productApiClient.GetById(id);
            if (product.ResultObject == null)
                return BadRequest("Thêm vào giỏ hàng thất bại ! Sản phẩm không tồn tại hoặc đã bị xóa.");
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            var currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            int quantity = 1;
            if (currentCart.Any(x => x.Id == product.ResultObject.id))
            {
                var item = currentCart.First(x => x.Id == id);
                item.Quantity++;
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
                if (currentCart == null) currentCart = new List<CartItemViewModel>();

                currentCart.Add(cartItem);
            }
            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

            return Ok(currentCart);
        }
        public IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            foreach (var item in currentCart)
            {
                if (item.Id == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                }
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
            return Ok(currentCart);
        }
    }
}