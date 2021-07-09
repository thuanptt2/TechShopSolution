using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categorytApiClient, IBrandApiClient brandApiClient)
        {
            _productApiClient = productApiClient;
            _categorytApiClient = categorytApiClient;
            _brandApiClient = brandApiClient;
        }
        [Route("san-pham/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var product = await _productApiClient.GetBySlug(slug);
            if (product.ResultObject == null)
                return View();
            string[] CateId = product.ResultObject.CateID.Split(",");
            var Category = await _categorytApiClient.GetById(int.Parse(CateId[0]));
            var Brand = await _brandApiClient.GetById(product.ResultObject.brand_id);

            return View(new ProductDetailViewModel()
            {
                Product = product.ResultObject,
                Category = Category.ResultObject,
                Brand = Brand.ResultObject,
                ProductsRelated = await _productApiClient.GetProductsRelated(product.ResultObject.brand_id, 4),
                ImageList = await _productApiClient.GetImageByProductID(product.ResultObject.id),
            });
        }
        [Route("danh-muc/{slug}")]
        public async Task<IActionResult> Category(string slug, decimal? giathapnhat = null, decimal? giacaonhat = null, int sortid = 1, int page = 1)
        {
            var Category = await _categorytApiClient.GetBySlug(slug);
            var products = await _productApiClient.GetPublicProducts(new GetPublicProductPagingRequest()
            {
                CategorySlug = slug,
                Highestprice = giacaonhat,
                idSortType = sortid,
                Lowestprice = giathapnhat,
                PageIndex = page,
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
        public async Task<IActionResult> SearchProducts(string tukhoa, string danhmuc, int? danhmuccha, string thuonghieu, int idsort = 1, decimal? giathapnhat = null, decimal? giacaonhat = null, bool tinhtrang = true, int pageIndex = 1)
        {
            var Category = await _categorytApiClient.GetBySlug(danhmuc);
            var Brands = await _productApiClient.GetAllBrand();
            var Categories = await _categorytApiClient.GetAllCategory();

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
                        tree.cate_name = String.Concat(Enumerable.Repeat(Microsoft.VisualBasic.Strings.Space(3), level)) + tree.cate_name;

                        result.Add(tree);  
                        List<CategoryViewModel> child = await OrderCateToTree(lst, cate.id, level + 1);
                        result.AddRange(child);
                    }
                }
                return result;
            }
            return null;
        }
    }
}
