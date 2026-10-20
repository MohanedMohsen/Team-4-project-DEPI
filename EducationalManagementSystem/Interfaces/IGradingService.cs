using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Interfaces
{
    public interface IGradingService
    {
        AssignmentSolution? ViewSolution(int solutionId);

        bool SetGrade(int solutionId, decimal grade);

        bool SetComment(int solutionId, string comment);

        List<AssignmentSolution> ViewGradesReport(int studentId);

        List<AssignmentSolution> GetSolutionsByAssignment(int assignmentId);
    }
}
