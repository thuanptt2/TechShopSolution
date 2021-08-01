using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Coupon.Validator
{
    public class CouponCreateValidator : AbstractValidator<CouponCreateRequest>
    {
        public CouponCreateValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Tên mã giảm giá không được để trống")
                   .MaximumLength(128).WithMessage("Tên mã giảm giá không thể vượt quá 128 kí tự");
            RuleFor(x => x.code).NotEmpty().WithMessage("Mã giảm giá không được để trống")
                  .MaximumLength(20).WithMessage("Mã giảm giá không được vượt quá 20 kí tự");
            RuleFor(x => x.value).NotEmpty().WithMessage("Giá trị không được để trống")
                    .Must((o, value) => { return BeAValidValuePercent(value, o.type); }).WithMessage("Giá trị giảm phải thuộc trong khoảng từ 1%-100%")
                    .Must((o, value) => { return BeAValidValueAmount(value, o.type); }).WithMessage("Giá trị phải lớn hơn 0");
            RuleFor(x => x.quantity)
                   .GreaterThan(0).WithMessage("Số lượng không hợp lệ");
            RuleFor(x => x.start_at).NotEmpty().WithMessage("Vui lòng chọn ngày bắt đầu")
                .LessThan(m => m.end_at).WithMessage("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
            RuleFor(x => x.end_at).NotEmpty().WithMessage("Vui lòng chọn ngày kết thúc")
                .GreaterThan(m => m.start_at).WithMessage("Ngày kết thúc phải lớn hơn ngày bắt đầu");
        }
        protected bool BeAValidValuePercent(double value, string type)
        {
           if(type.Equals("Phần trăm"))
           {
                if (value > 0 && value <= 100)
                    return true;
                else return false;
           }
            return true;
        }
        protected bool BeAValidValueAmount(double value, string type)
        {
            if (type.Equals("Số tiền"))
            {
                if (value > 0)
                    return true;
                else return false;
            }
            return true;
        }
    }
}
