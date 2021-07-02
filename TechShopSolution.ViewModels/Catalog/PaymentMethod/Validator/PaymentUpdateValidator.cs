using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.PaymentMethod.Validator
{
    public class PaymentUpdateValidator : AbstractValidator<PaymentUpdateRequest>
    {
        public PaymentUpdateValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Tên phương thức không được để trống")
                                .MaximumLength(255).WithMessage("Tên thương hiệu không thể vượt quá 255 kí tự");
        }
    }
}
