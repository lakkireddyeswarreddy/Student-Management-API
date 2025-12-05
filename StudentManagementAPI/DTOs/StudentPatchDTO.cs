using StudentManagementAPI.CustomValidations;

namespace StudentManagementAPI.DTOs
{
    public record StudentPatchDTO
    {
        public int Age { get; init; }

        [AllowedValues("A", "B", "C", "D", "E")]
        public string Grade { get; init; }
    }
}
