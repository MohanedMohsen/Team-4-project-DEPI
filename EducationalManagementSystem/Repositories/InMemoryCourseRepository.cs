using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace EducationalManagementSystem.Repositories
{
    public sealed class InMemoryCourseRepository : ICourseRepository
    {
        private readonly List<Course> _courses = new List<Course>();
        private int _nextId = 1;

        public bool AddCourse(Course course)
        {
            if (course is null) return false;

            if (course.Id > 0)
            {
                foreach (var item in _courses)
                {
                    if (item.Id == course.Id) return false;
                }
            }

            course.Id = course.Id > 0 ? course.Id : _nextId++;

            int maxId = 0;
            foreach (var item in _courses)
            {
                if (item.Id > maxId) maxId = item.Id;
            }
            _nextId = Math.Max(_nextId, maxId + 2);

            _courses.Add(course);
            return true;
        }

        public Course? GetCourseById(int courseId)
        {
            foreach (var item in _courses)
            {
                if (item.Id == courseId)
                {
                    return item;
                }
            }
            return null;
        }

        public List<Course> GetAllCourses()
        {
            List<Course> list = new List<Course>();
            foreach (var item in _courses)
            {
                list.Add(item);
            }
            return list;
        }

        public bool UpdateCourse(Course course)
        {
            if (course is null || course.Id <= 0) return false;

            int index = -1;
            for (int i = 0; i < _courses.Count; i++)
            {
                if (_courses[i].Id == course.Id)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0) return false;

            _courses[index] = course;
            return true;
        }

        public bool DeleteCourse(int courseId)
        {
            var course = GetCourseById(courseId);
            if (course is null) return false;

            return _courses.Remove(course);
        }

        public bool RegisterStudent(int studentId, int courseId)
        {
            var course = GetCourseById(courseId);
            if (course is null || studentId <= 0) return false;

           foreach (var student in course.RegisteredStudents)
            {
                if (student.Id == studentId) return false;
            }

            if (course.MaxStudents.HasValue &&
                course.RegisteredStudents.Count >= course.MaxStudents.Value)
                return false;

            course.RegisteredStudents.Add(new Student { Id = studentId });
            return true;
        }

        public bool AssignTeachingAssistant(int courseId, int assistantId)
        {
            var course = GetCourseById(courseId);
            if (course is null || assistantId <= 0) return false;

            foreach (var ta in course.TeachingAssistants)
            {
                if (ta.Id == assistantId) return false;
            }

            course.TeachingAssistants.Add(new TeachingAssistant { Id = assistantId });
            return true;
        }

        public List<Course> GetCoursesByDoctor(int doctorId)
        {
            List<Course> doctorCourses = new List<Course>();
            foreach (var course in _courses)
            {
                if (course.CreatedByDoctorId == doctorId)
                {
                    doctorCourses.Add(course);
                }
            }
            return doctorCourses;
        }

        public bool IsStudentRegistered(int studentId, int courseId)
        {
            var course = GetCourseById(courseId);
            if (course is null) return false;

            foreach (var student in course.RegisteredStudents)
            {
                if (student.Id == studentId) return true;
            }

            return false;
        }
    }
}