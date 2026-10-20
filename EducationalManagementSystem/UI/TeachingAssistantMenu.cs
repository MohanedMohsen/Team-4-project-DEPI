using System;
using System.Collections.Generic;
using System.Linq;
using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.UI
{
    public static class TeachingAssistantMenu
    {
        public static void WelcomeWindow(TeachingAssistant assistant)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("    ////////   //      //   ////////");
            Console.WriteLine("    //         ////  ////   //      ");
            Console.WriteLine("    ////////   //  //  //   ////////");
            Console.WriteLine("    //         //      //         //");
            Console.WriteLine("    ////////   //      //   ////////");
            Console.WriteLine("==== Welcome To Our Education System ===");
            Console.WriteLine($"\tTA. {assistant.FullName}");
        }

        public static void Show(
            TeachingAssistant assistant,
            ICourseService courseService,
            IAssignmentService assignmentService,
            IGradingService? gradingService = null,
            ICommunicationService? communicationService = null)
        {
            if (assistant == null || courseService == null) return;
            bool back = false;
            while (!back)
            {
                Console.Clear();
                WelcomeWindow(assistant);
                Console.WriteLine("1. View My Assigned Courses");
                Console.WriteLine("2. View All Available Courses");
                Console.WriteLine("3. Join Course as Teaching Assistant");
                Console.WriteLine("4. View Course Assignments");
                Console.WriteLine("5. Create Assignment");
                Console.WriteLine("6. View Assignment Solutions");
                Console.WriteLine("7. Grade Solution & Set Comment");
                Console.WriteLine("8. View Student Grades Report");
                Console.WriteLine("9. View Course Announcements");
                Console.WriteLine("10. Logout");
                Console.Write("\nSelect an Option : ");
                string choice = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        ViewMyCourses(assistant, courseService);
                        break;
                    case "2":
                        ViewAllCourses(assistant, courseService);
                        break;
                    case "3":
                        JoinCourseWindow(assistant, courseService);
                        break;
                    case "4":
                        ViewAssignmentsWindow(assistant, courseService, assignmentService);
                        break;
                    case "5":
                        CreateAssignmentWindow(assistant, courseService, assignmentService);
                        break;
                    case "6":
                        ViewSolutionsWindow(assistant, gradingService);
                        break;
                    case "7":
                        GradeSolutionWindow(assistant, gradingService);
                        break;
                    case "8":
                        ViewGradesReportWindow(assistant, gradingService);
                        break;
                    case "9":
                        ViewAnnouncementsWindow(assistant, communicationService);
                        break;
                    case "10":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Option! Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public static void ViewMyCourses(TeachingAssistant assistant, ICourseService courseService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("--- My Assigned Courses ---");
            var allCourses = courseService.GetAllCourses();
            var myCourses = allCourses?
                .Where(c => c.TeachingAssistants.Cast<object>().Any(ta => ta is TeachingAssistant a && a.Id == assistant.Id) ||
                            assistant.AssignedCourses.Any(ac => ac.Id == c.Id))
                .DistinctBy(c => c.Id)
                .ToList() ?? new List<Course>();

            if (myCourses.Count > 0)
            {
                foreach (var course in myCourses)
                {
                    Console.WriteLine($"ID : {course.Id} | Code : {course.Code} | Name : {course.Name} | Doctor ID : {course.CreatedByDoctorId}");
                }
            }
            else
            {
                Console.WriteLine("You are not assigned to any courses yet.");
            }

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }

        public static void ViewAllCourses(TeachingAssistant assistant, ICourseService courseService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- View All Courses --");
            var courses = courseService.GetAllCourses();
            int count = 0;
            if (courses != null)
            {
                foreach (var course in courses)
                {
                    Console.WriteLine($"ID: {course.Id} | Code : {course.Code} | Name : {course.Name} | Doctor ID : {course.CreatedByDoctorId}");
                    count++;
                }
            }
            if (count == 0)
            {
                Console.WriteLine("No courses available at the moment ..");
            }
            Console.WriteLine("\nPress Any Key to return .....");
            Console.ReadKey();
        }

        public static void JoinCourseWindow(TeachingAssistant assistant, ICourseService courseService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- Join Course as Teaching Assistant --");
            Console.Write("Enter Course ID to Join : ");
            if (int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine();
                bool success = courseService.AssignTeachingAssistant(courseId, assistant.Id);
                if (success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully assigned to the course as Teaching Assistant!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to join course. Course not found or you are already assigned.");
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

        public static void ViewAssignmentsWindow(TeachingAssistant assistant, ICourseService courseService, IAssignmentService assignmentService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- View Course Assignments --");
            Console.Write("Enter Course ID: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Course ID format.");
                Console.ResetColor();
            }
            else
            {
                var course = courseService.GetCourseById(courseId);
                if (course == null)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Course not found.");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"\nAssignments for [{course.Code} - {course.Name}]:");
                    List<Assignment> assignments = assignmentService.ViewAssignments(courseId);
                    if (assignments == null || assignments.Count == 0)
                    {
                        Console.WriteLine("No assignments available for this course.");
                    }
                    else
                    {
                        foreach (Assignment assignment in assignments)
                        {
                            Console.WriteLine($"ID: {assignment.Id} | Title: {assignment.Title} | Max Grade: {assignment.MaxGrade} | Due Date: {(assignment.DueDate.HasValue ? assignment.DueDate.Value.ToString("yyyy-MM-dd") : "No Due Date")}");
                            if (!string.IsNullOrWhiteSpace(assignment.Description))
                            {
                                Console.WriteLine($"   Description: {assignment.Description}");
                            }
                        }
                    }
                }
            }
            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }

        public static void CreateAssignmentWindow(TeachingAssistant assistant, ICourseService courseService, IAssignmentService assignmentService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- Create Assignment --");
            Console.Write("Enter Course ID: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Course ID format.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            var course = courseService.GetCourseById(courseId);
            if (course == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Course not found.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            // Assign assistant to course if not assigned yet
            bool isAssigned = assistant.AssignedCourses.Any(c => c.Id == courseId) ||
                              course.TeachingAssistants.Cast<object>().Any(ta => ta is TeachingAssistant a && a.Id == assistant.Id);
            if (!isAssigned)
            {
                courseService.AssignTeachingAssistant(courseId, assistant.Id);
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
            bool validDate = true;
            if (!string.IsNullOrWhiteSpace(dueDateText))
            {
                if (DateTime.TryParse(dueDateText, out DateTime parsedDueDate))
                {
                    dueDate = parsedDueDate.ToUniversalTime();
                }
                else
                {
                    validDate = false;
                }
            }

            if (!validGrade || maxGrade <= 0 || !validDate || string.IsNullOrWhiteSpace(title))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to create assignment. Please check the entered data (Title cannot be empty, Max Grade must be > 0, valid Date).");
                Console.ResetColor();
            }
            else
            {
                bool success = assignmentService.CreateAssignment(new Assignment
                {
                    CourseId = courseId,
                    DoctorCreatorId = course.CreatedByDoctorId,
                    Title = title,
                    Description = description,
                    MaxGrade = maxGrade,
                    DueDate = dueDate
                });

                if (success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Assignment created successfully.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to create assignment. Please check assignment details.");
                    Console.ResetColor();
                }
            }

            Console.WriteLine("\nPress Any Key To Return .....");
            Console.ReadKey();
        }

        public static void ViewSolutionsWindow(TeachingAssistant assistant, IGradingService? gradingService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- View Assignment Solutions --");
            if (gradingService == null)
            {
                Console.WriteLine("Grading service is not available.");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Assignment ID: ");
            if (!int.TryParse(Console.ReadLine(), out int assignmentId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Assignment ID format.");
                Console.ResetColor();
            }
            else
            {
                var solutions = gradingService.GetSolutionsByAssignment(assignmentId);
                if (solutions == null || solutions.Count == 0)
                {
                    Console.WriteLine("\nNo solutions submitted for this assignment yet.");
                }
                else
                {
                    Console.WriteLine($"\nFound {solutions.Count} submission(s):");
                    Console.WriteLine("--------------------------------------------------");
                    foreach (var sol in solutions)
                    {
                        string studentName = sol.Student != null ? sol.Student.FullName : $"ID {sol.StudentId}";
                        Console.WriteLine($"Solution ID: {sol.Id} | Student: {studentName} (ID: {sol.StudentId})");
                        Console.WriteLine($"Submitted At: {sol.SubmittedAt:yyyy-MM-dd HH:mm}");
                        Console.WriteLine($"Status: {(sol.IsGraded ? $"Graded: {sol.Grade}" : "Not Graded")}");
                        Console.WriteLine($"Solution Content: {sol.Content}");
                        if (!string.IsNullOrWhiteSpace(sol.Comment))
                        {
                            Console.WriteLine($"Feedback / Comment: {sol.Comment}");
                        }
                        Console.WriteLine("--------------------------------------------------");
                    }

                    Console.Write("\nWould you like to grade a solution now? (y/n): ");
                    string choice = Console.ReadLine() ?? string.Empty;
                    if (choice.Equals("y", StringComparison.OrdinalIgnoreCase))
                    {
                        GradeSolutionPrompt(gradingService);
                    }
                }
            }

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }

        public static void GradeSolutionWindow(TeachingAssistant assistant, IGradingService? gradingService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- Grade Solution & Feedback --");
            if (gradingService == null)
            {
                Console.WriteLine("Grading service is not available.");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            GradeSolutionPrompt(gradingService);

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }

        private static void GradeSolutionPrompt(IGradingService gradingService)
        {
            Console.Write("Enter Solution ID: ");
            if (!int.TryParse(Console.ReadLine(), out int solutionId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Solution ID format.");
                Console.ResetColor();
                return;
            }

            var solution = gradingService.ViewSolution(solutionId);
            if (solution == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Solution not found.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("\n--- Solution Details ---");
            Console.WriteLine($"Solution ID: {solution.Id}");
            Console.WriteLine($"Student ID: {solution.StudentId}");
            Console.WriteLine($"Assignment ID: {solution.AssignmentId}");
            Console.WriteLine($"Submitted Content:\n{solution.Content}");
            Console.WriteLine($"Current Grade: {(solution.IsGraded ? solution.Grade.ToString() : "Not Graded")}");
            Console.WriteLine($"Current Comment: {(string.IsNullOrWhiteSpace(solution.Comment) ? "None" : solution.Comment)}");
            Console.WriteLine("------------------------");

            Console.Write("Enter Grade: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal grade) || grade < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid grade. Grade must be a non-negative number.");
                Console.ResetColor();
                return;
            }

            bool gradeSuccess = gradingService.SetGrade(solutionId, grade);
            if (!gradeSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to set grade. Please ensure the grade does not exceed the assignment's max grade.");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter Feedback / Comment (Optional, press Enter to skip): ");
            string comment = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(comment))
            {
                gradingService.SetComment(solutionId, comment);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Solution graded and feedback saved successfully!");
            Console.ResetColor();
        }

        public static void ViewGradesReportWindow(TeachingAssistant assistant, IGradingService? gradingService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- Student Grades Report --");
            if (gradingService == null)
            {
                Console.WriteLine("Grading service is not available.");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Student ID format.");
                Console.ResetColor();
            }
            else
            {
                var reports = gradingService.ViewGradesReport(studentId);
                if (reports == null || reports.Count == 0)
                {
                    Console.WriteLine("\nNo submissions or grades found for this student.");
                }
                else
                {
                    Console.WriteLine($"\n--- Grades Report for Student ID: {studentId} ---");
                    foreach (var report in reports)
                    {
                        Console.WriteLine($"Solution ID: {report.Id} | Assignment ID: {report.AssignmentId}");
                        Console.WriteLine($"Grade: {(report.IsGraded ? report.Grade.ToString() : "Not Graded")}");
                        if (!string.IsNullOrWhiteSpace(report.Comment))
                        {
                            Console.WriteLine($"Feedback: {report.Comment}");
                        }
                        Console.WriteLine("--------------------------------------------------");
                    }
                }
            }

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }

        public static void ViewAnnouncementsWindow(TeachingAssistant assistant, ICommunicationService? communicationService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- Course Announcements --");
            if (communicationService == null)
            {
                Console.WriteLine("Communication service is not available.");
            }
            else
            {
                communicationService.DisplayAnnouncements();
            }

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }
    }

    public static class AssistantMenu
    {
        public static void Show(
            TeachingAssistant assistant,
            ICourseService courseService,
            IAssignmentService assignmentService,
            IGradingService? gradingService = null,
            ICommunicationService? communicationService = null)
        {
            TeachingAssistantMenu.Show(assistant, courseService, assignmentService, gradingService, communicationService);
        }
    }
}
