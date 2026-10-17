using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Services;


public sealed class AssignmentService : IAssignmentService
{
    // Private fields for managing dependencies.
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentSolutionRepository _solutionRepository;
    private readonly IAssignmentValidator _validator;

    // Constructor to initialize the AssignmentService with required dependencies.
    public AssignmentService(
        IAssignmentRepository assignmentRepository,
        IAssignmentSolutionRepository solutionRepository,
        IAssignmentValidator validator)
    {
        // Initialize the private fields with the provided dependencies, throwing an exception if any are null.
        _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        _solutionRepository = solutionRepository ?? throw new ArgumentNullException(nameof(solutionRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public bool CreateAssignment(Assignment assignment)
    {
        if (!_validator.IsValid(assignment))
            return false;

        assignment.Title = assignment.Title.Trim();
        assignment.Description = NormalizeOptionalText(assignment.Description);
        assignment.CreatedAt = assignment.CreatedAt == default ? DateTime.UtcNow : assignment.CreatedAt;

        return _assignmentRepository.AddAssignment(assignment);
    }

    // Method to view assignments for a specific course, ordered by due date and creation date.
    public List<Assignment> ViewAssignments(int courseId)
    {
        if (courseId <= 0)
            return [];

        return _assignmentRepository.GetAssignmentsByCourse(courseId)
            .OrderBy(assignment => assignment.DueDate ?? DateTime.MaxValue)
            .ThenBy(assignment => assignment.CreatedAt)
            .ToList();
    }

    // Method to submit a solution for an assignment, ensuring validation and business rules are followed.
    public bool SubmitSolution(AssignmentSolution solution)
    {
        if (!_validator.IsValid(solution))
            return false;

        var assignment = _assignmentRepository.GetAssignmentById(solution.AssignmentId);
        if (assignment is null || HasExpired(assignment) || HasAlreadySubmitted(solution))
            return false;

        solution.Content = solution.Content.Trim();
        solution.SubmittedAt = solution.SubmittedAt == default ? DateTime.UtcNow : solution.SubmittedAt;
        solution.Assignment = assignment;

        // Add the solution to the repository and associate it with the assignment.
        if (!_solutionRepository.AddSolution(solution))
            return false;

        assignment.Solutions.Add(solution);
        return true;
    }

    // Private helper method to check if a student has already submitted a solution for a specific assignment.
    private bool HasAlreadySubmitted(AssignmentSolution solution)
    {
        return _solutionRepository.GetSolutionsByAssignment(solution.AssignmentId)
            .Any(existing => existing.StudentId == solution.StudentId);
    }

    // Private helper method to check if an assignment has expired based on its due date.
    private static bool HasExpired(Assignment assignment)
    {
        return assignment.DueDate.HasValue && assignment.DueDate.Value < DateTime.UtcNow;
    }

    // Private helper method to normalize optional text fields, returning null for empty or whitespace values.
    private static string? NormalizeOptionalText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}

// Interface for validating assignments and assignment solutions.
public interface IAssignmentValidator
{
    bool IsValid(Assignment? assignment);
    bool IsValid(AssignmentSolution? solution);
}
