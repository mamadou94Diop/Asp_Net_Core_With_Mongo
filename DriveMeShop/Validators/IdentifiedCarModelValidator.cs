using DriveMeShop.Model;
using FluentValidation;

namespace DriveMeShop.Validators
{
    public class IdentifiedCarModelValidator : AbstractValidator<IdentifiedCarModel>
    {
        public IdentifiedCarModelValidator()
        {
            RuleFor(identifiedCarModel => identifiedCarModel).SetValidator(new UnidentifiedCarModelValidator());
        }
    }
}
