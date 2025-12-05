using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.CustomValidations
{
    public class AllowedValuesAttribute : ValidationAttribute
    {
        private readonly string[] _allowedvalues;

        public AllowedValuesAttribute(params string[] allowedvalues)
        {
            _allowedvalues = allowedvalues;
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            return _allowedvalues.Contains(value.ToString());
        }
    }
}
