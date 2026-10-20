using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace EducationalManagementSystem.Services
{
    public class GradingService : IGradingService
    {
        private readonly IAssignmentSolutionRepository _solutionRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public GradingService(
            IAssignmentSolutionRepository solutionRepository,
            IAssignmentRepository assignmentRepository)
        {
            _solutionRepository = solutionRepository;
            _assignmentRepository = assignmentRepository;
        }

        public AssignmentSolution? ViewSolution(int solutionId)
        {
            if (solutionId <= 0)
            {
                return null;
            }

            AssignmentSolution? solution = _solutionRepository.GetSolutionById(solutionId);
            return solution;
        }

        public bool SetGrade(int solutionId, decimal grade)
        {
            if (solutionId <= 0 || grade < 0)
            {
                return false;
            }

            AssignmentSolution? solution = _solutionRepository.GetSolutionById(solutionId);
            if (solution == null)
            {
                return false;
            }

            Assignment? assignment = _assignmentRepository.GetAssignmentById(solution.AssignmentId);
            if (assignment == null)
            {
                return false;
            }

            if (grade > assignment.MaxGrade)
            {
                return false;
            }

            solution.Grade = grade;
            return _solutionRepository.UpdateSolution(solution);
        }

        public bool SetComment(int solutionId, string comment)
        {
            if (solutionId <= 0 || string.IsNullOrWhiteSpace(comment))
            {
                return false;
            }

            AssignmentSolution? solution = _solutionRepository.GetSolutionById(solutionId);
            if (solution == null)
            {
                return false;
            }

            solution.Comment = comment.Trim();
            return _solutionRepository.UpdateSolution(solution);
        }

        public List<AssignmentSolution> ViewGradesReport(int studentId)
        {
            if (studentId <= 0)
            {
                return new List<AssignmentSolution>();
            }

            List<AssignmentSolution> solutions = _solutionRepository.GetSolutionsByStudent(studentId);
            if (solutions == null)
            {
                return new List<AssignmentSolution>();
            }

            return solutions;
        }
    }
}
