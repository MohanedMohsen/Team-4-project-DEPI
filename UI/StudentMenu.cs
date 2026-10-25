
using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;
using EducationalManagementSystem.Services;

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
        public static void Show(Student student, ICourseService courseService, IAssignmentService assignmentService , IGradingService gradingService)
        {
   
            if (student == null || courseService == null) return;
            bool back = false;
            while (!back)
            {
                Console.Clear();
                WelcomeWindow(student);
                Console.WriteLine("1. View All Available Courses");
                Console.WriteLine("2. Register in a Course");
                Console.WriteLine("3. View Course Assignments");
                Console.WriteLine("4. Submit Assignment Solution");
                Console.WriteLine("5. View My Grades");
                Console.WriteLine("6. Logout");
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
                        ViewAssignmentsWindow(student, courseService, assignmentService);
                        break;
                    case "4":
                        SubmitSolutionWindow(student,courseService, assignmentService);
                        break;
                    case "5":
                        ViewGradesReportWindow(student, gradingService);
                        break;
                    case "6":
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
            var courses = courseService.GetAllCourses();
            if(courses == null || courses.Count == 0)
            {
                Console.WriteLine("No courses availavble at the moment ...");
            }
            else
            {
                Console.WriteLine("Available Courses : ");
                foreach(var course in courses)
                {
                    Console.WriteLine($"Code : {course.Code} | Name : {course.Name}") ;
                }
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("Enter Course Code to Register (e.g. CS111) : ");
                string courseCode = (Console.ReadLine() ?? string.Empty).Trim();
                Course? targetCourse = null;
                foreach(var course in courses)
                {
                    if(course.Code.Trim() == courseCode)
                    {
                        targetCourse = course;
                        break;
                    }
                }
                if (targetCourse != null)
                {
                    Console.WriteLine();
                    bool success = courseService.RegisterStudent(student.Id, targetCourse.Id);
                    if (success)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully registered in the course!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Registration failed. You might already be registered or deadline has passed.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Course Code or course not found.");
                    Console.ResetColor();
                }
            }
            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();

        }


        public static void ViewAssignmentsWindow(Student student, ICourseService courseService, IAssignmentService assignmentService)
        {
            Console.Clear();
            WelcomeWindow(student);
            Console.WriteLine("-- View Course Assignments --\n");

            var courses = courseService.GetAllCourses();
            if (courses == null || courses.Count == 0)
            {
                Console.WriteLine("No courses available at the moment ..");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Available Courses:");
            foreach (var course in courses)
            {
                Console.WriteLine($"Code: {course.Code} | Name: {course.Name}");
            }
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter Course Code to view assignments (e.g., CS111): ");
            string courseCode = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

            Course? targetCourse = null;
            foreach (var course in courses)
            {
                if (course.Code.Trim().ToUpperInvariant() == courseCode)
                {
                    targetCourse = course;
                    break;
                }
            }

            Console.WriteLine();
            if (targetCourse == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Course Code or course not found.");
                Console.ResetColor();
            }
            else if (!courseService.IsStudentRegistered(student.Id, targetCourse.Id))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You are not registered in this course.");
                Console.ResetColor();
            }
            else
            {
                List<Assignment> assignments = assignmentService.ViewAssignments(targetCourse.Id);
                if (assignments == null || assignments.Count == 0)
                {
                    Console.WriteLine("No assignments available for this course.");
                }
                else
                {
                    Console.WriteLine($"Assignments for [{targetCourse.Code}]:");
                    foreach (Assignment assignment in assignments)
                    {
                        Console.WriteLine($"Assignment ID: {assignment.Id} | Title: {assignment.Title} | Due Date: {assignment.DueDate}");
                    }
                }
            }

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }

        public static void SubmitSolutionWindow(Student student, ICourseService courseService, IAssignmentService assignmentService)
        {
            Console.Clear();
            WelcomeWindow(student);
            Console.WriteLine("-- Submit Assignment Solution --\n");

            var courses = courseService.GetAllCourses();
            if (courses == null || courses.Count == 0)
            {
                Console.WriteLine("No courses available at the moment ..");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Available Courses:");
            foreach (var course in courses)
            {
                Console.WriteLine($"Code: {course.Code} | Name: {course.Name}");
            }
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter Course Code (e.g., CS111): ");
            string courseCode = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

            Course? targetCourse = null;
            foreach (var course in courses)
            {
                if (course.Code.Trim().ToUpperInvariant() == courseCode)
                {
                    targetCourse = course;
                    break;
                }
            }

            if (targetCourse == null)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Course Code or course not found.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            if (!courseService.IsStudentRegistered(student.Id, targetCourse.Id))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You are not registered in this course.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            List<Assignment> assignments = assignmentService.ViewAssignments(targetCourse.Id);
            if (assignments == null || assignments.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No assignments available for this course.");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nAssignments for [{targetCourse.Code}]:");
            int index = 1;
            foreach (Assignment assignment in assignments)
            {
                Console.WriteLine($"{index}. Title: {assignment.Title} | Due Date: {assignment.DueDate} | Max Grade: {assignment.MaxGrade}");
                index++;
            }
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter Assignment Number from the list above: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > assignments.Count)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid assignment number selection.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            Assignment selectedAssignment = assignments[choice - 1];

            Console.Write("Enter Your Solution Content: ");
            string content = Console.ReadLine() ?? string.Empty;

            bool success = assignmentService.SubmitSolution(new AssignmentSolution
            {
                AssignmentId = selectedAssignment.Id,
                StudentId = student.Id,
                Content = content
            });

            Console.WriteLine();
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Solution submitted successfully!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Submission failed. You might have already submitted a solution or deadline has passed.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }
        public static void ViewGradesReportWindow(Student student, IGradingService gradingService)
        {
            Console.Clear();
            WelcomeWindow(student);
            Console.WriteLine("-- My Grades Report --\n");

            if (gradingService == null)
            {
                Console.WriteLine("Grading service is not available.");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            var reports = gradingService.ViewGradesReport(student.Id);
            if (reports == null || reports.Count == 0)
            {
                Console.WriteLine("No submissions or grades found for you yet.");
            }
            else
            {
                Console.WriteLine("--------------------------------------------------");
                foreach (var report in reports)
                {
                    string gradeDisplay = report.IsGraded ? report.Grade.ToString() : "Not Graded";
                    Console.WriteLine($"Assignment ID: {report.AssignmentId}");
                    Console.WriteLine($"Submitted Content: {report.Content}");
                    Console.WriteLine($"Grade: {gradeDisplay}");

                    if (!string.IsNullOrWhiteSpace(report.Comment))
                    {
                        Console.WriteLine($"Feedback / Comment: {report.Comment}");
                    }
                    Console.WriteLine("--------------------------------------------------");
                }
            }

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }
    }
}

