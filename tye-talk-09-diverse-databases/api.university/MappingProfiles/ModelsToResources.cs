using AutoMapper;

namespace api.university.MappingProfiles
{
    public class ModelsToResources : Profile
    {
        public ModelsToResources()
        {
            CreateMap<Models.Course, Resources.CourseResource>().ReverseMap();
            CreateMap<Models.CourseAssignment, Resources.CourseAssignmentResource>().ReverseMap();
            CreateMap<Models.Department, Resources.DepartmentResource>().ReverseMap();
            CreateMap<Models.Enrollment, Resources.EnrollmentResource>().ReverseMap();
            CreateMap<Models.Instructor, Resources.InstructorResource>().ReverseMap();
            CreateMap<Models.OfficeAssignment, Resources.OfficeAssignmentResource>().ReverseMap();
            CreateMap<Models.Person, Resources.PersonResource>().ReverseMap();
            CreateMap<Models.Student, Resources.StudentResource>().ReverseMap();
        }
    }
}