using StudentManagementAPI.Model;
using StudentManagementAPI.RepositoryInterface;
using StudentManagementAPI.ServiceInterface;
using System.Collections;
using System.Threading.Tasks;

namespace StudentManagementAPI.Services
{
    public class StudentService : IStudentService
    { 
        //public List<Student>  Students { get; set; }

        private readonly IRepository<Student> _studentRepository;

        public StudentService(IRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student?> GetStudentById(long id)
        {
            Student student = await _studentRepository.GetById(id);

            return student;
        }

        public async  Task<ICollection<Student>> GetStudents()
        {
            var students = await _studentRepository.GetAll();

            return students;
        }

        public async Task AddStudent(Student student)
        {
             await _studentRepository.Add(student);
        }

        public async Task RemoveStudent(long id)
        {
            await _studentRepository.Remove(id);
        }

        public async Task UpdateStudent(Student student)
        {
            await _studentRepository.Update(student);
        }

    }
}
