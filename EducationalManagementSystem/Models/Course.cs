using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CreatedByDoctorId { get; set; }

        public Doctor? CreatedByDoctor { get; set; }

        public ICollection<Student> RegisteredStudents { get; set; }
            = new List<Student>();

        public ICollection<TeachingAssistant> TeachingAssistants { get; set; }
            = new List<TeachingAssistant>();

        public ICollection<Assignment> Assignments { get; set; }
            = new List<Assignment>();

        public int? MaxStudents { get; set; }

        public DateTime? RegistrationStartDate { get; set; }

        public DateTime? RegistrationEndDate { get; set; }
    }
}
