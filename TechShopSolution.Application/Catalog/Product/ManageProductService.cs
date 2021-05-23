using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.ViewModels.Catalog.Product.Manage;
using TechShopSolution.Application.DTO;
using TechShopSolution.Data.EF;
using TechShopSolution.Utilities.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using TechShopSolution.Application.Common;

namespace TechShopSolution.Application.Catalog.Product
{
    public class ManageProductService : IManageProductService
    {
        private readonly TechShopDBContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(TechShopDBContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public Task<int> AddImages(int productId, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new TechShopSolution.Data.Entities.Product
            {
                name = request.Name,
                best_seller = false,
                brand_id = request.Brand_id,
                cate_id = request.Cate_id,
                code = request.Code,
                create_at = DateTime.Now,
                descriptions = request.Descriptions,
                featured = false,
                instock = request.Instock,
                meta_descriptions = request.Meta_descriptions,
                meta_keywords = request.Meta_keywords,
                meta_tittle = request.Meta_tittle,
                more_images = request.More_images,
                promotion_price = request.Promotion_price,
                short_desc = request.Short_desc,
                slug = request.Slug,
                specifications = request.Specifications,
                status = request.Status,
                unit_price = request.Unit_price,
                warranty = request.Warranty,
            };
                if (request.Image != null)
                {
                    product.image = await this.SaveFile(request.Image);
                }
                _context.Products.Add(product);
                return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productID)
        {
            var product = await _context.Products.FindAsync(productID);
            if (product == null) throw new TechshopException($"Không tìm thấy sản phẩm: {productID}");
            if (product.image != null)
                await _storageService.DeleteFileAsync(product.image);
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }


        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pic in _context.CategoryProducts on p.id equals pic.product_id
                        join ct in _context.Categories on pic.cate_id equals ct.id
                        select new { p, pic};
            if (!String.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.p.name.Contains(request.Keyword));
            if(request.CategoryID.Count > 0)
            {
                query = query.Where(p => request.CategoryID.Contains(p.pic.cate_id));
            }
            int totalRow = await query.CountAsync();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new ProductViewModel()
                {
                    id = a.p.id,
                    name = a.p.name,
                    best_seller = a.p.best_seller,
                    brand_id = a.p.brand_id,
                    cate_id = a.p.cate_id,
                    code = a.p.code,
                    create_at = a.p.create_at,
                    descriptions = a.p.descriptions,
                    featured = false,
                    image = a.p.image,
                    instock = a.p.instock,
                    meta_descriptions = a.p.meta_descriptions,
                    meta_keywords = a.p.meta_keywords,
                    meta_tittle = a.p.meta_tittle,
                    more_images = a.p.more_images,
                    promotion_price = a.p.promotion_price,
                    short_desc = a.p.short_desc,
                    slug = a.p.slug,
                    specifications = a.p.specifications,
                    status = a.p.status,
                    unit_price = a.p.unit_price,
                    warranty = a.p.warranty,
                }).ToListAsync();

            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = await data,
            };
            return pageResult;
        }

        public Task<int> RemoveImages(int productId, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.id);
            if (product == null) throw new TechshopException($"Không tìm thấy sản phẩm với ID: {request.id}");

            product.name = request.name;
            product.slug = request.slug;
            product.warranty = request.warranty;
            product.specifications = request.specifications;
            product.short_desc = request.short_desc;
            product.descriptions = request.descriptions;
            product.status = request.status;
            product.meta_descriptions = request.meta_descriptions;
            product.meta_keywords = request.meta_keywords;
            product.meta_tittle = request.meta_tittle;
            if (request.image != null)
            {
                product.image = await this.SaveFile(request.image);
            }
            return await _context.SaveChangesAsync();
        }

        public Task<int> UpdateImage(int productId, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePrice(int productID, decimal newPrice, decimal newPromotionPrice)
        {
            var product = await _context.Products.FindAsync(productID);
            if (product == null) throw new TechshopException($"Không tìm thấy sản phẩm với ID: {productID}");
            product.unit_price = newPrice;
            product.promotion_price = newPromotionPrice;
            return await _context.SaveChangesAsync() > 0;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
