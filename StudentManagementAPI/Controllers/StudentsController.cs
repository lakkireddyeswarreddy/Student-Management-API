using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Model;
using StudentManagementAPI.ServiceInterface;

namespace StudentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentsController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet]
        public async  Task<ActionResult<ICollection<Student>>> GetStudents()
        {
            var students =  await studentService.GetStudents();

            return Ok(students);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(long id)
        {
            var student = await studentService.GetStudentById(id);

            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]

        public async  Task<IActionResult> AddStudent(Student student)
        {
            await studentService.AddStudent(student);

            return Created();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> RemoveStudent(long id)
        {
            await studentService.RemoveStudent(id);

            return Content("Deleted record successfully");
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateStudentPut(long id, [FromBody] StudentDTO studentDTO)
        {
            var result = await GetStudent(id);

            Student? student = null;

            if(result.Result is OkObjectResult okResult)
            {
               student = okResult.Value as Student;
            }

            if (student == null)
            {
                return NotFound("No student found with the id.");
            }

            student.StudentName = studentDTO.StudentName;
            student.Age = studentDTO.Age;
            student.Grade = studentDTO.Grade;

            await studentService.UpdateStudent(student);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStudentPatch(long id, [FromBody] JsonPatchDocument<StudentPatchDTO> patchDoc)
        {
            if (patchDoc == null) { return BadRequest(); }

            var result = await GetStudent(id);

            Student? student = null;

            if (result.Result is OkObjectResult okResult)
            {
                student = okResult.Value as Student;
            }

            if (student == null)
            {
                return NotFound("No student found with the id.");
            }

            var studentPatchDto = new StudentPatchDTO { Age = student.Age, Grade = student.Grade };

            patchDoc.ApplyTo(studentPatchDto, error => { ModelState.AddModelError(error.AffectedObject.ToString(), error.ErrorMessage);});

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            student.Age = studentPatchDto.Age;

            student.Grade = studentPatchDto.Grade;

            await studentService.UpdateStudent(student);

            return Ok();

        }
    }
}
