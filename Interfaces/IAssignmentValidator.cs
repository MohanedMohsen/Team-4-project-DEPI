
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Interfaces;

public interface IAssignmentValidator
{
    bool IsValid(Assignment? assignment);

    bool IsValid(AssignmentSolution? solution);
}

