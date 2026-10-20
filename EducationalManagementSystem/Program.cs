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
            IAuthService authService = new AuthService();

            ICourseService? courseService = null;
            IAssignmentService? assignmentService = null;
            IGradingService? gradingService = null;
            ICommunicationService? communicationService = null;
            MainMenu.Start(
                authService,
                courseService!,
                assignmentService!,
                gradingService!,
                communicationService!
            );
        }
    }
}