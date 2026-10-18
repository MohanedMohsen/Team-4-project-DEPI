using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Services;

public sealed class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository
            ?? throw new ArgumentNullException(nameof(courseRepository));
    }

    public bool CreateCourse(Course course)
    {
        if (!IsValidCourse(course)) return false;

        NormalizeCourse(course);

        return _courseRepository.AddCourse(course);
    }

    public Course? GetCourseById(int courseId)
    {
        return courseId > 0
            ? _courseRepository.GetCourseById(courseId)
            : null;
    }

    public List<Course> GetAllCourses()
    {
        return _courseRepository
            .GetAllCourses()
            .OrderBy(course => course.Code)
            .ToList();
    }

    public bool UpdateCourse(Course course)
    {
        if (course is null || course.Id <= 0 || !IsValidCourse(course))
            return false;

        NormalizeCourse(course);

        return _courseRepository.UpdateCourse(course);
    }

    public bool DeleteCourse(int courseId)
    {
        return courseId > 0 &&
               _courseRepository.DeleteCourse(courseId);
    }

    public bool RegisterStudent(int studentId, int courseId)
    {
        if (studentId <= 0 || courseId <= 0)
            return false;

        var course = _courseRepository.GetCourseById(courseId);

        if (course is null ||
            _courseRepository.IsStudentRegistered(studentId, courseId))
            return false;

        if (course.RegistrationStartDate.HasValue &&
            DateTime.UtcNow < course.RegistrationStartDate.Value)
            return false;

        if (course.RegistrationEndDate.HasValue &&
            DateTime.UtcNow > course.RegistrationEndDate.Value)
            return false;

        return _courseRepository.RegisterStudent(studentId, courseId);
    }

    public bool AssignTeachingAssistant(int courseId, int assistantId)
    {
        return courseId > 0 &&
               assistantId > 0 &&
               _courseRepository.AssignTeachingAssistant(courseId, assistantId);
    }

    public List<Course> GetCoursesByDoctor(int doctorId)
    {
        return doctorId > 0
            ? _courseRepository.GetCoursesByDoctor(doctorId)
            : [];
    }

    private static bool IsValidCourse(Course? course)
    {
        return course is not null &&
               !string.IsNullOrWhiteSpace(course.Name) &&
               !string.IsNullOrWhiteSpace(course.Code) &&
               course.CreatedByDoctorId > 0 &&
               (!course.MaxStudents.HasValue || course.MaxStudents > 0);
    }

    private static void NormalizeCourse(Course course)
    {
        course.Name = course.Name.Trim();
        course.Code = course.Code.Trim().ToUpperInvariant();

        course.Description = string.IsNullOrWhiteSpace(course.Description)
            ? null
            : course.Description.Trim();
    }
}
