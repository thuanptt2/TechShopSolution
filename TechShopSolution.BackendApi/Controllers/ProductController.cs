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
        public async Task<IActionResult> GetManagerProductByFilter(GetProductPagingRequest requet)
        {
            var products = await _productService.GetAllPaging(requet);
            if (products == null)
                return BadRequest("Không tồn tại sản phẩm này");
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetById(id);
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
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImageByProductID(int id)
        {
            var result = await _productService.GetImagesByProductID(id);
            return Ok(result);
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
