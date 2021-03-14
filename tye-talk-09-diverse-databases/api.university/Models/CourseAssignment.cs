using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.university.Models
{
    public class CourseAssignment
    {
        public int InstructorID { get; set; }
        public int CourseNumber { get; set; }
        public Instructor Instructor { get; set; }
        public Course Course { get; set; }
    }
}