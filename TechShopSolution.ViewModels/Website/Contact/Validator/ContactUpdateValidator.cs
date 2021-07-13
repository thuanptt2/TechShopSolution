using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Website.Contact.Validator
{
    public class ContactUpdateValidator : AbstractValidator<ContactUpdateRequest>
    {
        public ContactUpdateValidator()
        {
            RuleFor(x => x.company_name).NotEmpty().WithMessage("Tên công ty không được để trống")
                                                .MaximumLength(255).WithMessage("Tên công ty không thể vượt quá 255 kí tự");
            RuleFor(x => x.adress).NotEmpty().WithMessage("Địa chỉ không được để trống");
            RuleFor(x => x.email).NotEmpty().WithMessage("Email không được để trống")
                .MaximumLength(50).WithMessage("Email không thể vượt quá 50 kí tự");
            RuleFor(x => x.phone).NotEmpty().WithMessage("Số điện thoại không được để trống")
                .MinimumLength(10).WithMessage("Số điện thoại phải ít nhất 10 ký tự");
        }
    }
}
