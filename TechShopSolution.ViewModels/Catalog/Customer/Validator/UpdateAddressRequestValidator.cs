using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.ViewModels.Catalog.Customer.Validator
{
    public class UpdateAddressRequestValidator : AbstractValidator<CustomerUpdateAddressRequest>
    {
        public UpdateAddressRequestValidator()
        {
            RuleFor(x => x.City).NotEmpty().WithMessage("Ngày sinh không được để trống");
            RuleFor(x => x.District).NotEmpty().WithMessage("Ngày sinh không được để trống");
            RuleFor(x => x.Ward).NotEmpty().WithMessage("Ngày sinh không được để trống");
        }
    }
}
