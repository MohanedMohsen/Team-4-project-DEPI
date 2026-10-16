using EducationalManagementSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Models
{
    public class Doctor : User
    {
        public override UserRole Role => UserRole.Doctor;

        public string? AcademicTitle { get; set; }

        public ICollection<Course> CreatedCourses { get; set; }
            = new List<Course>();
    }
}
