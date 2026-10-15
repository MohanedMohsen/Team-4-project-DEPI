using EducationalManagementSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Models
{
    public class Student : User
    {
        public override UserRole Role => UserRole.Student;

        public int AcademicYear { get; set; } = 1;

        public ICollection<Course> EnrolledCourses { get; set; }
            = new List<Course>();

        public ICollection<AssignmentSolution> Solutions { get; set; }
            = new List<AssignmentSolution>();
    }
}
