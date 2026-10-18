using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Repositories;

public sealed class InMemoryCourseRepository : ICourseRepository
{
    private readonly List<Course> _courses = [];
    private int _nextId = 1;

    public bool AddCourse(Course course)
    {
        if (course is null) return false;

        if (course.Id > 0 && _courses.Any(item => item.Id == course.Id))
            return false;

        course.Id = course.Id > 0 ? course.Id : _nextId++;
        _nextId = Math.Max(_nextId, course.Id + 1);

        _courses.Add(course);
        return true;
    }

    public Course? GetCourseById(int courseId)
    {
        return _courses.SingleOrDefault(item => item.Id == courseId);
    }

    public List<Course> GetAllCourses()
    {
        return _courses.ToList();
    }

    public bool UpdateCourse(Course course)
    {
        if (course is null || course.Id <= 0) return false;

        int index = _courses.FindIndex(item => item.Id == course.Id);

        if (index < 0) return false;

        _courses[index] = course;
        return true;
    }

    public bool DeleteCourse(int courseId)
    {
        var course = GetCourseById(courseId);

        return course is not null && _courses.Remove(course);
    }

    public bool RegisterStudent(int studentId, int courseId)
    {
        var course = GetCourseById(courseId);

        if (course is null || studentId <= 0)
            return false;

        if (course.RegisteredStudents.Any(student => student.Id == studentId))
            return false;

        if (course.MaxStudents.HasValue &&
            course.RegisteredStudents.Count >= course.MaxStudents.Value)
            return false;

        course.RegisteredStudents.Add(new Student { Id = studentId });

        return true;
    }

    public bool AssignTeachingAssistant(int courseId, int assistantId)
    {
        var course = GetCourseById(courseId);

        if (course is null || assistantId <= 0)
            return false;

        if (course.TeachingAssistants.Any(ta => ta.Id == assistantId))
            return false;

        course.TeachingAssistants.Add(new TeachingAssistant { Id = assistantId });

        return true;
    }

    public List<Course> GetCoursesByDoctor(int doctorId)
    {
        return _courses
            .Where(course => course.CreatedByDoctorId == doctorId)
            .ToList();
    }

    public bool IsStudentRegistered(int studentId, int courseId)
    {
        var course = GetCourseById(courseId);

        return course is not null &&
               course.RegisteredStudents.Any(student => student.Id == studentId);
    }
}