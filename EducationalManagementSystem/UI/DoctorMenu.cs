using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;
using EducationalManagementSystem.Services;     

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
        public static void Show(Doctor doctor, ICourseService courseService, IAssignmentService assignmentService , IGradingService gradingService)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                WelcomeWindow(doctor);
                Console.WriteLine("1.View My Courses");
                Console.WriteLine("2.Create New Course");
                Console.WriteLine("3.Create Assignment");
                Console.WriteLine("4.Grade Student Solutions");
                Console.WriteLine("5.Logout");
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
                        CreateAssignmentWindow(doctor, courseService  , assignmentService);
                        break;
                    case "4":
                        GradeSolutionsWindow(doctor, courseService, assignmentService, gradingService);
                        break;
                    case "5":
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

        public static void CreateAssignmentWindow(Doctor doctor, ICourseService courseService, IAssignmentService assignmentService)
        {
            Console.Clear();
            WelcomeWindow(doctor);
            Console.WriteLine("-- Create Assignment --\n");

            var doctorCourses = courseService.GetCoursesByDoctor(doctor.Id);
            if (doctorCourses == null || doctorCourses.Count == 0)
            {
                Console.WriteLine("You don't have any courses yet to create assignments for.");
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Your Courses:");
            foreach (var c in doctorCourses)
            {
                Console.WriteLine($"Code: {c.Code} | Name: {c.Name}");
            }
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter Course Code (e.g., CS111): ");
            string courseCode = (Console.ReadLine() ?? string.Empty).Trim();

            Course? targetCourse = null;
            foreach (var c in doctorCourses)
            {
                if (c.Code.Trim() == courseCode)
                {
                    targetCourse = c;
                    break;
                }
            }

            if (targetCourse == null)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Course Code or you don't own this course.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Assignment Title: ");
            string title = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter Description (Optional): ");
            string description = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter Max Grade: ");
            bool validGrade = decimal.TryParse(Console.ReadLine(), out decimal maxGrade);

            Console.Write("Enter Due Date (yyyy-MM-dd, Optional): ");
            string dueDateText = Console.ReadLine() ?? string.Empty;

            DateTime? dueDate = null;
            bool isValidDate = true;
            if (!string.IsNullOrWhiteSpace(dueDateText))
            {
                if (!DateTime.TryParse(dueDateText, out DateTime parsedDueDate))
                    isValidDate = false;
                else
                    dueDate = parsedDueDate.ToUniversalTime();
            }

            bool success = validGrade && isValidDate && assignmentService.CreateAssignment(new Assignment
            {
                CourseId = targetCourse.Id, 
                DoctorCreatorId = doctor.Id,
                Title = title,
                Description = description,
                MaxGrade = maxGrade,
                DueDate = dueDate
            });

            Console.WriteLine();
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Assignment created successfully.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to create assignment. Check the entered data.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress Any Key To Return .....");
            Console.ReadKey();
        }
        public static void GradeSolutionsWindow(Doctor doctor, ICourseService courseService, IAssignmentService assignmentService, IGradingService gradingService)
        {
            Console.Clear();
            WelcomeWindow(doctor);
            Console.WriteLine("-- Grade Student Solutions --\n");

            var doctorCourses = courseService.GetCoursesByDoctor(doctor.Id);
            if (doctorCourses == null || doctorCourses.Count == 0)
            {
                Console.WriteLine("You don't have any courses yet.");
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Your Courses:");
            foreach (var c in doctorCourses)
            {
                Console.WriteLine($"Code: {c.Code} | Name: {c.Name}");
            }
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter Course Code (e.g., CS111): ");
            string courseCode = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

            Course? targetCourse = null;
            foreach (var c in doctorCourses)
            {
                if (c.Code.Trim().ToUpperInvariant() == courseCode)
                {
                    targetCourse = c;
                    break;
                }
            }

            if (targetCourse == null)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Course Code or you don't own this course.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            var assignments = assignmentService.ViewAssignments(targetCourse.Id);
            if (assignments == null || assignments.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No assignments found for this course.");
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nAssignments for [{targetCourse.Code}]:");
            int assignmentIndex = 1;
            foreach (var assignment in assignments)
            {
                Console.WriteLine($"{assignmentIndex}. Title: {assignment.Title} | Max Grade: {assignment.MaxGrade} | Due: {assignment.DueDate}");
                assignmentIndex++;
            }
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter Assignment Number from the list above: ");
            if (!int.TryParse(Console.ReadLine(), out int assignmentChoice) || assignmentChoice < 1 || assignmentChoice > assignments.Count)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid assignment number selection.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            Assignment selectedAssignment = assignments[assignmentChoice - 1];

            var solutions = gradingService.GetSolutionsByAssignment(selectedAssignment.Id);
            if (solutions == null || solutions.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No student submissions found for this assignment.");
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nSubmissions for [{selectedAssignment.Title}]:");
            int solutionIndex = 1;
            foreach (var sol in solutions)
            {
                Console.WriteLine($"{solutionIndex}. Student ID: {sol.StudentId} | Current Grade: {sol.Grade} | Content: {sol.Content}");
                solutionIndex++;
            }
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter Submission Number to grade: ");
            if (!int.TryParse(Console.ReadLine(), out int solutionChoice) || solutionChoice < 1 || solutionChoice > solutions.Count)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid submission number selection.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            AssignmentSolution selectedSolution = solutions[solutionChoice - 1];

            Console.Write($"Enter Grade (Max allowed: {selectedAssignment.MaxGrade}): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal grade))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Grade format.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            bool success = gradingService.SetGrade(selectedSolution.Id, grade);
            Console.WriteLine();
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Solution graded successfully!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to grade. Ensure the grade does not exceed the assignment Max Grade.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress Any Key To Return .....");
            Console.ReadKey();
        }
    }
}
