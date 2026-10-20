using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Services;

public sealed class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IAssignmentSolutionRepository _solutionRepository;
    private readonly IUserRepository _userRepository;

    public CourseService(ICourseRepository courseRepository, IAssignmentRepository assignmentRepository, IAssignmentSolutionRepository solutionRepository, IUserRepository userRepository)
    {
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
        _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        _solutionRepository = solutionRepository ?? throw new ArgumentNullException(nameof(solutionRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public bool CreateCourse(Course course)
    {
        if (!IsValidCourse(course))
            return false;

        NormalizeCourse(course);
        return _courseRepository.AddCourse(course);
    }

    public Course? GetCourseById(int courseId)
    {
        if (courseId <= 0)
            return null;

        return _courseRepository.GetCourseById(courseId);
    }

    public List<Course> GetAllCourses()
    {
        return _courseRepository.GetAllCourses();
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
        if (courseId <= 0)
            return false;

        Course? course = _courseRepository.GetCourseById(courseId);
        if (course is null)
            return false;

        foreach (Student student in course.RegisteredStudents)
            student.EnrolledCourses.Remove(course);

        foreach (TeachingAssistant assistant in course.TeachingAssistants)
            assistant.AssignedCourses.Remove(course);

        List<Assignment> assignments = _assignmentRepository.GetAssignmentsByCourse(courseId);
        foreach (Assignment assignment in assignments)
        {
            List<AssignmentSolution> solutions = _solutionRepository.GetSolutionsByAssignment(assignment.Id);
            foreach (AssignmentSolution solution in solutions)
            {
                if (solution.Student is not null)
                    solution.Student.Solutions.Remove(solution);
                _solutionRepository.DeleteSolution(solution.Id);
            }
            _assignmentRepository.DeleteAssignment(assignment.Id);
        }

        return _courseRepository.DeleteCourse(courseId);
    }

    public bool RegisterStudent(int studentId, int courseId)
    {
        if (studentId <= 0 || courseId <= 0)
            return false;

        Course? course = _courseRepository.GetCourseById(courseId);
        if (course is null || _courseRepository.IsStudentRegistered(studentId, courseId))
            return false;

        if (course.RegistrationStartDate.HasValue && DateTime.UtcNow < course.RegistrationStartDate.Value)
            return false;

        if (course.RegistrationEndDate.HasValue && DateTime.UtcNow > course.RegistrationEndDate.Value)
            return false;

        if (!_courseRepository.RegisterStudent(studentId, courseId))
            return false;

        User? user = _userRepository.GetUserByID(studentId);
        if (user is Student student)
            student.EnrolledCourses.Add(course);

        return true;
    }

    public bool IsStudentRegistered(int studentId, int courseId)
    {
        return studentId > 0 && courseId > 0 && _courseRepository.IsStudentRegistered(studentId, courseId);
    }

    public bool AssignTeachingAssistant(int courseId, int assistantId)
    {
        if (courseId <= 0 || assistantId <= 0)
            return false;

        if (!_courseRepository.AssignTeachingAssistant(courseId, assistantId))
            return false;

        User? user = _userRepository.GetUserByID(assistantId);
        if (user is TeachingAssistant assistant)
        {
            Course? course = _courseRepository.GetCourseById(courseId);
            if (course is not null)
                assistant.AssignedCourses.Add(course);
        }

        return true;
    }

    public List<Course> GetCoursesByDoctor(int doctorId)
    {
        if (doctorId <= 0)
            return new List<Course>();

        return _courseRepository.GetCoursesByDoctor(doctorId);
    }

    private static bool IsValidCourse(Course? course)
    {
        return course is not null
            && !string.IsNullOrWhiteSpace(course.Name)
            && !string.IsNullOrWhiteSpace(course.Code)
            && course.CreatedByDoctorId > 0
            && (!course.MaxStudents.HasValue || course.MaxStudents.Value > 0);
    }

    private static void NormalizeCourse(Course course)
    {
        course.Name = course.Name.Trim();
        course.Code = course.Code.Trim().ToUpperInvariant();
        course.Description = string.IsNullOrWhiteSpace(course.Description) ? null : course.Description.Trim();
    }
}
