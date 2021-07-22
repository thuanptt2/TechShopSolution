using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechShopSolution.Data.EF;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Catalog.Product;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.Sales;

namespace TechShopSolution.Application.Catalog.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly TechShopDBContext _context;
        public CustomerService(TechShopDBContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> Create(CustomerCreateRequest request)
        {
            try
            {
                string addressCustomer = request.House + " " + request.Ward + ", " + request.District + ", " + request.City;
                var customer = new TechShopSolution.Data.Entities.Customer
                {
                    name = request.name,
                    address = addressCustomer,
                    birthday = request.birthday,
                    email = request.email,
                    password = request.password,
                    phone = request.phone,
                    sex = request.sex,
                    isActive = request.isActive,
                    isDelete = false,
                    create_at = DateTime.Now
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch
            {
                return new ApiErrorResult<bool>("Thêm thất bại");
            }
        }
        public async Task<ApiResult<bool>> Register(CustomerRegisterRequest request)
        {
            try
            {
                var customer = new TechShopSolution.Data.Entities.Customer
                {
                    name = request.name,
                    birthday = request.birthday,
                    email = request.email,
                    password = request.password,
                    phone = request.phone,
                    address = "",
                    sex = request.sex,
                    isActive = true,
                    isDelete = false,
                    create_at = DateTime.Now
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>();
            }
            catch
            {
                return new ApiErrorResult<bool>("Đăng ký thất bại");
            }
        }
        public async Task<ApiResult<bool>> Delete(int cusID)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(cusID);
                if (customer != null)
                {
                    if (await _context.Orders.AnyAsync(x => x.cus_id == customer.id))
                        return new ApiErrorResult<bool>($"Khách hàng này đang có đơn hàng đấy, không thể xóa!");
                    customer.isDelete = true;
                    customer.delete_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>($"Khách hàng không tồn tại");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
        }
        public async Task<PagedResult<CustomerViewModel>> GetAllPaging(GetCustomerPagingRequest request)
        {
            var query = from cus in _context.Customers
                        where cus.isDelete == false
                        select cus;

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.name.Contains(request.Keyword) || x.phone.Contains(request.Keyword) || x.email.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = query.OrderByDescending(m => m.create_at)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new CustomerViewModel()
                {
                    id = a.id,
                    address = a.address,
                    birthday = a.birthday,
                    email = a.email,
                    name = a.name,
                    password = a.password,
                    phone = a.phone,
                    sex = a.sex,
                    isActive = a.isActive,
                }).ToListAsync();

            var pageResult = new PagedResult<CustomerViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = await data,
            };
            return pageResult;
        }
        public async Task<ApiResult<CustomerViewModel>> GetById(int customertId)
        {
            var cusExist = await _context.Customers.FindAsync(customertId);
            if (cusExist == null || cusExist.isDelete)
            {
                return new ApiErrorResult<CustomerViewModel>("Khách hàng không tồn tại");
            }
            var customer = new CustomerViewModel()
            {
                name = cusExist.name,
                address = cusExist.address,
                birthday = cusExist.birthday,
                email = cusExist.email,
                password = cusExist.password,
                phone = cusExist.phone,
                sex = cusExist.sex,
                isActive = cusExist.isActive,
                id = cusExist.id
            };
            return new ApiSuccessResult<CustomerViewModel>(customer);
        }
        public async Task<ApiResult<bool>> Update(int id, CustomerUpdateRequest request)
        {
            try
            {
                if (await _context.Customers.AnyAsync(x => x.email == request.email && x.id != id))
                {
                    return new ApiErrorResult<bool>("Emai đã tồn tại");
                }
                var cusExist = await _context.Customers.FindAsync(request.Id);
                if(cusExist!=null || cusExist.isDelete)
                {
                    cusExist.email = request.email;
                    cusExist.name = request.name;
                    cusExist.phone = request.phone;
                    cusExist.isActive = request.isActive;
                    cusExist.birthday = request.birthday;
                    cusExist.sex = request.sex;
                    cusExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy khách hàng này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> UpdatePublic(int id, CustomerPublicUpdateRequest request)
        {
            try
            {
                var cusExist = await _context.Customers.FindAsync(request.Id);
                if (cusExist != null || cusExist.isDelete)
                {
                    cusExist.email = request.email;
                    cusExist.name = request.name;
                    cusExist.phone = request.phone;
                    cusExist.birthday = request.birthday;
                    cusExist.sex = request.sex;
                    cusExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy tài khoản này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> UpdateAddress(int id, CustomerUpdateAddressRequest request)
        {
            try
            {
                var cusExist = await _context.Customers.FindAsync(id);
                if (cusExist != null || cusExist.isDelete)
                {
                    string newAddress = request.House + " " + request.Ward + ", " + request.District + ", " + request.City;
                    cusExist.address = newAddress;
                    cusExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy khách hàng này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> ChangeStatus(int id)
        {
            try
            {
                var cusExist = await _context.Customers.FindAsync(id);
                if (cusExist != null || cusExist.isDelete)
                {
                    if (cusExist.isActive)
                        cusExist.isActive = false;
                    else cusExist.isActive = true;
                    cusExist.update_at = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<bool>();
                }
                else return new ApiErrorResult<bool>("Không tìm thấy khách hàng này");
            }
            catch
            {
                return new ApiErrorResult<bool>("Cập nhật thất bại");
            }
        }
        public async Task<ApiResult<bool>> FavoriteProduct(int cus_id, int product_id)
        {
            var product = await _context.Products.FindAsync(product_id);
            if (product == null || product.isDelete == true)
                return new ApiErrorResult<bool>("Sản phẩm này hiện không còn tồn tại");
            if (!product.isActive)
                return new ApiErrorResult<bool>("Sản phẩm này đang bị khóa, bạn không thể yêu thích sản phẩm này");
            var favorite = new TechShopSolution.Data.Entities.Favorite
            {
                cus_id = cus_id,
                product_id = product_id,
                date_favorite = DateTime.Now
            };
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> UnFavoriteProduct(int cus_id, int product_id)
        {
            var favorite = await _context.Favorites.Where(x=>x.cus_id == cus_id && x.product_id == product_id).FirstOrDefaultAsync();
            if (favorite == null)
                return new ApiErrorResult<bool>("Không tìm thấy yêu thích của bạn");
           
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> RatingProduct(ProductRatingRequest request)
        {
            var rating = await _context.Ratings.Where(x => x.cus_id == request.cus_id && x.product_id == request.product_id).FirstOrDefaultAsync();
            if (rating != null)
                return new ApiErrorResult<bool>("Bạn đã đánh giá sản phẩm này rồi, không thể đánh giá thêm lần nữa");
            var product = await _context.Products.FindAsync(request.product_id);
            if (product == null || product.isDelete == true)
                return new ApiErrorResult<bool>("Sản phẩm bạn đánh giá hiện không còn tồn tại");
            if (!product.isActive)
                return new ApiErrorResult<bool>("Sản phẩm này đang bị khóa, bạn không thể đánh giá sản phẩm này");
            var customer = await _context.Customers.FindAsync(request.cus_id);
            

            var newRating = new TechShopSolution.Data.Entities.Rating()
            {
                content = request.content,
                cus_id = request.cus_id,
                date_rating = DateTime.Now,
                product_id = request.product_id,
                score = request.score
            };
            await _context.Ratings.AddAsync(newRating);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<bool> VerifyEmail(string email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.email.Equals(email) && x.isDelete == false);
            if(customer!=null)
            {
                return false;
            }
            return true;
        }
        public List<OrderViewModel> GetLatestOrder(int id,int take)
        {
            var query = from o in _context.Orders
                        join od in _context.OrDetails on o.id equals od.order_id
                        where o.cus_id == id
                        select new { o };

            var data = query.AsEnumerable()
                   .GroupBy(g => g.o);

            var result = data.OrderByDescending(x => x.Key.create_at)
                .Take(take)
                .Select(a => new OrderViewModel
                {
                    create_at = a.Key.create_at,
                    cus_id = a.Key.cus_id,
                    discount = a.Key.discount,
                    id = a.Key.id,
                    isPay = a.Key.isPay,
                    name_receiver = a.Key.name_receiver,
                    status = a.Key.status,
                    total = a.Key.total,
                    transport_fee = a.Key.transport_fee
                }).ToList();

            foreach (var item in result)
            {
                var tranport = _context.Transports.Where(x => x.order_id == item.id).FirstOrDefault();
                if (tranport != null)
                {
                    item.ship_status = tranport.ship_status;
                }
                else item.ship_status = 0;
            }
            return result;
        }
        public PagedResult<ProductOverViewModel> GetFavoriteProduct(GetFavoriteProductsPagingRequest request)
        {
            try
            {
                var query = from p in _context.Products
                            join f in _context.Favorites on p.id equals f.product_id
                            join cus in _context.Customers on f.cus_id equals cus.id
                            where p.isDelete == false && p.isActive == true && cus.id == request.cus_id
                            select new { p, f, cus };


                var data = query.AsEnumerable()
                    .OrderByDescending(p => p.f.date_favorite)
                    .GroupBy(g => g.p);

                int totalRow = data.Count();

                List<ProductOverViewModel> result = data.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(a => new ProductOverViewModel()
                    {
                        id = a.Key.id,
                        name = a.Key.name,
                        best_seller = a.Key.best_seller,
                        create_at = a.Key.create_at,
                        featured = a.Key.featured,
                        image = a.Key.image,
                        instock = a.Key.instock,
                        promotion_price = a.Key.promotion_price,
                        short_desc = a.Key.short_desc,
                        slug = a.Key.slug,
                        isActive = a.Key.isActive,
                        unit_price = a.Key.unit_price,
                    }).ToList();


                var pageResult = new PagedResult<ProductOverViewModel>()
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
                var pageResult = new PagedResult<ProductOverViewModel>()
                {
                    TotalRecords = 0,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = null,
                };
                return pageResult;
            }
        }

    }
}
