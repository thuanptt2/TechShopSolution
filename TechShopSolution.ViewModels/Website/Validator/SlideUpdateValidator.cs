using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Website.Validator
{
    public class SlideUpdateValidator : AbstractValidator<SlideUpdateRequest>
    {
        public SlideUpdateValidator()
        {
            RuleFor(x => x.link).NotEmpty().WithMessage("Link liên kết không thể để trống");
            RuleFor(x => x.display_order).NotEmpty().WithMessage("Thứ tự không thể để trống");
        }
    }
}
