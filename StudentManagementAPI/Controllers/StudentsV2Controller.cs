using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.CustomFilters;
using StudentManagementAPI.Model;
using StudentManagementAPI.ServiceInterface;

namespace StudentManagementAPI.Controllers
{
    [Authorize(Policy = "IITMTeacherOnly")]
    [ServiceFilter(typeof(ModelValidationFilter))]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class StudentsV2Controller : ControllerBase
    {
        private readonly IStudentService studentService;

        private readonly ILogger<StudentsController> _logger;

        public StudentsV2Controller(IStudentService studentService, ILogger<StudentsController> logger)
        {
            this.studentService = studentService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(long id)
        {
            _logger.LogInformation("Retrieving student data with ID : {id}", id);

            var student = await studentService.GetStudentById(id);

            if (student == null)
            {
                _logger.LogInformation("No student data found with matching ID : {id}", id);

                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved student data with ID : {id}", id);

            return Ok(student);
        }
    }
}
