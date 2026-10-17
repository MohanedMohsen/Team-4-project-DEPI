using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Repositories;

// In-memory implementation of the IAssignmentRepository interface for managing assignments.
public sealed class InMemoryAssignmentRepository : IAssignmentRepository
{
    private readonly List<Assignment> _assignments = [];
    private readonly object _syncRoot = new();
    private int _nextId = 1;

    // Adds a new assignment to the repository.
    public bool AddAssignment(Assignment assignment)
    {
        if (assignment is null)
            return false;

        lock (_syncRoot)
        {
            // Ensure that the assignment ID is unique and not already present in the repository.
            if (assignment.Id > 0 && _assignments.Any(item => item.Id == assignment.Id))
                return false;

            assignment.Id = assignment.Id > 0 ? assignment.Id : _nextId++;
            _nextId = Math.Max(_nextId, assignment.Id + 1);

            _assignments.Add(assignment);
            return true;
        }
    }

    // Retrieves an assignment by its ID.
    public Assignment? GetAssignmentById(int assignmentId)
    {
        lock (_syncRoot)
        {
            return _assignments.SingleOrDefault(item => item.Id == assignmentId);
        }
    }

    // Retrieves all assignments associated with a specific course ID.
    public List<Assignment> GetAssignmentsByCourse(int courseId)
    {
        lock (_syncRoot)
        {
            return _assignments.Where(item => item.CourseId == courseId).ToList();
        }
    }

    // Retrieves all assignments in the repository.
    public List<Assignment> GetAllAssignments()
    {
        lock (_syncRoot)
        {
            return _assignments.ToList();
        }
    }

    // Updates an existing assignment in the repository.
    public bool UpdateAssignment(Assignment assignment)
    {
        if (assignment is null || assignment.Id <= 0)
            return false;

        lock (_syncRoot)
        {
            var index = _assignments.FindIndex(item => item.Id == assignment.Id);
            if (index < 0)
                return false;

            _assignments[index] = assignment;
            return true;
        }
    }

    // Deletes an assignment from the repository by its ID.
    public bool DeleteAssignment(int assignmentId)
    {
        if (assignmentId <= 0)
            return false;

        lock (_syncRoot)
        {
            var assignment = _assignments.SingleOrDefault(item => item.Id == assignmentId);
            return assignment is not null && _assignments.Remove(assignment);
        }
    }
}
