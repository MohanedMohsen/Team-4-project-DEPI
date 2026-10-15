using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Models
{
    public class AssignmentSolution
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public int StudentId { get; set; }

        public Student? Student { get; set; }

        public int AssignmentId { get; set; }

        public Assignment? Assignment { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public decimal? Grade { get; set; }

        public string? Comment { get; set; }

        public bool IsGraded => Grade.HasValue;
    }
}
