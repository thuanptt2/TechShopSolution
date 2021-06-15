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
                promotion_price = request.Promotion_price,
                short_desc = request.Short_desc,
                slug = request.Slug,
                specifications = request.Specifications,
                isActive = request.IsActive,
                isDelete = false,
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
                         product.more_images += await this.SaveFile(request.More_images[i]) + ",";
                    }
                }
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            string[] cateIDs = request.CateID.Split(" ");
            foreach (string cateID in cateIDs)
            {
                var productInCategory = new TechShopSolution.Data.Entities.CategoryProduct
                {
                    cate_id = int.Parse(cateID),
                    product_id = product.id
                };
                _context.CategoryProducts.Add(productInCategory);
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
            if(product!=null)
            {
                List<string> moreImagesName = product.more_images.Split(",").ToList();
                bool flag = false;
                foreach (string moreimage in moreImagesName)
                {
                    if(moreimage!="")
                    {
                        if (moreimage.Equals(fileName))
                        {
                            moreImagesName.Remove(moreimage);
                            string MoreImageAfterDelete = null;
                            foreach (string imagestr in moreImagesName)
                            {
                                if(imagestr!="")
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
            var query = from p in _context.Products
                        join pic in _context.CategoryProducts on p.id equals pic.product_id
                        join ct in _context.Categories on pic.cate_id equals ct.id
                        where p.isDelete == false
                        select new { p, pic };
            if (!String.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.p.name.Contains(request.Keyword));
            if (request.CategoryID != null)
            {
                query = query.Where(x => x.pic.cate_id == request.CategoryID);
            }
            if (request.BrandID != null)
            {
                query = query.Where(x => x.p.brand_id == request.BrandID);
            }
            int totalRow = await query.CountAsync();

            var data = query.OrderByDescending(m => m.p.create_at)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
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

            var pageResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = await data,
            };
            return pageResult;
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
                        if(moreimage != "")
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

            var productViewModel = new ProductViewModel
            {
                id = product.id,
                name = product.name,
                best_seller = product.best_seller,
                brand_id = product.brand_id,
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
            if(await _context.Products.AnyAsync(x => x.slug.Equals(slug) && !x.code.Equals(Code) && x.isDelete == false))
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
                product.warranty = request.Warranty;
                product.specifications = request.Specifications;
                product.short_desc = request.Short_desc;
                product.descriptions = request.Descriptions;
                product.isActive = request.IsActive;
                product.meta_descriptions = request.Meta_descriptions;
                product.meta_keywords = request.Meta_keywords;
                product.unit_price = request.Unit_price;
                product.promotion_price = request.Promotion_price;
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
                        product.more_images += await this.SaveFile(request.More_images[i]) + "," ;
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
