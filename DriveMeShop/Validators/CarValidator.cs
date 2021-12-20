using System;
using DriveMeShop.Model;
using FluentValidation;

namespace DriveMeShop.Validators
{
    public class CarModelValidator: AbstractValidator<CarModel>
    {

        public CarModelValidator()
        {
            RuleFor(carModel => carModel)
                .Must(IsReleasedYearBeforeLastRevisionYear)
                .WithMessage("LastRevisionYear should not be before ReleasedYear");

            RuleFor(carModel => carModel)
                .Must(AreModelAndMakeDifferent)
                .WithMessage("Make and Model should not be the same"); 
        }

        private bool AreModelAndMakeDifferent(CarModel carModel)
        {
            return carModel.Make.ToLower() != carModel.Model.ToLower();
        }

        private bool IsReleasedYearBeforeLastRevisionYear(CarModel carModel)
        {
            return (carModel.LastRevisionYear == null ) || (carModel.ReleasedYear <= carModel.LastRevisionYear);
        }
    }
}
