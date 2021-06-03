using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Customer.Validation
{
    public class CreateRequestValidator : AbstractValidator<CustomerCreateRequest>
    {
        public CreateRequestValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Tên không được để trống")
                  .MaximumLength(255).WithMessage("Tên không thể vượt quá 255 kí tự");
            RuleFor(x => x.email).NotEmpty().WithMessage("Email không được để trống")
                  .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email không hợp lệ");
            RuleFor(x => x.password).NotEmpty().WithMessage("Mật khẩu không được để trống")
                  .MinimumLength(6).WithMessage("Mật khẩu phải ít nhất 6 kí tự");
            RuleFor(x => x.phone).NotEmpty().WithMessage("Số điện thoại không được để trống")
                  .MinimumLength(10).WithMessage("Số điện thoại phải ít nhất 10 kí tự")
                  .MaximumLength(11).WithMessage("Số điện thoại không vượt quá 11 kí tự");
            RuleFor(x => x.birthday).NotEmpty().WithMessage("Ngày sinh không được để trống")
                  .Must(BeAValidAge).WithMessage("Ngày sinh không hợp lệ");
            RuleFor(x => x.City).NotEmpty().WithMessage("Tỉnh/Thành phố không được để trống");
            RuleFor(x => x.District).NotEmpty().WithMessage("Quận/Huyện không được để trống");
            RuleFor(x => x.Ward).NotEmpty().WithMessage("Phường/xã không được để trống");
            RuleFor(x => x.House).NotEmpty().WithMessage("Vui lòng nhập số nhà, tên đường");
        }
        protected bool BeAValidAge(DateTime date)
        {
            int currentYear = DateTime.Now.Year;
            int dobYear = date.Year;

            if (dobYear <= currentYear && dobYear > (currentYear - 120))
            {
                return true;
            }

            return false;
        }
    }
}
