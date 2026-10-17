using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Repositories;


public sealed class InMemoryAssignmentSolutionRepository : IAssignmentSolutionRepository
{
    private readonly List<AssignmentSolution> _solutions = [];
    private readonly object _syncRoot = new();
    private int _nextId = 1;

    // Adds a new assignment solution to the repository.
    public bool AddSolution(AssignmentSolution solution)
    {
        if (solution is null)
            return false;

        // Ensure that the solution ID is unique and not already present in the repository.
        lock (_syncRoot)
        {
            if (solution.Id > 0 && _solutions.Any(item => item.Id == solution.Id))
                return false;

            solution.Id = solution.Id > 0 ? solution.Id : _nextId++;
            _nextId = Math.Max(_nextId, solution.Id + 1);

            _solutions.Add(solution);
            return true;
        }
    }

    // Retrieves an assignment solution by its ID.
    public AssignmentSolution? GetSolutionById(int solutionId)
    {
        lock (_syncRoot)
        {
            return _solutions.SingleOrDefault(item => item.Id == solutionId);
        }
    }

    // Retrieves all assignment solutions associated with a specific assignment ID.
    public List<AssignmentSolution> GetSolutionsByAssignment(int assignmentId)
    {
        lock (_syncRoot)
        {
            return _solutions
                .Where(item => item.AssignmentId == assignmentId)
                .OrderBy(item => item.SubmittedAt)
                .ToList();
        }
    }

    // Retrieves all assignment solutions associated with a specific student ID.
    public List<AssignmentSolution> GetSolutionsByStudent(int studentId)
    {
        lock (_syncRoot)
        {
            return _solutions
                .Where(item => item.StudentId == studentId)
                .OrderByDescending(item => item.SubmittedAt)
                .ToList();
        }
    }

    // Updates an existing assignment solution in the repository.
    public bool UpdateSolution(AssignmentSolution solution)
    {
        if (solution is null || solution.Id <= 0)
            return false;

        lock (_syncRoot)
        {
            var index = _solutions.FindIndex(item => item.Id == solution.Id);
            if (index < 0)
                return false;

            _solutions[index] = solution;
            return true;
        }
    }
}
