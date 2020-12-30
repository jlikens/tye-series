using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.university.Models
{
    public class Course
    {
        public ICollection<CourseAssignment> CourseAssignments { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        [Key]
        public int CourseNumber { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public Department Department { get; set; }

        public int DepartmentID { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
    }
}