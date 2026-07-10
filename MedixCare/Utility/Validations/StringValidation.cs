namespace MedixCare.Utility.Validations
{
    public class StringValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
   
            if (value is string stringValue)
            {
                return true; // Valid string
            }
            if(string.IsNullOrEmpty(value?.ToString()))
            {
                ErrorMessage = "The field cannot be empty or whitespace.";
                return false;
            }
            return false; // Not a string
        }

        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage ?? $"The field {name} must be a valid string.";
        }
    }
}
