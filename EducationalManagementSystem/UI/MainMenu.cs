using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;
using EducationalManagementSystem.Models.Enums;

namespace EducationalManagementSystem.UI;
public static class MainMenu
{
    public static void WelcomeWindow()
    {
        Console.WriteLine("=========================================");
        Console.WriteLine("    ////////   //      //   ////////");
        Console.WriteLine("    //         ////  ////   //      ");
        Console.WriteLine("    ////////   //  //  //   ////////");
        Console.WriteLine("    //         //      //         //");
        Console.WriteLine("    ////////   //      //   ////////");
        Console.WriteLine("==== Welcome To Our Education System ===");
    }
    public static void Start(IAuthService authService, ICourseService courseService, IAssignmentService assignmentService,
                              IGradingService gradingService, ICommunicationService communicationService)
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            WelcomeWindow();
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Sign-up ( Student )");
            Console.WriteLine("3. Exit");
            Console.WriteLine("Select an Option : ");
            string choice = Console.ReadLine() ?? string.Empty;
            switch (choice)
            {
                case "1":
                    LoginWindow(authService, courseService, assignmentService, gradingService, communicationService);
                    break;
                case "2":
                    SignupWindow(authService);
                    break;
                case "3":
                    exit = true;
                    Console.WriteLine("Thank you for using the system , Goodbye !");
                    break;
                default:
                    Console.WriteLine("Invalid Option ! Press Any key to try again");
                    Console.ReadKey();
                    break;
            }

        }
    }

    public static void LoginWindow(IAuthService authService, ICourseService courseService, IAssignmentService assignmentService,
                              IGradingService gradingService, ICommunicationService communicationService)
    {
        Console.Clear();
        WelcomeWindow();
        Console.WriteLine("              Login Window              ");
        Console.Write("Enter UserID: ");
        string userId = Console.ReadLine() ?? string.Empty;
        Console.Write("\nEnter Password: ");
        string password = Console.ReadLine() ?? string.Empty;
        Console.WriteLine();
        User? user = authService.Login(userId, password);
        if (user != null)
        {
            Console.WriteLine("Press Any Key To Enter Your Dashboard ..... ");
            Console.ReadKey();
            if (user is Doctor)
            {
                DoctorMenu.Show((Doctor)user, courseService, assignmentService);
            }
            else if (user is Student)
            {
                StudentMenu.Show((Student)user, courseService, assignmentService);
            }
            else
            {
                Console.WriteLine("Dashboard Working ....... ");
            }
        }
        else
        {
            Console.WriteLine("Press Any Key To Try Again ...");
            Console.ReadKey();
        }
    }

    public static void SignupWindow(IAuthService authService)
    {
        Console.Clear();
        WelcomeWindow();
        Console.WriteLine("           Student SignUp Window          ");
        string fullName;
        do
        {
            Console.Write("Enter Your Full Name : ");
            fullName = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(fullName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Name Cannot be Empty . Please Try Again .");
                Console.ResetColor();
            }
        } while (string.IsNullOrWhiteSpace(fullName));
        string? userId = null;
        while (userId == null)
        {
            Console.Write("\nEnter Your Email (Gmail or Hotmail) : ");
            string email = Console.ReadLine() ?? string.Empty;
            var result = authService.SignUp(fullName, email);
            userId = result.UserId;
        }
        Console.WriteLine("\nPress Any Key To Return To Main Menu ....");
        Console.ReadKey();
    }
}
