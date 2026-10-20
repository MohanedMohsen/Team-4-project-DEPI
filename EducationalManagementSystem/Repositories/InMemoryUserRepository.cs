using System;
using System.Collections.Generic;
using System.Text;
using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Repositories
{
    internal class InMemoryUserRepository : IUserRepository
    {
        private static readonly List<User> _users = new()
        {
            new Student { Id = 1, UserID = "STU00102345", Username = "s001", Password = "5001", FullName = "Hussien Samy", Email = "hussien.samy@ems.com", IsActive = true, MustPasswordChange = false },
            new Student { Id = 2, UserID = "STU00204690", Username = "s002", Password = "5002", FullName = "Ashraf Sayed", Email = "ashraf.sayed@ems.com", IsActive = true, MustPasswordChange = false },
            new Student { Id = 3, UserID = "STU00307035", Username = "s003", Password = "5003", FullName = "Mostafa Hussien", Email = "mostafa.hussien@ems.com", IsActive = true, MustPasswordChange = false },
            new Student { Id = 4, UserID = "STU00409380", Username = "s004", Password = "5004", FullName = "Ali Mohamed", Email = "ali.mohamed@ems.com", IsActive = true, MustPasswordChange = false },
            new Student { Id = 5, UserID = "STU00501725", Username = "s005", Password = "5005", FullName = "Hani Sayed", Email = "hani.sayed@ems.com", IsActive = true, MustPasswordChange = false },

            new Doctor { Id = 101, UserID = "DOC101", Username = "dr_ali", Password = "doc123", FullName = "Ali", Email = "ali.doctor@ems.com", IsActive = true, MustPasswordChange = false },
            new Doctor { Id = 102, UserID = "DOC102", Username = "dr_mostafa", Password = "doc123", FullName = "Mostafa", Email = "mostafa.doctor@ems.com", IsActive = true, MustPasswordChange = false },
            new Doctor { Id = 103, UserID = "DOC103", Username = "dr_hani", Password = "doc123", FullName = "Hani", Email = "hani.doctor@ems.com", IsActive = true, MustPasswordChange = false },
            new Doctor { Id = 104, UserID = "DOC104", Username = "dr_mohamed", Password = "doc123", FullName = "Mohamed", Email = "mohamed.doctor@ems.com", IsActive = true, MustPasswordChange = false },
            new Doctor { Id = 105, UserID = "DOC105", Username = "dr_ashraf", Password = "doc123", FullName = "Ashraf", Email = "ashraf.doctor@ems.com", IsActive = true, MustPasswordChange = false },
            new Doctor { Id = 106, UserID = "DOC106", Username = "dr_samy", Password = "doc123", FullName = "Samy", Email = "samy.doctor@ems.com", IsActive = true, MustPasswordChange = false },
            new Doctor { Id = 107, UserID = "DOC107", Username = "dr_morad", Password = "doc123", FullName = "Morad", Email = "morad.doctor@ems.com", IsActive = true, MustPasswordChange = false },
            new Doctor { Id = 108, UserID = "DOC108", Username = "dr_sayed", Password = "doc123", FullName = "Sayed", Email = "sayed.doctor@ems.com", IsActive = true, MustPasswordChange = false },
            new Doctor { Id = 109, UserID = "DOC109", Username = "dr_hussien", Password = "doc123", FullName = "Hussien", Email = "hussien.doctor@ems.com", IsActive = true, MustPasswordChange = false },

            new TeachingAssistant { Id = 201, UserID = "TEA201", Username = "ta_ahmed", Password = "ta123", FullName = "Ahmed TAhseen", Email = "ahmed.ta@ems.com", IsActive = true, MustPasswordChange = false },
            new TeachingAssistant { Id = 202, UserID = "TEA202", Username = "ta_mahmoud", Password = "ta123", FullName = "Mahmoud Ali", Email = "mahmoud.ta@ems.com", IsActive = true, MustPasswordChange = false }
        };
        public int NumOfUsers {
            get {
                int count = 0;
                foreach (var u in _users) count++;
                return count;
            }
        }
        public List<User> GetUsersFromDB(){
            List<User> list = new List<User>();
            foreach( var user in _users)
            {
                list.Add(user);
            }
            return list;
        }
        public bool AddUser(User user)
        {
            if (user == null || user.UserID) return false;
            foreach(var usr in _users)
            {
                if(usr.UserID != null)
                {
                    string id1 = usr.UserID;
                    string id2 = user.UserID;
                    if (id1 != id2) return false;
                }
            }
            _users.Add(user);
            return true;
        }
      public User? GetUserByID(int id)
        {
            foreach(var user in _users)
            {
                if(user.UserID == id)
                {
                    return user;
                }
            }
            return null;
        }
        public int GetNextID()
        {
            int maxID = 0;
            foreach(var user in _users)
            {
                if(user.UserID > maxID) maxID = user.UserID
            }
            return maxID;
        }
    }
}
