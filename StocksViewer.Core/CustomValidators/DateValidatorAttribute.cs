using System.ComponentModel.DataAnnotations;

namespace CustomValidators
{
    public class DateValidatorAttribute : ValidationAttribute
    {
        public int MinimumYear { get; set; } = 2000;
        public string DefaultErrorMessage { get; set; } = "Date should not be less than {0}";

        public DateValidatorAttribute() { }
        public DateValidatorAttribute(int minimumYear) {
            MinimumYear = minimumYear;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime givenDate = (DateTime) value;
                if (givenDate.Year < MinimumYear)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, MinimumYear));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return null;
        }
    }
}
