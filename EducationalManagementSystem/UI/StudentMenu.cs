using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.UI
{
    public static class StudentMenu
    {
        public static void WelcomeWindow(Student student)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("    ////////   //      //   ////////");
            Console.WriteLine("    //         ////  ////   //      ");
            Console.WriteLine("    ////////   //  //  //   ////////");
            Console.WriteLine("    //         //      //         //");
            Console.WriteLine("    ////////   //      //   ////////");
            Console.WriteLine("==== Welcome To Our Education System ===");
            Console.WriteLine($"\t{student.FullName}");
        }
        public static void Show(Student student , ICourseService courseService)
        {
   
            if (student == null || courseService == null) return;
            bool back = false;
            while (!back)
            {
                Console.Clear();
                WelcomeWindow();
                Console.WriteLine("1. View All Available Courses");
                Console.WriteLine("2. Register in a Course");
                Console.WriteLine("3. Logout");
                Console.Write("\nSelect an Option : ");
                string choice = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        ViewAllCourses(student, courseService);
                        break;
                    case "2":
                        RegisterCourseWindow(student, courseService);
                        break;
                    case "3":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Option! Press any key to try again.");
                        Console.ReadKey();
                        break;

                }
            }
        }
        public static void ViewAllCourses(Student student , ICourseService courseService)
        {
            Console.Clear();
            WelcomeWindow(student);
            Console.WriteLine("-- View All Courses --");
            var courses = courseService.GetAllCourses();
            int count = 0;
            if(courses != null)
            {
                foreach(var course in courses)
                {
                    Console.WriteLine($"ID: {course.Id} | Code : {course.Code} | Name : {course.Name}");
                    count++;
                }
            }
            if(count == 0)
            {
                Console.WriteLine("No coursese available at the moment ..");
            }
            Console.WriteLine("Press Any Key to return .....");
            Console.ReadKey();
        }
        public static void RegisterCourseWindow(Student student , ICourseService courseService)
        {
            Console.Clear();
            WelcomeWindow(student);
            Console.WriteLine("-- Course Regesteration --");
            Console.Write("Enter course ID to Register : ");
            if(int.TryParse(Console.ReadLine() , out int courseID))
            {
                Console.WriteLine();
                bool success = courseService.RegisterStudent(student.Id, courseID);
                if (success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully registered in the course!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Registration failed. Please check Course ID or if you are already registered.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Course ID format.");
                Console.ResetColor();
            }
            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }
    }
}

