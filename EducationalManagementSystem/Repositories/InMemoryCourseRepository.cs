using System.Collections;
using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Repositories;

public sealed class InMemoryCourseRepository : ICourseRepository
{
    private readonly ArrayList _courses = new ArrayList();
    private readonly IUserRepository _userRepository;
    private int _nextId = 1;

    public InMemoryCourseRepository(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public void Add(Course course)
    {
        AddCourse(course);
    }

    public Course? GetById(int id)
    {
        return GetCourseById(id);
    }

    public IEnumerable<Course> GetAll()
    {
        return GetAllCourses();
    }

    public void Update(Course course)
    {
        UpdateCourse(course);
    }

    public void Delete(int id)
    {
        DeleteCourse(id);
    }

    public bool AddCourse(Course course)
    {
        if (course is null)
            return false;

        foreach (Course item in _courses)
        {
            if (course.Id > 0 && item.Id == course.Id)
                return false;
        }

        if (course.Id <= 0)
            course.Id = _nextId;

        if (course.Id >= _nextId)
            _nextId = course.Id + 1;

        _courses.Add(course);
        return true;
    }

    public Course? GetCourseById(int courseId)
    {
        foreach (Course course in _courses)
        {
            if (course.Id == courseId)
                return course;
        }

        return null;
    }

    public List<Course> GetAllCourses()
    {
        List<Course> courses = new List<Course>();
        foreach (Course course in _courses)
            courses.Add(course);

        return courses;
    }

    public bool UpdateCourse(Course course)
    {
        if (course is null || course.Id <= 0)
            return false;

        for (int index = 0; index < _courses.Count; index++)
        {
            Course existingCourse = (Course)_courses[index]!;
            if (existingCourse.Id == course.Id)
            {
                existingCourse.Name = course.Name;
                existingCourse.Code = course.Code;
                existingCourse.Description = course.Description;
                existingCourse.CreatedByDoctorId = course.CreatedByDoctorId;
                existingCourse.CreatedByDoctor = course.CreatedByDoctor;
                existingCourse.MaxStudents = course.MaxStudents;
                existingCourse.RegistrationStartDate = course.RegistrationStartDate;
                existingCourse.RegistrationEndDate = course.RegistrationEndDate;
                return true;
            }
        }

        return false;
    }

    public bool DeleteCourse(int courseId)
    {
        if (courseId <= 0)
            return false;

        for (int index = 0; index < _courses.Count; index++)
        {
            Course course = (Course)_courses[index]!;
            if (course.Id == courseId)
            {
                _courses.RemoveAt(index);
                return true;
            }
        }

        return false;
    }

    public bool RegisterStudent(int studentId, int courseId)
    {
        Course? course = GetCourseById(courseId);
        if (course is null || studentId <= 0)
            return false;

        foreach (Student student in course.RegisteredStudents)
        {
            if (student.Id == studentId)
                return false;
        }

        if (course.MaxStudents.HasValue && course.RegisteredStudents.Count >= course.MaxStudents.Value)
            return false;

        User? user = _userRepository.GetUserByID(studentId);
        if (user is not Student registeredStudent)
            return false;

        course.RegisteredStudents.Add(registeredStudent);
        return true;
    }

    public bool AssignTeachingAssistant(int courseId, int assistantId)
    {
        Course? course = GetCourseById(courseId);
        if (course is null || assistantId <= 0)
            return false;

        foreach (TeachingAssistant assistant in course.TeachingAssistants)
        {
            if (assistant.Id == assistantId)
                return false;
        }

        User? user = _userRepository.GetUserByID(assistantId);
        if (user is not TeachingAssistant teachingAssistant)
            return false;

        course.TeachingAssistants.Add(teachingAssistant);
        return true;
    }

    public List<Course> GetCoursesByDoctor(int doctorId)
    {
        List<Course> courses = new List<Course>();
        foreach (Course course in _courses)
        {
            if (course.CreatedByDoctorId == doctorId)
                courses.Add(course);
        }

        return courses;
    }

    public bool IsStudentRegistered(int studentId, int courseId)
    {
        Course? course = GetCourseById(courseId);
        if (course is null)
            return false;

        foreach (Student student in course.RegisteredStudents)
        {
            if (student.Id == studentId)
                return true;
        }

        return false;
    }
}
