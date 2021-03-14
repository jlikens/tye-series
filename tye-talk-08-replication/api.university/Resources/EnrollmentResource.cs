namespace api.university.Resources
{
    public class EnrollmentResource
    {
        public int EnrollmentID { get; set; }

        public Grade? Grade { get; set; }

        public CourseResource Course { get; set; }
        public StudentResource Student { get; set; }
    }
}