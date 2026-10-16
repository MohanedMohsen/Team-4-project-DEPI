using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Interfaces
{
    public interface IStudentService
    {
        List<Course> ViewMyCourses(int studentId);

        bool RegisterCourse(int studentId, int courseId);

        bool SubmitSolution(
            int studentId,
            int assignmentId,
            string content
        );

        List<AssignmentSolution> ViewGradesReport(int studentId);
    }
}
