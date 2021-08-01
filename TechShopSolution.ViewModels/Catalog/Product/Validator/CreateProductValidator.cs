using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Product.Validator
{
    public class CreateProductValidator : AbstractValidator<ProductCreateRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên sản phẩn không được để trống")
                   .MaximumLength(150).WithMessage("Tên sản phẩm không thể vượt quá 150 kí tự");
            RuleFor(x => x.Image).NotNull().WithMessage("Chưa chọn hình ảnh cho sản phẩm");
            RuleFor(x => x.Brand_id).NotNull().WithMessage("Chưa chọn thương hiệu cho sản phẩm");
            RuleFor(x => x.CateID).NotNull().WithMessage("Chưa chọn loại sản phẩm");
            RuleFor(x => x.Unit_price).NotEmpty().WithMessage("Vui lòng nhập giá cho sản phẩm");
            RuleFor(x => x.Promotion_price).Must((o, value) => { return BeAValidPromotionPrice(value, o.Unit_price); }).WithMessage("Giá khuyến mãi không thể lớn hơn giá giá gốc");

            RuleFor(x => x.Slug).NotEmpty().WithMessage("Nhập đường dẫn cho sản phẩm")
                  .MaximumLength(150).WithMessage("Đường dẫn không thể vượt quá 150 kí tự");
        }
        protected bool BeAValidPromotionPrice(string value, string Unit_price)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (decimal.Parse(Unit_price) < decimal.Parse(value))
                    return false;
                else return true;
            }
            return true;
        }
    }
}
