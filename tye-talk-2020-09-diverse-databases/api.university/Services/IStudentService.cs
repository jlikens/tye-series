using api.university.Resources;
using System.Threading.Tasks;

namespace api.university.Services
{
    public interface IStudentService
    {
        StudentResource GetStudent(int studentId);
        Task<StudentResource> GetStudentAsync(int studentId);
    }
}
