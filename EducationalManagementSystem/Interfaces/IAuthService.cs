using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Interfaces
{
    public interface IAuthService
    {
        // SignUp by full name and email
        (string? UserId, string? tempPassword) SignUp(string FullName, string email);

        User? Login(string userID, string password);

        bool ChangePassword(int userId, string currentPassword, string newPassword);

        bool IsValidPassword(string password);
        bool ValidateUser(User user);
    }
}
