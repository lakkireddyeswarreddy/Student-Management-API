using StudentManagementAPI.Model;

namespace StudentManagementAPI.ServiceInterface
{
    public interface IStudentService
    {

        public Task<Student?> GetStudentById(long id);

        public Task<ICollection<Student>> GetStudents();

        public Task AddStudent(Student student);

        public Task RemoveStudent(long id);

        public Task UpdateStudent(Student student);
    }
}
