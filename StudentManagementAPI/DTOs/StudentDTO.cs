using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.DTOs
{
    public record StudentDTO
    {
        [Required(ErrorMessage = "Student Name is required"), StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be of minmum of 3 characters and maximum of 60 characters.")]
        public string StudentName { get; init; }
        public int Age { get; init; }

        [AllowedValues("A", "B", "C", "D", "E")]
        public string Grade { get; init; }
    }
}
