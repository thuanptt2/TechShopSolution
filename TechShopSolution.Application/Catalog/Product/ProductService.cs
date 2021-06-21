using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.Data.EF;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using TechShopSolution.Application.Common;
using TechShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;

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
        public Task<ApiResult<bool>> AddImages(int productId, IFormFile file)
        {
            throw new NotImplementedException();
        }
        public async Task<ApiResult<bool>> Create(ProductCreateRequest request)
        {
            if (request.Promotion_price == null)
                request.Promotion_price = 0;
            if (request.Warranty == null)
                request.Warranty = 0;
            var product = new TechShopSolution.Data.Entities.Product
            {
                name = request.Name,
                best_seller = request.Best_seller,
                brand_id = request.Brand_id,
                code = request.Code,
                create_at = DateTime.Now,
                descriptions = request.Descriptions,
                featured = request.Featured,
                instock = request.Instock,
                meta_descriptions = request.Meta_descriptions,
                meta_keywords = request.Meta_keywords,
                meta_tittle = request.Meta_tittle,
                promotion_price = (decimal)request.Promotion_price,
                short_desc = request.Short_desc,
                slug = request.Slug,
                specifications = request.Specifications,
                isActive = request.IsActive,
                isDelete = false,
                unit_price = request.Unit_price,
                warranty = (int)request.Warranty,
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
                        product.more_images += await this.SaveFile(request.More_images[i]) + ",";
                    }
                }
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            string[] cateIDs = request.CateID.Split(",");
            foreach (string cateID in cateIDs)
            {
                if (cateID != "")
                {
                    var productInCategory = new TechShopSolution.Data.Entities.CategoryProduct
                    {
                        cate_id = int.Parse(cateID),
                        product_id = product.id
                    };
                    _context.CategoryProducts.Add(productInCategory);
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(int productID)
        {
            try
            {
                var product = await _context.Products.FindAsync(productID);
                if (product != null)
                {
                    product.isDelete = true;
                    product.delete_at = DateTime.Now;

                    //Xóa hình ảnh sản phẩm
                    if (product.image != null)
                    {
                        await _storageService.DeleteFileAsync(product.image);
                        product.image = "";
                    }
                    if (product.more_images != null)
                    {
                        List<string> moreImages = product.more_images.Split(",").ToList();
                        foreach (string img in moreImages)
                        {
                            await _storageService.DeleteFileAsync(img);
                        }
                        product.more_images = "";
                    }

                    var result = await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>($"Sản phẩm không tồn tại");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
        public async Task<ApiResult<bool>> DeleteImage(int id, string fileName)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                List<string> moreImagesName = product.more_images.Split(",").ToList();
                bool flag = false;
                foreach (string moreimage in moreImagesName)
                {
                    if (moreimage != "")
                    {
                        if (moreimage.Equals(fileName))
                        {
                            moreImagesName.Remove(moreimage);
                            string MoreImageAfterDelete = null;
                            foreach (string imagestr in moreImagesName)
                            {
                                if (imagestr != "")
                                {
                                    MoreImageAfterDelete += imagestr + ",";
                                }
                            }
                            product.more_images = MoreImageAfterDelete;
                            product.update_at = DateTime.Now;
                            var result = await _storageService.DeleteFileAsync(fileName);
                            await _context.SaveChangesAsync();
                            flag = true;
                            break;
                        }
                    }
                }
                if (!flag)
                    return new ApiErrorResult<bool>("Hình này không tồn tại trong CSDL của sản phẩm");
                else return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Sản phẩm không tồn tại");
        }
        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            try
            {
                var query = from p in _context.Products
                            join pic in _context.CategoryProducts on p.id equals pic.product_id
                            where p.isDelete == false
                            select new { p, pic };

                if (!String.IsNullOrEmpty(request.Keyword))
                    query = query.Where(x => x.p.name.Contains(request.Keyword));

                if (request.CategoryID.Count() != 0)
                {
                    query = query.Where(x => x.pic.cate_id == (request.CategoryID[0]));
                }

                if (request.BrandID != null)
                {
                    query = query.Where(x => x.p.brand_id == request.BrandID);
                }

                var data = query.AsEnumerable()
                   .OrderByDescending(m => m.p.create_at)
                   .GroupBy(g => g.p);

                int totalRow = data.Count();

                List<ProductViewModel> result = data.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(a => new ProductViewModel()
                    {
                        id = a.Key.id,
                        name = a.Key.name,
                        best_seller = a.Key.best_seller,
                        brand_id = a.Key.brand_id,
                        code = a.Key.code,
                        create_at = a.Key.create_at,
                        descriptions = a.Key.descriptions,
                        featured = a.Key.featured,
                        image = a.Key.image,
                        instock = a.Key.instock,
                        meta_descriptions = a.Key.meta_descriptions,
                        meta_keywords = a.Key.meta_keywords,
                        meta_tittle = a.Key.meta_tittle,
                        more_images = a.Key.more_images,
                        promotion_price = a.Key.promotion_price,
                        short_desc = a.Key.short_desc,
                        slug = a.Key.slug,
                        specifications = a.Key.specifications,
                        isActive = a.Key.isActive,
                        unit_price = a.Key.unit_price,
                        warranty = a.Key.warranty,
                    }).ToList();

                var pageResult = new PagedResult<ProductViewModel>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = result,
                };
                return pageResult;
            }
            catch
            {
                var pageResult = new PagedResult<ProductViewModel>()
                {
                    TotalRecords = 0,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = null,
                };
                return pageResult;
            }
        }
        public async Task<PagedResult<ProductViewModel>> GetAllPagingWithMainImage(GetProductPagingRequest request)
        {
            try
            {
                var query = from p in _context.Products
                            join pic in _context.CategoryProducts on p.id equals pic.product_id
                            where p.isDelete == false
                            select new { p, pic };

                if (!String.IsNullOrEmpty(request.Keyword))
                    query = query.Where(x => x.p.name.Contains(request.Keyword));

                if (request.CategoryID != null)
                    if (request.CategoryID.Count != 0)
                        query = query.Where(x => x.pic.cate_id == (request.CategoryID[0]));

                if (request.BrandID != null)
                {
                    query = query.Where(x => x.p.brand_id == request.BrandID);
                }

                var data = query.AsEnumerable()
                    .OrderByDescending(m => m.p.create_at)
                    .GroupBy(g => g.p);

                int totalRow = data.Count();

                List<ProductViewModel> result = data.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(a => new ProductViewModel()
                    {
                        id = a.Key.id,
                        name = a.Key.name,
                        best_seller = a.Key.best_seller,
                        brand_id = a.Key.brand_id,
                        code = a.Key.code,
                        create_at = a.Key.create_at,
                        descriptions = a.Key.descriptions,
                        featured = a.Key.featured,
                        image = a.Key.image,
                        instock = a.Key.instock,
                        meta_descriptions = a.Key.meta_descriptions,
                        meta_keywords = a.Key.meta_keywords,
                        meta_tittle = a.Key.meta_tittle,
                        more_images = a.Key.more_images,
                        promotion_price = a.Key.promotion_price,
                        short_desc = a.Key.short_desc,
                        slug = a.Key.slug,
                        specifications = a.Key.specifications,
                        isActive = a.Key.isActive,
                        unit_price = a.Key.unit_price,
                        warranty = a.Key.warranty,
                    }).ToList();

                foreach (var pro in result)
                {
                    if (pro.image != null)
                    {
                        ImageListResult image = new ImageListResult();
                        pro.image = GetBase64StringForImage(_storageService.GetFileUrl(pro.image));
                    }
                }

                var pageResult = new PagedResult<ProductViewModel>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = result,
                };
                return pageResult;
            }
            catch
            {
                var pageResult = new PagedResult<ProductViewModel>()
                {
                    TotalRecords = 0,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = null,
                };
                return pageResult;
            }
        }
        public async Task<PagedResult<ProductViewModel>> GetPublicProducts(GetPublicProductPagingRequest request)
        {
            try
            {
                var query = from p in _context.Products
                            join pic in _context.CategoryProducts on p.id equals pic.product_id
                            join c in _context.Categories on pic.cate_id equals c.id
                            where p.isDelete == false
                            select new { p, pic, c};

                if (!String.IsNullOrEmpty(request.Keyword))
                    query = query.Where(x => x.p.name.Contains(request.Keyword));

                if (!String.IsNullOrEmpty(request.CategorySlug))
                    query = query.Where(x => x.c.cate_slug.Equals(request.CategorySlug));

                if (!String.IsNullOrEmpty(request.BrandSlug))
                {
                    query = query.Where(x => x.pic.cate_id.Equals(request.BrandSlug));
                }
                switch(request.idSortType)
                {
                    case 1:
                        query = query.OrderBy(x=>x.p.name);
                        break;
                    case 2:
                        query = query.OrderByDescending(x => x.p.name);
                        break;
                    case 3:
                        query = query.OrderBy(x => x.p.unit_price);
                        break;
                    case 4:
                        query = query.OrderByDescending(x => x.p.unit_price);
                        break;
                }
                if (request.Lowestprice != null && request.Highestprice != null)
                {
                    query = query.Where(x => x.p.unit_price >= request.Lowestprice && x.p.unit_price <= request.Highestprice);
                } else if(request.Lowestprice != null && request.Highestprice == null)
                {
                    query = query.Where(x => x.p.unit_price >= request.Lowestprice);
                }else if(request.Lowestprice == null && request.Highestprice != null)
                {
                    query = query.Where(x => x.p.unit_price <= request.Highestprice);
                }


                var data = query.AsEnumerable()
                    .GroupBy(g => g.p);

                int totalRow = data.Count();

                List<ProductViewModel> result = data.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(a => new ProductViewModel()
                    {
                        id = a.Key.id,
                        name = a.Key.name,
                        best_seller = a.Key.best_seller,
                        brand_id = a.Key.brand_id,
                        code = a.Key.code,
                        create_at = a.Key.create_at,
                        descriptions = a.Key.descriptions,
                        featured = a.Key.featured,
                        image = a.Key.image,
                        instock = a.Key.instock,
                        meta_descriptions = a.Key.meta_descriptions,
                        meta_keywords = a.Key.meta_keywords,
                        meta_tittle = a.Key.meta_tittle,
                        more_images = a.Key.more_images,
                        promotion_price = a.Key.promotion_price,
                        short_desc = a.Key.short_desc,
                        slug = a.Key.slug,
                        specifications = a.Key.specifications,
                        isActive = a.Key.isActive,
                        unit_price = a.Key.unit_price,
                        warranty = a.Key.warranty,
                    }).ToList();

                foreach (var pro in result)
                {
                    if (pro.image != null)
                    {
                        ImageListResult image = new ImageListResult();
                        pro.image = GetBase64StringForImage(_storageService.GetFileUrl(pro.image));
                    }
                }

                var pageResult = new PagedResult<ProductViewModel>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = result,
                };
                return pageResult;
            }
            catch
            {
                var pageResult = new PagedResult<ProductViewModel>()
                {
                    TotalRecords = 0,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = null,
                };
                return pageResult;
            }
        }

        public async Task<List<ProductViewModel>> GetFeaturedProduct(int take)
        {
            try
            {
                var query = from p in _context.Products
                            where p.isDelete == false && p.featured == true
                            select new { p };

                var data = query.OrderByDescending(m => m.p.create_at)
                    .Take(take)
                    .Select(a => new ProductViewModel()
                    {
                        id = a.p.id,
                        name = a.p.name,
                        best_seller = a.p.best_seller,
                        brand_id = a.p.brand_id,
                        code = a.p.code,
                        create_at = a.p.create_at,
                        descriptions = a.p.descriptions,
                        featured = a.p.featured,
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
                        isActive = a.p.isActive,
                        unit_price = a.p.unit_price,
                        warranty = a.p.warranty,
                    }).ToListAsync();

                foreach (var pro in await data)
                {
                    if (pro.image != null)
                    {
                        ImageListResult image = new ImageListResult();
                        pro.image = GetBase64StringForImage(_storageService.GetFileUrl(pro.image));
                    }
                }

                return await data;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<ProductViewModel>> GetProductsByCategory(int id, int take)
        {
            try
            {
                var query = from p in _context.Products
                            join pic in _context.CategoryProducts on p.id equals pic.product_id
                            join c in _context.Categories on pic.cate_id equals c.id
                            where p.isDelete == false && c.id == id
                            select new { p, pic, c };

                var data = query.OrderByDescending(m => m.p.create_at)
                    .Take(take)
                    .Select(a => new ProductViewModel()
                    {
                        id = a.p.id,
                        name = a.p.name,
                        best_seller = a.p.best_seller,
                        brand_id = a.p.brand_id,
                        code = a.p.code,
                        create_at = a.p.create_at,
                        descriptions = a.p.descriptions,
                        featured = a.p.featured,
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
                        isActive = a.p.isActive,
                        unit_price = a.p.unit_price,
                        warranty = a.p.warranty,
                    }).ToListAsync();

                foreach (var pro in await data)
                {
                    if (pro.image != null)
                    {
                        ImageListResult image = new ImageListResult();
                        pro.image = GetBase64StringForImage(_storageService.GetFileUrl(pro.image));
                    }
                }

                return await data;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<ProductViewModel>> GetProductsRelated(int id, int take)
        {
            try
            {
                var query = from p in _context.Products
                            join pic in _context.CategoryProducts on p.id equals pic.product_id
                            join c in _context.Categories on pic.cate_id equals c.id
                            where p.isDelete == false && p.id == id
                            select new { pic };
                var first = await query.FirstAsync();

               var query2 = from p in _context.Products
                            join pic in _context.CategoryProducts on p.id equals pic.product_id
                             join c in _context.Categories on pic.cate_id equals c.id
                             where p.isDelete == false && p.id != first.pic.product_id && c.id == first.pic.cate_id
                             select new { p };


                var data = query2.OrderByDescending(m => m.p.create_at)
                    .Take(take)
                    .Select(a => new ProductViewModel()
                    {
                        id = a.p.id,
                        name = a.p.name,
                        best_seller = a.p.best_seller,
                        brand_id = a.p.brand_id,
                        code = a.p.code,
                        create_at = a.p.create_at,
                        descriptions = a.p.descriptions,
                        featured = a.p.featured,
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
                        isActive = a.p.isActive,
                        unit_price = a.p.unit_price,
                        warranty = a.p.warranty,
                    }).ToListAsync();

                foreach (var pro in await data)
                {
                    if (pro.image != null)
                    {
                        ImageListResult image = new ImageListResult();
                        pro.image = GetBase64StringForImage(_storageService.GetFileUrl(pro.image));
                    }
                }

                return await data;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<ProductViewModel>> GetBestSellerProduct(int take)
        {
            try
            {
                var query = from p in _context.Products
                            where p.isDelete == false && p.best_seller == true
                            select new { p };

                var data = query.OrderByDescending(m => m.p.create_at)
                    .Take(take)
                    .Select(a => new ProductViewModel()
                    {
                        id = a.p.id,
                        name = a.p.name,
                        best_seller = a.p.best_seller,
                        brand_id = a.p.brand_id,
                        code = a.p.code,
                        create_at = a.p.create_at,
                        descriptions = a.p.descriptions,
                        featured = a.p.featured,
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
                        isActive = a.p.isActive,
                        unit_price = a.p.unit_price,
                        warranty = a.p.warranty,
                    }).ToListAsync();

                foreach (var pro in await data)
                {
                    if (pro.image != null)
                    {
                        ImageListResult image = new ImageListResult();
                        pro.image = GetBase64StringForImage(_storageService.GetFileUrl(pro.image));
                    }
                }

                return await data;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<ImageListResult>> GetImagesByProductID(int id)
        {
            List<ImageListResult> ImagesResult = new List<ImageListResult>();
            var product = await _context.Products.FindAsync(id);
            if (product != null || product.isDelete)
            {
                if (product.image != null)
                {
                    ImageListResult image = new ImageListResult();
                    string imageBase64 = GetBase64StringForImage(_storageService.GetFileUrl(product.image));
                    image.FileName = product.image;
                    image.FileAsBase64 = imageBase64;
                    ImagesResult.Add(image);
                }
                if (product.more_images != null)
                {
                    List<string> moreImagesName = product.more_images.Split(",").ToList();
                    foreach (string moreimage in moreImagesName)
                    {
                        if (moreimage != "")
                        {
                            string filePath = _storageService.GetFileUrl(moreimage);
                            if (filePath != "")
                            {
                                ImageListResult images = new ImageListResult();
                                string imageBase64 = GetBase64StringForImage(filePath);
                                images.FileName = moreimage;
                                images.FileAsBase64 = imageBase64;
                                ImagesResult.Add(images);
                            }
                        }
                    }
                }
                return ImagesResult;
            }
            return ImagesResult;
        }
        protected static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
        public async Task<ApiResult<ProductViewModel>> GetById(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.isDelete)
            {
                return new ApiErrorResult<ProductViewModel>("Sản phẩm không tồn tại");
            }
            string CateIds = "";
            var pic = await _context.CategoryProducts.Where(x => x.product_id == productId).ToListAsync();
            if (pic != null)
            {
                foreach (var cate in pic)
                {
                    CateIds += cate.cate_id + ",";
                }
            }

            var productViewModel = new ProductViewModel
            {
                id = product.id,
                name = product.name,
                best_seller = product.best_seller,
                brand_id = product.brand_id,
                CateID = CateIds,
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
                isActive = product.isActive,
                unit_price = product.unit_price,
                warranty = product.warranty,
            };
            return new ApiSuccessResult<ProductViewModel>(productViewModel);
        }
        public async Task<ApiResult<ProductViewModel>> GetBySlug(string slug)
        {
            var product = await _context.Products.Where(x=> x.slug.Equals(slug)).FirstOrDefaultAsync();
            if (product == null || product.isDelete)
            {
                return new ApiErrorResult<ProductViewModel>("Sản phẩm không tồn tại");
            }
            string CateIds = "";
            var pic = await _context.CategoryProducts.Where(x => x.product_id == product.id).ToListAsync();
            if (pic != null)
            {
                foreach (var cate in pic)
                {
                    CateIds += cate.cate_id + ",";
                }
            }

            var productViewModel = new ProductViewModel
            {
                id = product.id,
                name = product.name,
                best_seller = product.best_seller,
                brand_id = product.brand_id,
                CateID = CateIds,
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
                isActive = product.isActive,
                unit_price = product.unit_price,
                warranty = product.warranty,
            };
            return new ApiSuccessResult<ProductViewModel>(productViewModel);
        }
        public async Task<bool> isValidSlug(string Code, string slug)
        {
            if (await _context.Products.AnyAsync(x => x.slug.Equals(slug) && !x.code.Equals(Code) && x.isDelete == false))
                return false;
            return true;
        }
        public async Task<ApiResult<bool>> Update(ProductUpdateRequest request)
        {
            try
            {
                var product = await _context.Products.FindAsync(request.Id);
                if (product == null || product.isDelete) return new ApiErrorResult<bool>($"Không tìm thấy sản phẩm: {request.Id}");

                product.name = request.Name;
                product.slug = request.Slug;
                if (request.Warranty == null)
                    product.warranty = 0;
                else product.warranty = (int)request.Warranty;
                product.brand_id = request.Brand_id;
                product.specifications = request.Specifications;
                product.short_desc = request.Short_desc;
                product.descriptions = request.Descriptions;
                product.isActive = request.IsActive;
                product.meta_descriptions = request.Meta_descriptions;
                product.meta_keywords = request.Meta_keywords;
                product.unit_price = request.Unit_price;
                if (request.Promotion_price == null)
                    product.promotion_price = 0;
                else product.promotion_price = (decimal)request.Promotion_price;
                product.best_seller = request.Best_seller;
                product.featured = request.Featured;
                product.instock = request.Instock;
                product.brand_id = request.Brand_id;
                product.meta_tittle = request.Meta_tittle;
                product.code = request.Code;
                product.update_at = DateTime.Now;
                if (request.Image != null)
                {
                    await _storageService.DeleteFileAsync(product.image);
                    product.image = await this.SaveFile(request.Image);
                }
                if (request.More_images != null)
                {
                    for (int i = 0; i < request.More_images.Count(); i++)
                    {
                        product.more_images += await this.SaveFile(request.More_images[i]) + ",";
                    }
                }

                var pic = await _context.CategoryProducts.Where(x => x.product_id == request.Id).ToListAsync();
                if (pic != null)
                {
                    foreach (var cate in pic)
                    {
                        _context.CategoryProducts.Remove(cate);
                    }
                }

                string[] cateIDs = request.CateID.Split(",");
                foreach (string cateID in cateIDs)
                {

                    if (cateID != "")
                    {
                        var productInCategory = new TechShopSolution.Data.Entities.CategoryProduct
                        {
                            cate_id = int.Parse(cateID),
                            product_id = request.Id
                        };
                        _context.CategoryProducts.Add(productInCategory);
                    }
                }
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public async Task<ApiResult<bool>> ChangeStatus(int id)
        {
            try
            {
                var productExist = await _context.Products.FindAsync(id);
                if (productExist != null || productExist.isDelete)
                {
                    if (productExist.isActive)
                        productExist.isActive = false;
                    else productExist.isActive = true;
                    productExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy sản phẩm này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> OffBestSeller(int id)
        {
            try
            {
                var productExist = await _context.Products.FindAsync(id);
                if (productExist != null || productExist.isDelete)
                {
                    if (productExist.best_seller)
                        productExist.best_seller = false;
                    else productExist.best_seller = true;
                    productExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy sản phẩm này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> OffFeatured(int id)
        {
            try
            {
                var productExist = await _context.Products.FindAsync(id);
                if (productExist != null || productExist.isDelete)
                {
                    if (productExist.featured)
                        productExist.featured = false;
                    else productExist.featured = true;
                    productExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy sản phẩm này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
    }
}
