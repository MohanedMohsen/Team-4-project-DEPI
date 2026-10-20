using System.Collections;
using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Repositories;

public sealed class InMemoryAssignmentRepository : IAssignmentRepository
{
    private readonly ArrayList _assignments = new ArrayList();
    private int _nextId = 1;

    public InMemoryAssignmentRepository()
    {
        _assignments.Add(new Assignment
        {
            Id = 1,
            CourseId = 1,
            DoctorCreatorId = 106,
            Title = "Assignment 1: Variables & Control Flow",
            Description = "Implement simple calculator and loop conditions.",
            MaxGrade = 20,
            DueDate = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        });

        _assignments.Add(new Assignment
        {
            Id = 2,
            CourseId = 1,
            DoctorCreatorId = 106,
            Title = "Assignment 2: Functions & Arrays",
            Description = "Write functions for sorting and searching arrays.",
            MaxGrade = 30,
            DueDate = DateTime.UtcNow.AddDays(14),
            CreatedAt = DateTime.UtcNow
        });

        _assignments.Add(new Assignment
        {
            Id = 3,
            CourseId = 2,
            DoctorCreatorId = 107,
            Title = "Assignment 1: OOP Classes & Encapsulation",
            Description = "Create a Student class hierarchy with properties.",
            MaxGrade = 50,
            DueDate = DateTime.UtcNow.AddDays(10),
            CreatedAt = DateTime.UtcNow
        });

        _assignments.Add(new Assignment
        {
            Id = 4,
            CourseId = 4,
            DoctorCreatorId = 103,
            Title = "Assignment 1: Logic & Sets Proofs",
            Description = "Solve problem set 1 for propositional logic.",
            MaxGrade = 25,
            DueDate = DateTime.UtcNow.AddDays(5),
            CreatedAt = DateTime.UtcNow
        });

        _nextId = 5;
    }

    public bool AddAssignment(Assignment assignment)
    {
        if (assignment is null)
            return false;

        foreach (Assignment item in _assignments)
        {
            if (assignment.Id > 0 && item.Id == assignment.Id)
                return false;
        }

        if (assignment.Id <= 0)
            assignment.Id = _nextId;

        if (assignment.Id >= _nextId)
            _nextId = assignment.Id + 1;
        _assignments.Add(assignment);
        return true;
    }

    public Assignment? GetAssignmentById(int assignmentId)
    {
        foreach (Assignment assignment in _assignments)
        {
            if (assignment.Id == assignmentId)
                return assignment;
        }

        return null;
    }

    public List<Assignment> GetAssignmentsByCourse(int courseId)
    {
        List<Assignment> assignments = new List<Assignment>();
        foreach (Assignment assignment in _assignments)
        {
            if (assignment.CourseId == courseId)
                assignments.Add(assignment);
        }

        return assignments;
    }

    public List<Assignment> GetAllAssignments()
    {
        List<Assignment> assignments = new List<Assignment>();
        foreach (Assignment assignment in _assignments)
            assignments.Add(assignment);

        return assignments;
    }

    public bool UpdateAssignment(Assignment assignment)
    {
        if (assignment is null || assignment.Id <= 0)
            return false;

        for (int index = 0; index < _assignments.Count; index++)
        {
            Assignment existingAssignment = (Assignment)_assignments[index]!;
            if (existingAssignment.Id == assignment.Id)
            {
                existingAssignment.Title = assignment.Title;
                existingAssignment.Description = assignment.Description;
                existingAssignment.MaxGrade = assignment.MaxGrade;
                existingAssignment.DueDate = assignment.DueDate;
                return true;
            }
        }

        return false;
    }

    public bool DeleteAssignment(int assignmentId)
    {
        if (assignmentId <= 0)
            return false;

        for (int index = 0; index < _assignments.Count; index++)
        {
            Assignment assignment = (Assignment)_assignments[index]!;
            if (assignment.Id == assignmentId)
            {
                _assignments.RemoveAt(index);
                return true;
            }
        }

        return false;
    }
}
