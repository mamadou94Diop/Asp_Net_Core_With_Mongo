using System;
using System.ComponentModel.DataAnnotations;

namespace DriveMeShop.CustomAnnotations
{
    public class YearStartingByAttribute : ValidationAttribute
    {
        public int StartingYear {get; set;}

        private readonly int currentYear = DateTime.Now.Year;
        

        public override bool IsValid(object value)
        {
            var year = (int)value;
            return year >= StartingYear && year <= currentYear;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Year value should be between {StartingYear} and {currentYear}";
        }
    }
}
