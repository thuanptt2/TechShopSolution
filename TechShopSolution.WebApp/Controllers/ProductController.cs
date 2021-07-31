using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TechShopSolution.ApiIntegration;
using TechShopSolution.ViewModels.Catalog.Category;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.WebApp.Models;

namespace TechShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categorytApiClient;
        private readonly IBrandApiClient _brandApiClient;
        private readonly ICustomerApiClient _customerApiClient;

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categorytApiClient, IBrandApiClient brandApiClient,
            ICustomerApiClient customerApiClient)
        {
            _productApiClient = productApiClient;
            _categorytApiClient = categorytApiClient;
            _brandApiClient = brandApiClient;
            _customerApiClient = customerApiClient;
        }
        [Route("san-pham/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            int? cus_id = null;
            if(User.Identity.IsAuthenticated)
            {
                cus_id = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
            }

            var product = await _productApiClient.GetPublicProductDetail(slug, cus_id);

            if (!product.IsSuccess)
            {
                if (product.statusCode != null)
                    return RedirectToAction("HttpStatusCodeHandler", "Error", new { statusCode = product.statusCode });
                else
                {
                    TempData["error"] = product.Message;
                    return RedirectToAction("Index", "Home");
                }
            }

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMsg = TempData["error"];
            }

            List<ProductRecentlyViewModel> RecentlyProducts = new List<ProductRecentlyViewModel>();
            var session = HttpContext.Session.GetString("RecentlyProducts");
            if(!string.IsNullOrWhiteSpace(session))
            {
                RecentlyProducts = JsonConvert.DeserializeObject<List<ProductRecentlyViewModel>>(session);
            }
            var recentlyProduct = RecentlyProducts.Where(x => x.id == product.ResultObject.id).FirstOrDefault();
            if(recentlyProduct == null)
            {
                var pro = new ProductRecentlyViewModel()
                {
                    id = product.ResultObject.id,
                    name = product.ResultObject.name,
                    promotion_price = product.ResultObject.promotion_price,
                    slug = product.ResultObject.slug,
                    instock = product.ResultObject.instock,
                    image = product.ResultObject.image,
                    unit_price = product.ResultObject.unit_price,
                    view_at = DateTime.Now,
                };
                RecentlyProducts.Add(pro);
                HttpContext.Session.SetString("RecentlyProducts", JsonConvert.SerializeObject(RecentlyProducts));
            }
            else
            {
                recentlyProduct.view_at = DateTime.Now;
                HttpContext.Session.SetString("RecentlyProducts", JsonConvert.SerializeObject(RecentlyProducts));
            }
            return View(new ProductDetailViewModel()
            {
                Product = product.ResultObject,
                ProductsRecently = RecentlyProducts.OrderByDescending(x => x.view_at).ToList(),
                Ratings = await _productApiClient.GetRatingsProduct(slug),
                ProductsRelated = await _productApiClient.GetProductsRelated(product.ResultObject.brand_id, 4),
            });
        }
        [HttpGet]
        public IActionResult Rating(int product_id, string product_slug)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
            }
            var id = User.FindFirst(ClaimTypes.Sid).Value;
            var request = new ProductRatingRequest()
            {
                cus_id = int.Parse(id),
                product_id = product_id,
                product_slug = product_slug,
                score = 0,
                content = null
            };
            return View(request);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Rating(ProductRatingRequest request)
        {
            var result = await _customerApiClient.RatingPoduct(request);
            if (!result.IsSuccess)
            {
                TempData["error"] = result.Message;
                return RedirectToAction("Detail", new { slug = request.product_slug });
            }
            TempData["result"] = "Cảm ơn bạn đã đánh giá Sản phẩm & Dịch vụ của chúng tôi ^^";
            return RedirectToAction("Detail", new { slug = request.product_slug });
        }
        [Route("danh-muc/{slug}")]
        public async Task<IActionResult> Category(string slug, decimal? giathapnhat = null, decimal? giacaonhat = null, int sortid = 1, int pageIndex = 1)
        {
            var Category = await _categorytApiClient.GetBySlug(slug);
            var products = await _productApiClient.GetPublicProducts(new GetPublicProductPagingRequest()
            {
                CategorySlug = slug,
                Highestprice = giacaonhat,
                idSortType = sortid,
                Lowestprice = giathapnhat,
                PageIndex = pageIndex,
                PageSize = 9,
                
            });
            ViewBag.PageResult = products;
            ViewBag.LowestPrice = giathapnhat;
            ViewBag.HighestPrice = giacaonhat;
            ViewBag.SortID = sortid;
            return View(new ProductCategoryViewModel() { 
                Category = Category.ResultObject,
                Products = products
            });
        }
        [Route("san-pham")]
        public async Task<IActionResult> SearchProducts(int pageIndex = 1, string tukhoa = null, string danhmuc = null, int? danhmuccha = null, string thuonghieu = null, int idsort = 1, decimal? giathapnhat = null, decimal? giacaonhat = null, bool tinhtrang = true)
        {
            var Category = await _categorytApiClient.GetBySlug(danhmuc);
            var Brands = await _productApiClient.GetAllBrand();
            var Categories = await _categorytApiClient.GetAllCategory();

            var request = HttpContext.Request;

            if (danhmuc != null)
            {
                if (danhmuccha != null)
                {
                    var Categogyy = await _categorytApiClient.GetById((int)danhmuccha);
                    Categories = await OrderCateToTree(Categories, (int)danhmuccha);
                    Categories.Add(Categogyy.ResultObject);
                }
                else
                {
                    if(Category.ResultObject != null)
                    {
                        Categories = await OrderCateToTree(Categories, Category.ResultObject.id);
                        Categories.Add(Category.ResultObject);
                        danhmuccha = Category.ResultObject.id;
                    }
                }
                if(giathapnhat > giacaonhat)
                {
                    decimal temp = (decimal)giathapnhat;
                    giathapnhat = giacaonhat;
                    giacaonhat = temp;
                }

                var products = await _productApiClient.GetPublicProducts(new GetPublicProductPagingRequest()
                {
                    CategorySlug = danhmuc,
                    Keyword = tukhoa,
                    PageIndex = pageIndex,
                    PageSize = 9,
                    BrandSlug = thuonghieu,
                    Highestprice = giacaonhat,
                    Lowestprice = giathapnhat,
                    idSortType = idsort
                });


                ViewBag.Keyword = tukhoa;
                ViewBag.CateParent = danhmuccha;
                ViewBag.Brands = Brands.OrderBy(x=> x.brand_name).Select(x => new SelectListItem()
                {
                    Text = x.brand_name,
                    Value = x.brand_slug,
                    Selected = thuonghieu!=null && thuonghieu.Equals(x.brand_slug)
                });
                ViewBag.Categories = Categories.Select(x => new SelectListItem()
                {
                    Text = x.cate_name,
                    Value = x.cate_slug,
                    Selected = danhmuc != null && danhmuc.Equals(x.cate_slug)
                });
                ViewBag.PageResult = products;
                ViewBag.IdSort = idsort;
                ViewBag.LowestPrice = giathapnhat;
                ViewBag.HighestPrice = giacaonhat;


                return View(new ProductCategoryViewModel()
                {
                    Category = Category.ResultObject,
                    Products = products,
                });
            }
            else
            {
                Categories = await OrderCateToTree(await _categorytApiClient.GetAllCategory());
                if (giathapnhat > giacaonhat)
                {
                    decimal temp = (decimal)giathapnhat;
                    giathapnhat = giacaonhat;
                    giacaonhat = temp;
                }
                var products = await _productApiClient.GetPublicProducts(new GetPublicProductPagingRequest()
                {
                    CategorySlug = danhmuc,
                    Keyword = tukhoa,
                    PageIndex = pageIndex,
                    PageSize = 9,
                    BrandSlug = thuonghieu,
                    Highestprice = giacaonhat,
                    Lowestprice = giathapnhat,
                    idSortType = idsort
                });
                ViewBag.Keyword = tukhoa;
                ViewBag.Brands = Brands.Select(x => new SelectListItem()
                {
                    Text = x.brand_name,
                    Value = x.brand_slug,
                    Selected = thuonghieu != null && thuonghieu.Equals(x.brand_slug)
                });
                ViewBag.Categories = Categories.Select(x => new SelectListItem()
                {
                    Text = x.cate_name,
                    Value = x.cate_slug,
                    Selected = danhmuc != null && danhmuc.Equals(x.cate_slug)
                });
                ViewBag.PageResult = products;
                ViewBag.IdSort = idsort;
                ViewBag.LowestPrice = giathapnhat;
                ViewBag.HighestPrice = giacaonhat;
                return View(new ProductCategoryViewModel()
                {
                    Category = null,
                    Products = products
                });
            }
        }
        public async Task<List<CategoryViewModel>> OrderCateToTree(List<CategoryViewModel> lst, int parent_id = 0, int level = 0)
        {
            if (lst != null)
            {
                List<CategoryViewModel> result = new List<CategoryViewModel>();
                foreach (CategoryViewModel cate in lst)
                {
                    if (cate.parent_id == parent_id)
                    {
                        CategoryViewModel tree = new CategoryViewModel();
                        tree = cate;
                        tree.level = level;
                        tree.cate_name = String.Concat(Enumerable.Repeat("‣", level)) + tree.cate_name;

                        result.Add(tree);  
                        List<CategoryViewModel> child = await OrderCateToTree(lst, cate.id, level + 1);
                        result.AddRange(child);
                    }
                }
                return result;
            }
            return null;
        }
        [HttpPost]
        public async Task<IActionResult> FavoriteProduct(int cus_id, int product_id)
        {
            var result = await _customerApiClient.FavoriteProduct(cus_id, product_id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UnFavoriteProduct(int cus_id, int product_id)
        {
            var result = await _customerApiClient.UnFavoriteProduct(cus_id, product_id);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            return Ok();
        }
    }
}
