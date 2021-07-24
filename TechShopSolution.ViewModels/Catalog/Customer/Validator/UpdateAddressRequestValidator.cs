using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Customer.Validator
{
    public class UpdateAddressRequestValidator : AbstractValidator<CustomerUpdateAddressRequest>
    {
        public UpdateAddressRequestValidator()
        {
            RuleFor(x => x.City).NotEmpty().WithMessage("Tỉnh/Thành phố không được để trống");
            RuleFor(x => x.District).NotEmpty().WithMessage("Quận/Huyện không được để trống");
            RuleFor(x => x.Ward).NotEmpty().WithMessage("Phường/xã không được để trống");
            RuleFor(x => x.House).NotEmpty().WithMessage("Vui lòng nhập số nhà, tên đường")
                                .MaximumLength(50).WithMessage("Số nhà, tên đường không thể vượt quá 50 kí tự");
        }
    }
}
