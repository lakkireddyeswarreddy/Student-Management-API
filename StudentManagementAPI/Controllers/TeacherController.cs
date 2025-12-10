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

        private readonly string[] _allowedFileExtensions ;

        public TeacherController(IPasswordHasher<Teacher> passwordHasher, ITeacherService teacherService)
        {
            _passwordHasher = passwordHasher;
            _teacherService = teacherService;
            _allowedFileExtensions = [".pdf", ".txt", ".py"];
        }

        [HttpPost("[action]")]
        public async  Task<IActionResult> Register([FromBody] TeacherDto teacherDto)
        {
            Teacher teacher = new Teacher { Email = teacherDto.Email };

            teacher.Passwordhash = _passwordHasher.HashPassword(teacher, teacherDto.Password);

            await _teacherService.RegisterTeacher(teacher);

            return Ok(teacher);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadFile(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return BadRequest("No file was uploaded");
            }

            var fileExtension = Path.GetExtension(formFile.FileName).ToLowerInvariant();

            if(fileExtension == null || !_allowedFileExtensions.Contains(fileExtension))
            {
                return BadRequest($"Invalid file type, only {string.Join(" and ", _allowedFileExtensions)} are allowed");
            }

            var maxFileSize = 0.01 * 1024 * 1024;

            if(formFile.Length > maxFileSize)
            {
                return BadRequest($"File size exceeds the limit, please upload files below {maxFileSize/(1024 * 1024)} MB");
            }

            var filesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(filesFolder))
            {
                Directory.CreateDirectory(filesFolder);
            }

            var filePath = Path.Combine(filesFolder, formFile.FileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            formFile.CopyToAsync(stream);

            return Ok(new {message = $"File : {formFile.FileName} uploaded successfully.", size = formFile.Length});
        }
    }
}
