using System;
using System.Linq;
using System.Collections.Generic;
using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Repositories
{
   
    public class AuthService : IAuthService
    {
        #region DataBase
        private static List<User> usersDataBase = new List<User>()
        {
    new Student
    {
        Id = 1,
        UserID = "STU00102345",
        Username = "s001",
        Password = "5001",
        FullName = "Hussien Samy",
        Email = "hussien.samy@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Student
    {
        Id = 2,
        UserID = "STU00204690",
        Username = "s002",
        Password = "5002",
        FullName = "Ashraf Sayed",
        Email = "ashraf.sayed@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Student
    {
        Id = 3,
        UserID = "STU00307035",
        Username = "s003",
        Password = "5003",
        FullName = "Mostafa Hussien",
        Email = "mostafa.hussien@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Student
    {
        Id = 4,
        UserID = "STU00409380",
        Username = "s004",
        Password = "5004",
        FullName = "Ali Mohamed",
        Email = "ali.mohamed@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Student
    {
        Id = 5,
        UserID = "STU00501725",
        Username = "s005",
        Password = "5005",
        FullName = "Hani Sayed",
        Email = "hani.sayed@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },

    new Doctor
    {
        Id = 101,
        UserID = "DOC101",
        Username = "dr_ali",
        Password = "doc123",
        FullName = "Ali",
        Email = "ali.doctor@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Doctor
    {
        Id = 102,
        UserID = "DOC102",
        Username = "dr_mostafa",
        Password = "doc123",
        FullName = "Mostafa",
        Email = "mostafa.doctor@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Doctor
    {
        Id = 103,
        UserID = "DOC103",
        Username = "dr_hani",
        Password = "doc123",
        FullName = "Hani",
        Email = "hani.doctor@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Doctor
    {
        Id = 104,
        UserID = "DOC104",
        Username = "dr_mohamed",
        Password = "doc123",
        FullName = "Mohamed",
        Email = "mohamed.doctor@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Doctor
    {
        Id = 105,
        UserID = "DOC105",
        Username = "dr_ashraf",
        Password = "doc123",
        FullName = "Ashraf",
        Email = "ashraf.doctor@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Doctor
    {
        Id = 106,
        UserID = "DOC106",
        Username = "dr_samy",
        Password = "doc123",
        FullName = "Samy",
        Email = "samy.doctor@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Doctor
    {
        Id = 107,
        UserID = "DOC107",
        Username = "dr_morad",
        Password = "doc123",
        FullName = "Morad",
        Email = "morad.doctor@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Doctor
    {
        Id = 108,
        UserID = "DOC108",
        Username = "dr_sayed",
        Password = "doc123",
        FullName = "Sayed",
        Email = "sayed.doctor@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new Doctor
    {
        Id = 109,
        UserID = "DOC109",
        Username = "dr_hussien",
        Password = "doc123",
        FullName = "Hussien",
        Email = "hussien.doctor@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },

    new TeachingAssistant
    {
        Id = 201,
        UserID = "TEA201",
        Username = "ta_ahmed",
        Password = "ta123",
        FullName = "Ahmed TAhseen",
        Email = "ahmed.ta@ems.com",
        IsActive = true,
        MustPasswordChange = false
    },
    new TeachingAssistant
    {
        Id = 202,
        UserID = "TEA202",
        Username = "ta_mahmoud",
        Password = "ta123",
        FullName = "Mahmoud Ali",
        Email = "mahmoud.ta@ems.com",
        IsActive = true,
        MustPasswordChange = false
    }
};
        #endregion
        private static int id_Counter = 1000;

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return email.Contains("@") && email.Contains(".") && (email.Contains("gmail") || email.Contains("hotmail")) && email.IndexOf('.') > 0;
        }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8) return false;
            int letterCount = 0;
            int digitCount = 0;
            foreach (char ch in password)
            {
                if (char.IsLetter(ch)) letterCount++;
                else if (char.IsDigit(ch)) digitCount++;
            }
            return letterCount >= 3 && digitCount >= 3;
        }

        public (string? UserId, string? tempPassword) SignUp(string FullName, string email)
        {
            if (!IsValidEmail(email))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error !! | Email's Format is wrong !");
                Console.ResetColor();
                return (null, null);
            }

            int numericId = id_Counter++;
            string generatedId = "STU" + numericId;
            string tempPassword = "Temp" + new Random().Next(100, 999);

            Student newStudent = new Student
            {
                Id = numericId,
                UserID = generatedId,
                Username = generatedId,
                Password = tempPassword,
                FullName = FullName,
                Email = email,
                MustPasswordChange = true,
                IsActive = true
            };

            usersDataBase.Add(newStudent);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successful ! :) : The Creation of Account is Done !");
            Console.WriteLine($"Your UserID is : {generatedId}");
            Console.WriteLine($"Your Temporary Password: {tempPassword}");
            Console.WriteLine("Please Don't Forget This Data , Now Please Sign in\nTo Create your password !");
            Console.ResetColor();

            return (generatedId, tempPassword);
        }

        public User? Login(string userID, string password)
        {
            User? targetUser = null;
            foreach (var user in usersDataBase)
            {
                if (user.UserID == userID)
                {
                    targetUser = user;
                    break;
                }
            }

            if (targetUser == null || targetUser.Password != password)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Sign in Failed");
                Console.ResetColor();
                return null;
            }

            if (targetUser.MustPasswordChange)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Welcome  ,  {targetUser.FullName}");
                Console.WriteLine($"You Should Change Your Temporary Password");
                Console.WriteLine("Rules : Password should be contain 3 charachters and 3 numbers at least , with length 8");
                Console.ResetColor();

                string newPassword;
                do
                {
                    Console.WriteLine("Enter Your New Password :");
                    newPassword = Console.ReadLine() ?? string.Empty;
                    if (!IsValidPassword(newPassword))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error !! : The Password's Rules is not Applied , Please Try Again !!");
                        Console.ResetColor();
                    }
                } while (!IsValidPassword(newPassword));

                ChangePassword(targetUser.Id, targetUser.Password, newPassword);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Password Changed Successful !!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Welcome Back ,  {targetUser.FullName}");
                Console.ResetColor();
            }

            return targetUser;
        }

        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            foreach (var user in usersDataBase)
            {
                if (user.Id == userId && user.Password == currentPassword)
                {
                    user.Password = newPassword;
                    user.MustPasswordChange = false;
                    return true;
                }
            }
            return false;
        }

        public bool ValidateUser(User user)
        {
            if (user == null) return false;
            return user.IsActive;
        }
    }
}
