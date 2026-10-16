using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Interfaces
{
    public interface ICourseService
    {
        bool CreateCourse(Course course);

        Course? GetCourseById(int courseId);

        List<Course> GetAllCourses();

        bool UpdateCourse(Course course);

        bool DeleteCourse(int courseId);

        bool RegisterStudent(int studentId, int courseId);

        bool AssignTeachingAssistant(int courseId, int assistantId);

        List<Course> GetCoursesByDoctor(int doctorId);
    }
}
