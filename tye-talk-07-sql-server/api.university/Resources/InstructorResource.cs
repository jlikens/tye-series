using System;
using System.Collections.Generic;

namespace api.university.Resources
{
    public class InstructorResource : PersonResource
    {
        public DateTime HireDate { get; set; }
        public ICollection<CourseAssignmentResource> CourseAssignments { get; set; }
        public OfficeAssignmentResource OfficeAssignment { get; set; }
    }
}