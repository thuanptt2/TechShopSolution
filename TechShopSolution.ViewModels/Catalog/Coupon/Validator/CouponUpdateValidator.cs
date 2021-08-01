using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Coupon.Validator
{
    public class CouponUpdateValidator : AbstractValidator<CouponUpdateRequest>
    {
        public CouponUpdateValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Tên mã giảm giá không được để trống")
                                .MaximumLength(128).WithMessage("Tên mã giảm giá không thể vượt quá 128 kí tự");
            RuleFor(x => x.start_at).NotEmpty().WithMessage("Vui lòng chọn ngày bắt đầu")
                  .LessThan(m => m.end_at).WithMessage("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
            RuleFor(x => x.end_at).NotEmpty().WithMessage("Vui lòng chọn ngày kết thúc")
                .GreaterThan(m => m.start_at).WithMessage("Ngày kết thúc phải lớn hơn ngày bắt đầu");
        }
    }
}
