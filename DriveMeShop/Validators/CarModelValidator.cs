using System;
using DriveMeShop.Model;
using FluentValidation;

namespace DriveMeShop.Validators
{
    public class CarModelValidator<T> : AbstractValidator<T> where T: BaseCarModel
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

        private bool AreModelAndMakeDifferent(T carModel)
        {
            return carModel.Make.ToLower() != carModel.Model.ToLower();
        }

        private bool IsReleasedYearBeforeLastRevisionYear(T carModel)
        {
            return (carModel.LastRevisionYear == null) || (carModel.ReleasedYear <= carModel.LastRevisionYear);
        }
    }
}