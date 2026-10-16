using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Interfaces
{
    public interface ICommunicationService
    {
        bool CreateAnnouncement(
            int doctorId,
            int courseId,
            string title,
            string message);

        bool SendNotification(
            int studentId,
            string message);
    }
}
