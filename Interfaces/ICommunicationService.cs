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

        void DisplayAnnouncements();

        void DisplayNotifications(int studentId);
    }
}
