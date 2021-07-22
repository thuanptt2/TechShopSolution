using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechShopSolution.Application.Catalog.Product;
using TechShopSolution.ViewModels.Catalog.Product;

namespace TechShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost("filter")]
        public IActionResult GetManagerProductByFilter(GetProductPagingRequest requet)
        {
            var products = _productService.GetAllPaging(requet);
            if (products == null)
                return BadRequest("Không có sản phẩm nào");
            return Ok(products);
        }
        [HttpPost("publicfilter")]
        public IActionResult GetPublicProducts(GetPublicProductPagingRequest requet)
        {
            var products = _productService.GetPublicProducts(requet);
            if (products == null)
                return BadRequest("Không có sản phẩm nào");
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetById(id);
            return Ok(product);
        }
        [HttpGet("slug")]
        public async Task<IActionResult> GetPublicProductDetail(string slug, int? cus_id)
        {
            var result = await _productService.GetPublicProductDetail(slug, cus_id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("featured/{take}")]
        public async Task<IActionResult> GetFeaturedProduct(int take)
        {
            var product = await _productService.GetFeaturedProduct(take);
            return Ok(product);
        }
        [HttpGet("bestseller/{take}")]
        public async Task<IActionResult> GetBestSellerProduct(int take)
        {
            var product = await _productService.GetBestSellerProduct(take);
            return Ok(product);
        }
        [HttpGet("Category")]
        public async Task<IActionResult> GetProductsByCategory(int id, int take)
        {
            var product = await _productService.GetProductsByCategory(id, take);
            return Ok(product);
        }
        [HttpGet("HomeProducts")]
        public async Task<IActionResult> GetHomeProducts(int id, int take)
        {
            var product = await _productService.GetHomeProductByCategory(id, take);
            return Ok(product);
        }
        [HttpGet("Related")]
        public IActionResult GetProductsRelated(int idBrand, int take)
        {
            var product = _productService.GetProductsRelated(idBrand, take);
            return Ok(product);
        }
        [HttpGet("Rating")]
        public IActionResult GetRatingsProduct(string slug)
        {
            var product = _productService.GetRatingsProduct(slug);
            return Ok(product);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            var result = await _productService.Create(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            var result = await _productService.Update(request);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }    
        [HttpDelete("DeleteImage")]
        public async Task<IActionResult> DeleteImage(int id, string fileName)
        {
            var result = await _productService.DeleteImage(id, fileName);
            if(result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> isValidSlug(string Code, string slug)
        {
            if (await _productService.isValidSlug(Code, slug))
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _productService.ChangeStatus(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("OffBestSeller/{id}")]
        public async Task<IActionResult> OffBestSeller(int id)
        {
            var result = await _productService.OffBestSeller(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("OffFeatured/{id}")]
        public async Task<IActionResult> OffFeatured(int id)
        {
            var result = await _productService.OffFeatured(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
