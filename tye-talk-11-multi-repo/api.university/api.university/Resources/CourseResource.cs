using System.Collections.Generic;

namespace api.university.Resources
{
    public class CourseResource
    {
        public ICollection<CourseAssignmentResource> CourseAssignments { get; set; }

        public int CourseNumber { get; set; }

        public int Credits { get; set; }

        public DepartmentResource Department { get; set; }

        public ICollection<EnrollmentResource> Enrollments { get; set; }

        public string Title { get; set; }
    }
}