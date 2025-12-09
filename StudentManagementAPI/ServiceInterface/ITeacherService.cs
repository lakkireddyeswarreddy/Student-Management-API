using StudentManagementAPI.Model;

namespace StudentManagementAPI.ServiceInterface
{
    public interface ITeacherService
    {
        Task RegisterTeacher (Teacher teacher);

        Task<Teacher> GetTeacherByEmail(string email);
    }
}
