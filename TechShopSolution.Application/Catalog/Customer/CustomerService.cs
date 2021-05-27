using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.Data.EF;
using TechShopSolution.Utilities.Exceptions;
using TechShopSolution.ViewModels.Catalog.Customer;
using TechShopSolution.ViewModels.Common;

namespace TechShopSolution.Application.Catalog.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly TechShopDBContext _context;
        public CustomerService(TechShopDBContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(CustomerCreateRequest request)
        {
            try
            {
                string addressCustomer = request.House + ", " + request.Ward + ", " + request.District + ", " + request.City;
                var customer = new TechShopSolution.Data.Entities.Customer
                {
                    name = request.name,
                    address = addressCustomer,
                    birthday = request.birthday,
                    email = request.email,
                    password = request.password,
                    phone = request.phone,
                    sex = request.sex,
                    status = request.status,
                    Order = new List<Data.Entities.Order>(),
                    create_at = DateTime.Now
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<int> Delete(int cusID)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<CustomerViewModel>> GetAllPaging(GetCustomerPagingRequest request)
        {
            var query = from cus in _context.Customers
                        select cus;

            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.name.Contains(request.Keyword) || x.phone.Contains(request.Keyword) || x.email.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
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
                    status = a.status,
                }).ToListAsync();

            var pageResult = new PagedResult<CustomerViewModel>()
            {
                TotalRecord = totalRow,
                Items = await data,
            };
            return pageResult;
        }

        public Task<CustomerViewModel> GetById(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(CustomerUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
