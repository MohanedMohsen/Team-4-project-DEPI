using EducationalManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetUsersFromDB();

        bool AddUser(User user);

        User? GetUserByID(int id);

        int GetNextID();

        int NumOfUsers { get; }
    }
}
