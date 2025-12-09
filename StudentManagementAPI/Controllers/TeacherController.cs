using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.CustomFilters;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Model;
using StudentManagementAPI.ServiceInterface;

namespace StudentManagementAPI.Controllers
{
    [ServiceFilter(typeof(ModelValidationFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IPasswordHasher<Teacher> _passwordHasher;

        private readonly ITeacherService _teacherService;

        public TeacherController(IPasswordHasher<Teacher> passwordHasher, ITeacherService teacherService)
        {
            _passwordHasher = passwordHasher;
            _teacherService = teacherService;
        }

        [HttpPost("[action]")]
        public async  Task<IActionResult> Register([FromBody] TeacherDto teacherDto)
        {
            Teacher teacher = new Teacher { Email = teacherDto.Email };

            teacher.Passwordhash = _passwordHasher.HashPassword(teacher, teacherDto.Password);

            await _teacherService.RegisterTeacher(teacher);

            return Ok(teacher);
        }
    }
}
