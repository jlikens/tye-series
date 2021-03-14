namespace api.university.Resources
{
    public class CourseAssignmentResource
    {
        public InstructorResource Instructor { get; set; }
        public CourseResource Course { get; set; }
    }
}