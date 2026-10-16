using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Interfaces
{
    public interface IAssignmentRepository
    {
        bool AddAssignment(Assignment assignment);

        Assignment? GetAssignmentById(int assignmentId);

        List<Assignment> GetAssignmentsByCourse(int courseId);

        List<Assignment> GetAllAssignments();

        bool UpdateAssignment(Assignment assignment);

        bool DeleteAssignment(int assignmentId);
    }
}
