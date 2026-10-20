using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Services;

public sealed class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentSolutionRepository _solutionRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAssignmentValidator _validator;

    public AssignmentService(IAssignmentRepository assignmentRepository, IAssignmentSolutionRepository solutionRepository, ICourseRepository courseRepository, IUserRepository userRepository, IAssignmentValidator validator)
    {
        _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        _solutionRepository = solutionRepository ?? throw new ArgumentNullException(nameof(solutionRepository));
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public bool CreateAssignment(Assignment? assignment)
    {
        if (!_validator.IsValid(assignment)) return false;

        assignment!.Title = assignment.Title.Trim();
        assignment.Description = NormalizeOptionalText(assignment.Description);
        if (assignment.CreatedAt == default) assignment.CreatedAt = DateTime.UtcNow;

        Course? course = _courseRepository.GetCourseById(assignment.CourseId);
        if (course is null || course.CreatedByDoctorId != assignment.DoctorCreatorId)
            return false;

        if (!_assignmentRepository.AddAssignment(assignment))
            return false;

        assignment.Course = course;
        course.Assignments.Add(assignment);
        return true;
    }

    public List<Assignment> ViewAssignments(int courseId)
    {
        if (courseId <= 0) return new List<Assignment>();
        return _assignmentRepository.GetAssignmentsByCourse(courseId);
    }

    public bool SubmitSolution(AssignmentSolution? solution)
    {
        if (!_validator.IsValid(solution)) return false;

        AssignmentSolution submittedSolution = solution!;
        Assignment? assignment = _assignmentRepository.GetAssignmentById(submittedSolution.AssignmentId);
        if (assignment is null || HasExpired(assignment) || HasAlreadySubmitted(submittedSolution)) return false;

        if (!_courseRepository.IsStudentRegistered(submittedSolution.StudentId, assignment.CourseId))
            return false;

        User? user = _userRepository.GetUserByID(submittedSolution.StudentId);
        if (user is not Student student)
            return false;

        submittedSolution.Content = submittedSolution.Content.Trim();
        if (submittedSolution.SubmittedAt == default) submittedSolution.SubmittedAt = DateTime.UtcNow;
        submittedSolution.Assignment = assignment;
        submittedSolution.Student = student;

        if (!_solutionRepository.AddSolution(submittedSolution)) return false;

        assignment.Solutions.Add(submittedSolution);
        student.Solutions.Add(submittedSolution);
        return true;
    }

    private bool HasAlreadySubmitted(AssignmentSolution solution)
    {
        List<AssignmentSolution> solutions = _solutionRepository.GetSolutionsByAssignment(solution.AssignmentId);
        foreach (AssignmentSolution existingSolution in solutions)
        {
            if (existingSolution.StudentId == solution.StudentId) return true;
        }
        return false;
    }

    private static bool HasExpired(Assignment assignment)
    {
        return assignment.DueDate.HasValue && assignment.DueDate.Value < DateTime.UtcNow;
    }

    private static string? NormalizeOptionalText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        return value.Trim();
    }
}
