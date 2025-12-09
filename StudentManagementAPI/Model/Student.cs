using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Model
{
    public class Student
    {

        [Key]
        public long StudentId { get; set; }

        [Required(ErrorMessage = "Student Name is required"), StringLength(50, MinimumLength=3, ErrorMessage = "Username must be of minmum of 3 characters and maximum of 60 characters.")]
        public string StudentName { get; set; }

        //[Required(ErrorMessage = "Email address should not be empty."), EmailAddress(ErrorMessage = "Email is not in correct format.")]
        //public string Email { get; set; }

        //[Required(ErrorMessage="Password cannot be null")]
        //public string Password { get; set; }

        //[Required(ErrorMessage = "Roll no should not be empty.")]
        //public string RollNo { get; set; }
        public int Age { get; set; }

        [AllowedValues("A", "B", "C", "D", "E")]
        public string Grade { get; set; }

        //public Student( long studentId, string studentName, string email, string rollNo)
        //{
        //    StudentId = studentId;
        //    StudentName = studentName;
        //    Email = email;
        //    RollNo = rollNo;
        //}
        
    }
}
