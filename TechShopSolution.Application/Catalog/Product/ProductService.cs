﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.Data.EF;
using TechShopSolution.Utilities.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using TechShopSolution.Application.Common;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.Application.Catalog.Product
{
    public class ProductService : IProductService
    {
        private readonly TechShopDBContext _context;
        private readonly IStorageService _storageService;
        public ProductService(TechShopDBContext context, IStorageService storageService)
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
            if (request.More_images != null)
            {
                if (request.More_images.Count == 1)
                    product.more_images = await this.SaveFile(request.More_images[0]);
                else
                {
                    for (int i = 0; i < request.More_images.Count(); i++)
                    {
                        if (request.More_images.Count - i == 1)
                            product.more_images += await this.SaveFile(request.More_images[i]);
                        else product.more_images += await this.SaveFile(request.More_images[i]) + ",";
                    }
                }
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.id;
        }

        public async Task<int> Delete(int productID)
        {
            var product = await _context.Products.FindAsync(productID);
            if (product == null) throw new TechshopException($"Không tìm thấy sản phẩm: {productID}");
            if (product.image != null)
                await _storageService.DeleteFileAsync(product.image);
            if (product.more_images != null)
            {
                string strListImage = product.more_images;
                string[] listImage = strListImage.Split(',');   
                for(int i = 0; i < listImage.Count();i++)
                    await _storageService.DeleteFileAsync(listImage[i]);
            }

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
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

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
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
                Items = data,
            };
            return pageResult;
        }


        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pic in _context.CategoryProducts on p.id equals pic.product_id
                        join ct in _context.Categories on pic.cate_id equals ct.id
                        select new { p };
            if (!String.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.p.name.Contains(request.Keyword));
            if (request.CategoryID != null)
            {
                query = query.Where(x => x.p.cate_id == request.CategoryID);
            }
            if(request.BrandID != null)
            {
                query = query.Where(x => x.p.brand_id == request.BrandID);
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
        public async Task<ProductViewModel> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                var productViewModel = new ProductViewModel
                {
                    id = product.id,
                    name = product.name,
                    best_seller = product.best_seller,
                    brand_id = product.brand_id,
                    cate_id = product.cate_id,
                    code = product.code,
                    create_at = product.create_at,
                    descriptions = product.descriptions,
                    featured = product.featured,
                    image = product.image,
                    instock = product.instock,
                    meta_descriptions = product.meta_descriptions,
                    meta_keywords = product.meta_keywords,
                    meta_tittle = product.meta_tittle,
                    more_images = product.more_images,
                    promotion_price = product.promotion_price,
                    short_desc = product.short_desc,
                    slug = product.slug,
                    specifications = product.specifications,
                    status = product.status,
                    unit_price = product.unit_price,
                    warranty = product.warranty,
                };
                return productViewModel;
            }
            return null;
        }
        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null) throw new TechshopException($"Không tìm thấy sản phẩm với ID: {request.Id}");

            product.name = request.Name;
            product.slug = request.Slug;
            product.warranty = request.Warranty;
            product.specifications = request.Specifications;
            product.short_desc = request.Short_desc;
            product.descriptions = request.Descriptions;
            product.status = request.Status;
            product.meta_descriptions = request.Meta_descriptions;
            product.meta_keywords = request.Meta_keywords;
            product.unit_price = request.Unit_price;
            product.promotion_price = request.Promotion_price;
            product.best_seller = request.Best_seller;
            product.featured = request.Featured;
            product.instock = request.Instock;
            product.brand_id = request.Brand_id;
            product.cate_id = request.Cate_id;
            product.meta_tittle = request.Meta_tittle;
            product.code = request.Code;
            if (request.Image != null)
            {
                product.image = await this.SaveFile(request.Image);
            }
            if (request.More_images != null)
            {
                for (int i = 0; i < request.More_images.Count(); i++)
                {
                    if(!this._storageService.isExistFile(request.More_images[i].ToString()))
                    {
                        if (request.More_images.Count - i == 1)
                            product.more_images += await this.SaveFile(request.More_images[i]);
                        else product.more_images += await this.SaveFile(request.More_images[i]) + ",";
                    }
                }
            }
            return await _context.SaveChangesAsync();
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