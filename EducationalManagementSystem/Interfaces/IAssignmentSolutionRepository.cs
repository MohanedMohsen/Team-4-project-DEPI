using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Interfaces
{
    public interface IAssignmentSolutionRepository
    {
        bool AddSolution(AssignmentSolution solution);

        AssignmentSolution? GetSolutionById(int solutionId);

        List<AssignmentSolution> GetSolutionsByAssignment(
            int assignmentId);

        List<AssignmentSolution> GetSolutionsByStudent(
            int studentId);

        bool UpdateSolution(AssignmentSolution solution);
    }
}
