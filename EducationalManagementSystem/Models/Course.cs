using System;
using System.Collections;

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
        public ArrayList RegisteredStudents { get; set; } = new ArrayList();
        public ArrayList TeachingAssistants { get; set; } = new ArrayList();
        public ArrayList Assignments { get; set; } = new ArrayList();
        public int? MaxStudents { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
    }
}
