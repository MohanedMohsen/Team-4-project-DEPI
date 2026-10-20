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
        public static void Show(Doctor doctor, ICourseService courseService, IAssignmentService assignmentService)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                WelcomeWindow(doctor);
                Console.WriteLine("1.View My Courses");
                Console.WriteLine("2.Create New Course");
                Console.WriteLine("3.Create Assignment");
                Console.WriteLine("4.Logout");
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
                        CreateAssignmentWindow(doctor, assignmentService);
                        break;
                    case "4":
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
            WelcomeWindow(doctor);
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
                Console.WriteLine("No courses found.");
            }

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }
        public static void CreateCourseWindow(Doctor doctor , ICourseService courseService)
        {
            Console.Clear();
            WelcomeWindow(doctor);
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
                    CreatedByDoctorId = doctor.Id
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

        public static void CreateAssignmentWindow(Doctor doctor, IAssignmentService assignmentService)
        {
            Console.Clear();
            WelcomeWindow(doctor);
            Console.WriteLine("-- Create Assignment --");
            Console.Write("Enter Course ID: ");
            bool validCourseId = int.TryParse(Console.ReadLine(), out int courseId);
            Console.Write("Enter Assignment Title: ");
            string title = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter Description (Optional): ");
            string description = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter Max Grade: ");
            bool validGrade = decimal.TryParse(Console.ReadLine(), out decimal maxGrade);
            Console.Write("Enter Due Date (yyyy-MM-dd, Optional): ");
            string dueDateText = Console.ReadLine() ?? string.Empty;
            DateTime? dueDate = null;
            if (!string.IsNullOrWhiteSpace(dueDateText))
            {
                if (!DateTime.TryParse(dueDateText, out DateTime parsedDueDate))
                    validCourseId = false;
                else
                    dueDate = parsedDueDate.ToUniversalTime();
            }

            bool success = validCourseId && validGrade && assignmentService.CreateAssignment(new Assignment
            {
                CourseId = courseId,
                DoctorCreatorId = doctor.Id,
                Title = title,
                Description = description,
                MaxGrade = maxGrade,
                DueDate = dueDate
            });

            Console.WriteLine(success ? "Assignment created successfully." : "Failed to create assignment. Check the entered data and course ownership.");
            Console.WriteLine("Press Any Key To Return .....");
            Console.ReadKey();
        }
    }
}
