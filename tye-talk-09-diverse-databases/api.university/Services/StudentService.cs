using api.university.Data;
using api.university.Resources;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace api.university.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolContext _schoolContext;
        private readonly IMapper _mapper;

        public StudentService(
            SchoolContext schoolContext
            , IMapper mapper)
        {
            _schoolContext = schoolContext;
            _mapper = mapper;
        }

        public StudentResource GetStudent(int studentId)
        {
            return GetStudentAsync(studentId).Result;
        }

        public async Task<StudentResource> GetStudentAsync(int studentId)
        {
            var model = await _schoolContext.Students.FirstOrDefaultAsync(s => s.ID == studentId);
            var resource = _mapper.Map<StudentResource>(model);
            return resource;
        }
    }
}
