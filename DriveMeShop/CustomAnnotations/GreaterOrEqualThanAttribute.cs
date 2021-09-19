using System;
using System.ComponentModel.DataAnnotations;

namespace DriveMeShop.CustomAnnotations
{
    public class GreaterOrEqualThanAttribute: ValidationAttribute
    {
        public int Minimum { get; set; }
        public override bool IsValid(object value)
        {
            return (int)value >= Minimum;
        }

        public override string FormatErrorMessage(string name)
        {
            return  $"Should be greater or equal to {Minimum}";
        }
    }
}
