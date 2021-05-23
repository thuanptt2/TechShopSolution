using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.Application.DTO;
using TechShopSolution.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace TechShopSolution.Application.Catalog.Product
{
    public class PublicProductService : IPublicProductService
    {
        private readonly TechShopDBContext _context;
        public PublicProductService(TechShopDBContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pic in _context.CategoryProducts on p.id equals pic.product_id
                        join ct in _context.Categories on pic.cate_id equals ct.id
                        select new { p, pic };

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.cate_id == request.CategoryId);
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
    }
}
