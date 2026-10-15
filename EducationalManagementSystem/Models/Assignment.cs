using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CourseId { get; set; }

        public Course? Course { get; set; }

        public decimal MaxGrade { get; set; }

        public int DoctorCreatorId { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AssignmentSolution> Solutions { get; set; }
            = new List<AssignmentSolution>();
    }
}
