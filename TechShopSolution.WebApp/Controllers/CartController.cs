using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.Utilities.Constants;
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
        public IActionResult Index()
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

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if(session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            int quantity = 1;
            if(currentCart.Any(x => x.Id == product.ResultObject.id))
            {
                quantity = currentCart.First(x => x.Id == id).Quantity + 1;
            }
            var cartItem = new CartItemViewModel()
            {
                Id = id,
                Code = product.ResultObject.code,
                Images = product.ResultObject.image,
                Name = product.ResultObject.name,
                Quantity = quantity
            };
            if (currentCart == null) currentCart = new List<CartItemViewModel>();

            currentCart.Add(cartItem);

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(cartItem));
            return Ok();
        }
    }
}
