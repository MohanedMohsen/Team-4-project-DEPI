using EducationalManagementSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Models
{
    public class TeachingAssistant : User
    {
        public override UserRole Role => UserRole.TeachingAssistant;

        public ICollection<Course> AssignedCourses { get; set; }
            = new List<Course>();
    }
}
