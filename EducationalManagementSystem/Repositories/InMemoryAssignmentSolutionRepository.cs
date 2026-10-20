using System.Collections;
using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Repositories;

public sealed class InMemoryAssignmentSolutionRepository : IAssignmentSolutionRepository
{
    private readonly ArrayList _solutions = new ArrayList();
    private int _nextId = 1;

    public bool AddSolution(AssignmentSolution solution)
    {
        if (solution is null)
            return false;

        foreach (AssignmentSolution item in _solutions)
        {
            if (solution.Id > 0 && item.Id == solution.Id)
                return false;
        }

        if (solution.Id <= 0)
            solution.Id = _nextId;

        if (solution.Id >= _nextId)
            _nextId = solution.Id + 1;
        _solutions.Add(solution);
        return true;
    }

    public AssignmentSolution? GetSolutionById(int solutionId)
    {
        foreach (AssignmentSolution solution in _solutions)
        {
            if (solution.Id == solutionId)
                return solution;
        }

        return null;
    }

    public List<AssignmentSolution> GetSolutionsByAssignment(int assignmentId)
    {
        List<AssignmentSolution> solutions = new List<AssignmentSolution>();
        foreach (AssignmentSolution solution in _solutions)
        {
            if (solution.AssignmentId == assignmentId)
                solutions.Add(solution);
        }

        return solutions;
    }

    public List<AssignmentSolution> GetSolutionsByStudent(int studentId)
    {
        List<AssignmentSolution> solutions = new List<AssignmentSolution>();
        foreach (AssignmentSolution solution in _solutions)
        {
            if (solution.StudentId == studentId)
                solutions.Add(solution);
        }

        return solutions;
    }

    public bool UpdateSolution(AssignmentSolution solution)
    {
        if (solution is null || solution.Id <= 0)
            return false;

        for (int index = 0; index < _solutions.Count; index++)
        {
            AssignmentSolution existingSolution = (AssignmentSolution)_solutions[index]!;
            if (existingSolution.Id == solution.Id)
            {
                _solutions[index] = solution;
                return true;
            }
        }

        return false;
    }

    public bool DeleteSolution(int solutionId)
    {
        for (int index = 0; index < _solutions.Count; index++)
        {
            AssignmentSolution solution = (AssignmentSolution)_solutions[index]!;
            if (solution.Id == solutionId)
            {
                _solutions.RemoveAt(index);
                return true;
            }
        }

        return false;
    }
}
