using System;
using System.Collections.Generic;

namespace api.university.Resources
{
    public class DepartmentResource
    {
        public int DepartmentID { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }

        public InstructorResource Administrator { get; set; }
        public ICollection<CourseResource> Courses { get; set; }
    }
}