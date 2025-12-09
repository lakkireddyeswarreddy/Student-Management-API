using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentManagementAPI.ApplicationDbContext;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Model;
using StudentManagementAPI.Service;
using StudentManagementAPI.ServiceInterface;
using StudentManagementAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        private readonly IPasswordHasher<Teacher> _passwordHasher;

        private readonly IConfiguration _configuration;

        public AuthController(ITeacherService teacherService, IPasswordHasher<Teacher> passwordHasher, IConfiguration configuration)
        {
            _teacherService = teacherService;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public async  Task<IActionResult> Login([FromBody] TeacherDto loginDto)
        {
            var teacher = await _teacherService.GetTeacherByEmail(loginDto.Email);

            if (teacher == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(teacher, teacher.Passwordhash, loginDto.Password);

            if (verificationResult == PasswordVerificationResult.Success)
            {
                var token = GenerateJwtToken(teacher.Email);

                return Ok(token);
            }
            else
            {
                return Unauthorized("Invalid credentials");

            }


        }

        private string GenerateJwtToken(string email)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, email),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials:credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
