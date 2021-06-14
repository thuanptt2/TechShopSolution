using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Brand.Validator
{
    public class UpdateBrandValidator : AbstractValidator<BrandUpdateRequest>
    {
        public UpdateBrandValidator()
        {
            RuleFor(x => x.brand_name).NotEmpty().WithMessage("Tên thương hiệu không được để trống")
                  .MaximumLength(255).WithMessage("Tên thương hiệu không thể vượt quá 255 kí tự");
            RuleFor(x => x.brand_slug).NotEmpty().WithMessage("Nhập đường dẫn cho thương hiệu")
                  .MaximumLength(255).WithMessage("Đường dẫn không thể vượt quá 255 kí tự");
        }
    }
}
