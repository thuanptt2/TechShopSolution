using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Transport.Validator
{
    public class TransporterCreateValidator : AbstractValidator<TransporterCreateRequest>
    {
        public TransporterCreateValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Tên đơn vị vận chuyển không được để trống")
                                .MaximumLength(255).WithMessage("Tên thương hiệu không thể vượt quá 255 kí tự");
            RuleFor(x => x.link).NotEmpty().WithMessage("Link tra cứu giúp người dung tra cứu đơn hàng của đơn vị vận chuyển, không thể để trống");
            RuleFor(x => x.image).NotEmpty().WithMessage("Hình ảnh không được để trống");
        }
    }
}
