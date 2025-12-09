using StudentManagementAPI.Model;
using StudentManagementAPI.RepositoryInterface;
using StudentManagementAPI.ServiceInterface;

namespace StudentManagementAPI.Service
{
    public class TeacherService : ITeacherService
    {
        private readonly IRepository<Teacher> _teacherRepository;

        public TeacherService(IRepository<Teacher> teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<Teacher> GetTeacherByEmail(string email)
        {
            return await _teacherRepository.GetById(email);
        }

        public async Task RegisterTeacher(Teacher teacher)
        {
           await _teacherRepository.Register(teacher);
        }
    }
}
