using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TechShopSolution.ViewModels.Website.Slide;

namespace TechShopSolution.ViewModels.Website.Slide.Validator
{
    public class SlideCreateValidator : AbstractValidator<SlideCreateRequest>
    {
        public SlideCreateValidator()
        {
            RuleFor(x => x.link).NotEmpty().WithMessage("Link liên kết không thể để trống");
            RuleFor(x => x.image).NotEmpty().WithMessage("Hình ảnh không được để trống");
        }
    }
}
