using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Category.Validator
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.cate_name).NotEmpty().WithMessage("Tên loại sản phẩm không được để trống")
                  .MaximumLength(255).WithMessage("Tên loại sản phẩm không thể vượt quá 255 kí tự");
            RuleFor(x => x.cate_slug).NotEmpty().WithMessage("Nhập đường dẫn cho thương hiệu")
                  .MaximumLength(255).WithMessage("Đường dẫn không thể vượt quá 255 kí tự");
        }
    }
}
