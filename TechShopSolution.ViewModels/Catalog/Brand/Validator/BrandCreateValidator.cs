using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Brand.Validator
{
    public class BrandCreateValidator : AbstractValidator<BrandCreateRequest>
    {
        public BrandCreateValidator()
        {
            RuleFor(x => x.brand_slug).NotEmpty().WithMessage("Tên thương hiệu không được để trống")
                  .MaximumLength(128).WithMessage("Tên thương hiệu không thể vượt quá 128 kí tự");
            RuleFor(x => x.brand_slug).NotEmpty().WithMessage("Nhập đường dẫn cho thương hiệu")
                  .MaximumLength(128).WithMessage("Đường dẫn không thể vượt quá 128 kí tự");
        }
    }
}
