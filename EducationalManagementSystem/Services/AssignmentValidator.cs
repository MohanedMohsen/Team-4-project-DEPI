using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Services;


public sealed class AssignmentValidator : IAssignmentValidator
{
    public bool IsValid(Assignment? assignment)
    {
        if (assignment is null)
            return false;

        bool hasValidTitle = !string.IsNullOrWhiteSpace(assignment.Title)
                             && assignment.Title.Trim().Length <= 200;

        bool hasValidIds = assignment.CourseId > 0
                           && assignment.DoctorCreatorId > 0;

        bool hasValidMaxGrade = assignment.MaxGrade > 0;

        bool hasValidDueDate = assignment.DueDate is not { } dueDate
                               || dueDate > DateTime.UtcNow;

        return hasValidTitle && hasValidIds && hasValidMaxGrade && hasValidDueDate;
    }
    
    public bool IsValid(AssignmentSolution? solution)
    {
        if (solution is null)
            return false;

        bool hasValidContent = !string.IsNullOrWhiteSpace(solution.Content)
                               && solution.Content.Trim().Length <= 10_000;

        bool hasValidIds = solution.StudentId > 0
                           && solution.AssignmentId > 0;

        bool isUncheckedStatus = !solution.Grade.HasValue
                                 && string.IsNullOrWhiteSpace(solution.Comment);

        return hasValidContent && hasValidIds && isUncheckedStatus;
    }
}
