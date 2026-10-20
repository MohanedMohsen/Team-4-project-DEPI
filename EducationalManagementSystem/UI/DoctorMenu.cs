using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.UI
{
    public static class DoctorMenu
    {
        public static void WelcomeWindow(Doctor doctor)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("    ////////   //      //   ////////");
            Console.WriteLine("    //         ////  ////   //      ");
            Console.WriteLine("    ////////   //  //  //   ////////");
            Console.WriteLine("    //         //      //         //");
            Console.WriteLine("    ////////   //      //   ////////");
            Console.WriteLine("==== Welcome To Our Education System ===");
            Console.WriteLine($"\tDr.{doctor.FullName}");
        }
        public void Show(Doctor doctor , ICourseRepository courseRepository)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                WelcomeWindow(doctor);
                Console.WriteLine("1.View My Courses");
                Console.WriteLine("2.Create New Course");
                Console.WriteLine("3.Logout");
                Console.Write("\nSelect an Option: ");
                string choice = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        ViewDoctorCourses(doctor, courseService);
                        break;
                    case "2":
                        CreateCourseWindow(doctor, courseService);
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
        public static void ViewDoctorCourses(Doctor doctor , ICourseService courseService)
        {
            Console.Clear();
            WelcomeWindow(Doctor doctor);
            Console.WriteLine("--- My Courses ---");
            var courses = courseService.GetCoursesByDoctor(doctor.Id);
            int count = 0;
            if(courses != null)
            {
                foreach(var course in courses)
                {
                    Console.WriteLine($"ID : {course.Id} | Code : {course.Code} | Name : {course.Name}");
                    count++;
                }
            }
            if(count == 0)
            {
                Console.WriteLine("Press Any Key To Return ....");
                Console.ReadKey();
            }
        }
        public static void CreateCourseWindow(Doctor doctor , ICourseService courseService)
        {
            Console.Clear();
            WelcomeWindow(Doctor doctor);
            Console.WriteLine("-- Create Course --");
            Console.Write("Enter Course Name : ");
            string courseName = Console.ReadLine() ?? string.Empty;
            Console.Write("\nEnter Course Code : ");
            string courseCode = Console.ReadLine() ?? string.Empty;
            Console.Write("\nEnter Discription (Optional) :");
            string description = Console.ReadLine() ?? string.Empty;
            Console.WriteLine();
            if(string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(courseCode))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error : Course Name and Code cannot be empty");
                Console.ResetColor();
            }
            else
            {
                Course newCourse = new Course
                {
                    Name = courseName,
                    Code = courseCode,
                    Description = description,
                    CreatedByDoctorId = doctor.Id;
                };
                bool Successful = courseService.CreateCourse(newCourse);
                if (Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Course Created Successfully !");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to Create Course , Please Try Again");
                    Console.ResetColor();
                }
            }
            Console.WriteLine("\nPress Any Key To Return .....");
            Console.ReadKey();
        }
    }
}