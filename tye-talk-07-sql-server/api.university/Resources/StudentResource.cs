using System;
using System.Collections.Generic;

namespace api.university.Resources
{
    public class StudentResource : PersonResource
    {
        public DateTime EnrollmentDate { get; set; }

        public ICollection<EnrollmentResource> Enrollments { get; set; }
    }
}