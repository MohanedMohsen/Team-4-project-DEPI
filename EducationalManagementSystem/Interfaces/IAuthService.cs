using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Interfaces
{
    public interface IAuthService
    {
        bool CreateAccount(User user);

        User? Login(string username, string password);

        bool ChangePassword(
            int userId,
            string currentPassword,
            string newPassword);

        bool ValidateUser(User user);
    }
}
