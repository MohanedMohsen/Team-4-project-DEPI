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

        var ta1 = _userRepository.GetUserByID(201) as TeachingAssistant;
        var ta2 = _userRepository.GetUserByID(202) as TeachingAssistant;

        var c1 = new Course { Id = 1, Name = "Prog 1", Code = "CS111", CreatedByDoctorId = 106, Description = "Introduction to Programming" };
        var c2 = new Course { Id = 2, Name = "Prog 2", Code = "CS112", CreatedByDoctorId = 107, Description = "Object Oriented Programming" };
        var c3 = new Course { Id = 3, Name = "Math 1", Code = "CS123", CreatedByDoctorId = 105, Description = "Calculus & Linear Algebra" };
        var c4 = new Course { Id = 4, Name = "Math 2", Code = "CS333", CreatedByDoctorId = 103, Description = "Discrete Mathematics" };
        var c5 = new Course { Id = 5, Name = "Prog 3", Code = "CS136", CreatedByDoctorId = 108, Description = "Data Structures & Algorithms" };

        if (ta1 != null)
        {
            c1.TeachingAssistants.Add(ta1);
            c2.TeachingAssistants.Add(ta1);
            ta1.AssignedCourses.Add(c1);
            ta1.AssignedCourses.Add(c2);
        }

        if (ta2 != null)
        {
            c4.TeachingAssistants.Add(ta2);
            ta2.AssignedCourses.Add(c4);
        }

        _courses.Add(c1);
        _courses.Add(c2);
        _courses.Add(c3);
        _courses.Add(c4);
        _courses.Add(c5);

        _nextId = 6;
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
