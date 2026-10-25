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
                        ViewSolutionsWindow(assistant, courseService, assignmentService, gradingService);
                        break;
                    case "7":
                        GradeSolutionWindow(assistant, courseService, assignmentService, gradingService);
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
                    Console.WriteLine($"Code : {course.Code} | Name : {course.Name} | Doctor ID : {course.CreatedByDoctorId}");
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
                    Console.WriteLine($"Code : {course.Code} | Name : {course.Name} | Doctor ID : {course.CreatedByDoctorId}");
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
            Console.WriteLine("-- Join Course as Teaching Assistant --\n");

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
                Console.WriteLine($"Code: {course.Code} | Name: {course.Name} | Doctor ID: {course.CreatedByDoctorId}");
            }
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter Course Code to Join (e.g., CS111): ");
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
            if (targetCourse != null)
            {
                bool success = courseService.AssignTeachingAssistant(targetCourse.Id, assistant.Id);
                if (success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully assigned to the course as Teaching Assistant!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to join course. You might already be assigned.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Course Code or course not found.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }

        public static void ViewAssignmentsWindow(TeachingAssistant assistant, ICourseService courseService, IAssignmentService assignmentService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- View Course Assignments --\n");

            var courses = courseService.GetAllCourses();
            if (courses == null || courses.Count == 0)
            {
                Console.WriteLine("No courses available ..");
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

            Console.WriteLine();
            if (targetCourse == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Course Code.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"Assignments for [{targetCourse.Code} - {targetCourse.Name}]:");
                List<Assignment> assignments = assignmentService.ViewAssignments(targetCourse.Id);
                if (assignments == null || assignments.Count == 0)
                {
                    Console.WriteLine("No assignments available for this course.");
                }
                else
                {
                    int index = 1;
                    foreach (Assignment assignment in assignments)
                    {
                        Console.WriteLine($"{index}. Title: {assignment.Title} | Max Grade: {assignment.MaxGrade} | Due Date: {(assignment.DueDate.HasValue ? assignment.DueDate.Value.ToString("yyyy-MM-dd") : "No Due Date")}");
                        if (!string.IsNullOrWhiteSpace(assignment.Description))
                        {
                            Console.WriteLine($"   Description: {assignment.Description}");
                        }
                        index++;
                    }

                    Console.WriteLine("-----------------------------------------");
                    Console.Write("\nEnter Assignment Number to view details (0 to cancel): ");
                    if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= assignments.Count)
                    {
                        Assignment selectedAssignment = assignments[choice - 1];
                        Console.WriteLine($"\n--- Selected Assignment: {selectedAssignment.Title} ---");
                        Console.WriteLine($"Title: {selectedAssignment.Title}");
                        Console.WriteLine($"Max Grade: {selectedAssignment.MaxGrade}");
                        Console.WriteLine($"Due Date: {(selectedAssignment.DueDate.HasValue ? selectedAssignment.DueDate.Value.ToString("yyyy-MM-dd") : "None")}");
                        Console.WriteLine($"Description: {(string.IsNullOrWhiteSpace(selectedAssignment.Description) ? "None" : selectedAssignment.Description)}");
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
            Console.WriteLine("-- Create Assignment --\n");

            var courses = courseService.GetAllCourses();
            if (courses == null || courses.Count == 0)
            {
                Console.WriteLine("No courses available ..");
                Console.WriteLine("\nPress Any Key To Return .....");
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
                Console.WriteLine("Invalid Course Code.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return .....");
                Console.ReadKey();
                return;
            }

            bool isAssigned = assistant.AssignedCourses.Any(c => c.Id == targetCourse.Id) ||
                              targetCourse.TeachingAssistants.Cast<object>().Any(ta => ta is TeachingAssistant a && a.Id == assistant.Id);
            if (!isAssigned)
            {
                courseService.AssignTeachingAssistant(targetCourse.Id, assistant.Id);
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

            Console.WriteLine();
            if (!validGrade || maxGrade <= 0 || !validDate || string.IsNullOrWhiteSpace(title))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to create assignment. Please check the entered data.");
                Console.ResetColor();
            }
            else
            {
                bool success = assignmentService.CreateAssignment(new Assignment
                {
                    CourseId = targetCourse.Id,
                    DoctorCreatorId = targetCourse.CreatedByDoctorId,
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
                    Console.WriteLine("Failed to create assignment.");
                    Console.ResetColor();
                }
            }

            Console.WriteLine("\nPress Any Key To Return .....");
            Console.ReadKey();
        }

        public static void ViewSolutionsWindow(TeachingAssistant assistant, ICourseService courseService, IAssignmentService assignmentService, IGradingService? gradingService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- View Assignment Solutions --\n");
            if (gradingService == null)
            {
                Console.WriteLine("Grading service is not available.");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            var allCourses = courseService.GetAllCourses();
            var myCourses = allCourses?
                .Where(c => c.TeachingAssistants.Cast<object>().Any(ta => ta is TeachingAssistant a && a.Id == assistant.Id) ||
                            assistant.AssignedCourses.Any(ac => ac.Id == c.Id))
                .DistinctBy(c => c.Id)
                .ToList() ?? new List<Course>();

            if (myCourses.Count == 0)
            {
                Console.WriteLine("You are not assigned to any courses yet.");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Your Assigned Courses:");
            foreach (var c in myCourses)
            {
                Console.WriteLine($"Code: {c.Code} | Name: {c.Name}");
            }
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter Course Code (e.g., CS111): ");
            string courseCode = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

            Course? targetCourse = null;
            foreach (var c in myCourses)
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
                Console.WriteLine("Invalid Course Code or you are not assigned to this course.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            var assignments = assignmentService.ViewAssignments(targetCourse.Id);
            if (assignments == null || assignments.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No assignments found for this course.");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nAssignments for [{targetCourse.Code}]:");
            int aIndex = 1;
            foreach (var assignment in assignments)
            {
                Console.WriteLine($"{aIndex}. Title: {assignment.Title} | Max Grade: {assignment.MaxGrade}");
                aIndex++;
            }
            Console.WriteLine("-----------------------------------------");

            Console.Write("Enter Assignment Number: ");
            if (!int.TryParse(Console.ReadLine(), out int aChoice) || aChoice < 1 || aChoice > assignments.Count)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid selection.");
                Console.ResetColor();
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            Assignment selectedAssignment = assignments[aChoice - 1];
            var solutions = gradingService.GetSolutionsByAssignment(selectedAssignment.Id);

            if (solutions == null || solutions.Count == 0)
            {
                Console.WriteLine("\nNo solutions submitted for this assignment yet.");
            }
            else
            {
                Console.WriteLine($"\nFound {solutions.Count} submission(s):");
                Console.WriteLine("--------------------------------------------------");
                int sIndex = 1;
                foreach (var sol in solutions)
                {
                    string studentName = sol.Student != null ? sol.Student.FullName : $"ID {sol.StudentId}";
                    Console.WriteLine($"{sIndex}. Student: {studentName} | Submitted: {sol.SubmittedAt:yyyy-MM-dd HH:mm}");
                    Console.WriteLine($"   Status: {(sol.IsGraded ? $"Graded: {sol.Grade}" : "Not Graded")}");
                    Console.WriteLine($"   Content: {sol.Content}");
                    if (!string.IsNullOrWhiteSpace(sol.Comment))
                    {
                        Console.WriteLine($"   Feedback: {sol.Comment}");
                    }
                    Console.WriteLine("--------------------------------------------------");
                    sIndex++;
                }

                Console.Write("\nWould you like to grade a solution now? (y/n): ");
                string choice = Console.ReadLine() ?? string.Empty;
                if (choice.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    GradeSolutionPromptByList(solutions, selectedAssignment.MaxGrade, gradingService);
                }
            }

            Console.WriteLine("\nPress Any Key To Return ....");
            Console.ReadKey();
        }

        public static void GradeSolutionWindow(TeachingAssistant assistant, ICourseService courseService, IAssignmentService assignmentService, IGradingService? gradingService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- Grade Solution & Feedback --\n");
            if (gradingService == null)
            {
                Console.WriteLine("Grading service is not available.");
                Console.WriteLine("\nPress Any Key To Return ....");
                Console.ReadKey();
                return;
            }

            ViewSolutionsWindow(assistant, courseService, assignmentService, gradingService);
        }

        private static void GradeSolutionPromptByList(List<AssignmentSolution> solutions, decimal maxGrade, IGradingService gradingService)
        {
            Console.Write("\nEnter Submission Number from the list to grade: ");
            if (!int.TryParse(Console.ReadLine(), out int sChoice) || sChoice < 1 || sChoice > solutions.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid selection.");
                Console.ResetColor();
                return;
            }

            AssignmentSolution selectedSolution = solutions[sChoice - 1];

            Console.Write($"Enter Grade (Max {maxGrade}): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal grade) || grade < 0 || grade > maxGrade)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid grade or exceeds max grade.");
                Console.ResetColor();
                return;
            }

            bool gradeSuccess = gradingService.SetGrade(selectedSolution.Id, grade);
            if (!gradeSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to set grade.");
                Console.ResetColor();
                return;
            }

            Console.Write("Enter Feedback / Comment (Optional, press Enter to skip): ");
            string comment = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(comment))
            {
                gradingService.SetComment(selectedSolution.Id, comment);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Solution graded and feedback saved successfully!");
            Console.ResetColor();
        }

        public static void ViewGradesReportWindow(TeachingAssistant assistant, IGradingService? gradingService)
        {
            Console.Clear();
            WelcomeWindow(assistant);
            Console.WriteLine("-- Student Grades Report --\n");
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
                        Console.WriteLine($"Assignment ID: {report.AssignmentId} | Grade: {(report.IsGraded ? report.Grade.ToString() : "Not Graded")}");
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
            Console.WriteLine("-- Course Announcements --\n");
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
