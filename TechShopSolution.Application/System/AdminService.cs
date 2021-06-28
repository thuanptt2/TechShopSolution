using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TechShopSolution.Data.EF;
using TechShopSolution.Data.Entities;
using TechShopSolution.ViewModels.Common;
using TechShopSolution.ViewModels.System;

namespace TechShopSolution.Application.System
{
    public class AdminService : IAdminService
    {
        private readonly TechShopDBContext _context;
        private readonly IConfiguration _config;

        public AdminService(TechShopDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public string Authenticate(LoginRequest request)
        {
            Admin admin = _context.Admins.FirstOrDefault(x=>x.email == request.Email);
            if (admin == null)
            {
                return null;
            }
            if (admin.password != request.Password)
            {
                return null;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,admin.email),
                new Claim(ClaimTypes.Name,admin.name),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public ApiResult<string> AuthenticateCustomer(LoginRequest request)
        {
            Customer customer = _context.Customers.FirstOrDefault(x => x.email == request.Email);
            if(customer == null)
            {
                return new ApiErrorResult<string>("Tài khoản này không tồn tại");
            }
            else
            {
                if (customer.isDelete == true)
                {
                    return new ApiErrorResult<string>("Tài khoản này đã bị xóa. Quý khách vui lòng liên hệ với QTV để biết thêm thông tin.");
                }
                if (customer.isActive == false)
                {
                    return new ApiErrorResult<string>("Tài khoản này đang bị khóa. Quý khách vui lòng liên hệ với QTV để biết thêm thông tin.");
                }
                if (customer.password != request.Password)
                {
                    return new ApiErrorResult<string>("Sai mật khẩu.");
                }
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,customer.email),
                new Claim(ClaimTypes.Name,customer.name),
                new Claim(ClaimTypes.Sid,customer.id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            string resulToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new ApiSuccessResult<string>(resulToken);
        }
    }
}
