using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Model
{
    public class Teacher
    {
        [Key]
        [Required(ErrorMessage = "Teacher Email cannot be empty"), EmailAddress(ErrorMessage ="Email should be in the correct format")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Hashed password cannot be empty")]
        public string Passwordhash { get; set; }
    }
}
