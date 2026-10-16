using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Interfaces
{
    public interface IAssignmentService
    {
        bool CreateAssignment(Assignment assignment);

        List<Assignment> ViewAssignments(int courseId);

        bool SubmitSolution(AssignmentSolution solution);
    }
}
