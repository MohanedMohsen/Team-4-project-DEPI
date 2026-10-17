using System;
using System.Collections.Generic;
using EducationalManagementSystem.Interfaces;

namespace EducationalManagementSystem.Services
{
    internal class CommunicationService : ICommunicationService
    {
        private class Announcement
        {
            public int DoctorId { get; set; }
            public int CourseId { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; } = DateTime.Now;
        }

        private class Notification
        {
            public int StudentId { get; set; }
            public string Message { get; set; } = string.Empty;
            public DateTime SentAt { get; set; } = DateTime.Now;
        }

        private readonly List<Announcement> announcements = new();
        private readonly List<Notification> notifications = new();

        public bool CreateAnnouncement(
            int doctorId,
            int courseId,
            string title,
            string message)
        {
            if (doctorId <= 0 || courseId <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(message))
                return false;

            announcements.Add(new Announcement
            {
                DoctorId = doctorId,
                CourseId = courseId,
                Title = title,
                Message = message
            });

            Console.WriteLine("Announcement published successfully.");
            return true;
        }

        public bool SendNotification(
            int studentId,
            string message)
        {
            if (studentId <= 0 ||
                string.IsNullOrWhiteSpace(message))
                return false;

            notifications.Add(new Notification
            {
                StudentId = studentId,
                Message = message
            });

            Console.WriteLine("Notification saved successfully.");
            return true;
        }

        public void DisplayAnnouncements()
        {
            Console.WriteLine("\n===== Course Announcements =====");

            if (announcements.Count == 0)
            {
                Console.WriteLine("No announcements available.");
                return;
            }

            foreach (var announcement in announcements)
            {
                Console.WriteLine($"Course ID: {announcement.CourseId}");
                Console.WriteLine($"Doctor ID: {announcement.DoctorId}");
                Console.WriteLine($"Title: {announcement.Title}");
                Console.WriteLine($"Message: {announcement.Message}");
                Console.WriteLine($"Date: {announcement.CreatedAt}");
                Console.WriteLine("--------------------------------");
            }
        }

        public void DisplayNotifications(int studentId)
        {
            Console.WriteLine("\n===== My Notifications =====");

            bool found = false;

            foreach (var notification in notifications)
            {
                if (notification.StudentId == studentId)
                {
                    Console.WriteLine($"Message: {notification.Message}");
                    Console.WriteLine($"Date: {notification.SentAt}");
                    Console.WriteLine("--------------------------------");
                    found = true;
                }
            }

            if (!found)
                Console.WriteLine("No notifications available.");
        }
    }
}
