using System;
using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Repositories;
using EducationalManagementSystem.Services;
using EducationalManagementSystem.UI;

namespace EducationalManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            IUserRepository userRepository = new InMemoryUserRepository();
            ICourseRepository courseRepository = new InMemoryCourseRepository(userRepository);
            IAssignmentRepository assignmentRepository = new InMemoryAssignmentRepository();
            IAssignmentSolutionRepository solutionRepository = new InMemoryAssignmentSolutionRepository();

            IAuthService authService = new EducationalManagementSystem.Repositories.AuthService(userRepository);
            ICourseService courseService = new CourseService(courseRepository, assignmentRepository, solutionRepository, userRepository);
            IAssignmentService assignmentService = new AssignmentService(assignmentRepository, solutionRepository, courseRepository, userRepository, new AssignmentValidator());
            IGradingService gradingService = new GradingService(solutionRepository, assignmentRepository);
            ICommunicationService communicationService = new CommunicationService(courseRepository, userRepository);
            MainMenu.Start(
                authService,
                courseService,
                assignmentService,
                gradingService,
                communicationService
            );
        }
    }
}
