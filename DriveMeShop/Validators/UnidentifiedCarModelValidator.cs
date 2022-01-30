using System;
using DriveMeShop.Model;
using FluentValidation;

namespace DriveMeShop.Validators
{
    public class UnidentifiedCarModelValidator: AbstractValidator<UnidentifiedCarModel>
    {

        public UnidentifiedCarModelValidator()
        {
            RuleFor(carModel => carModel)
                .Must(IsReleasedYearBeforeLastRevisionYear)
                .WithMessage("LastRevisionYear should not be before ReleasedYear");

            RuleFor(carModel => carModel)
                .Must(AreModelAndMakeDifferent)
                .WithMessage("Make and Model should not be the same"); 
        }

        private bool AreModelAndMakeDifferent(UnidentifiedCarModel carModel)
        {
            if (carModel.Make != null && carModel.Model != null)
            {
                return carModel.Make.ToLower() != carModel.Model.ToLower();
            }
            return true;
        }

        private bool IsReleasedYearBeforeLastRevisionYear(UnidentifiedCarModel carModel)
        {
            return (carModel.LastRevisionYear == null ) || (carModel.ReleasedYear <= carModel.LastRevisionYear);
        }
    }
}
