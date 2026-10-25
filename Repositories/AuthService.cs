using System;
using System.Collections.Generic;
using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Models;

namespace EducationalManagementSystem.Repositories
{

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository));
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            int atIndex = email.IndexOf('@');
            int dotIndex = email.LastIndexOf('.');
            return atIndex > 0 && dotIndex > atIndex + 1 && dotIndex < email.Length - 1;
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
            email = email.Trim();
            if (string.IsNullOrWhiteSpace(FullName) || !IsValidEmail(email))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error !! | Email's Format is wrong !");
                Console.ResetColor();
                return (null, null);
            }

            foreach (User user in _userRepository.GetUsersFromDB())
            {
                if (string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
                    return (null, null);
            }

            int numericId = _userRepository.GetNextID();
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

            bool isAdded = _userRepository.AddUser(newStudent);
            if (!isAdded) return (null, null);
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
            var users = _userRepository.GetUsersFromDB();
            foreach (var user in users)
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
            var user = _userRepository.GetUserByID(userId);
            if(user!=null && user.Password == currentPassword && IsValidPassword(newPassword))
            {
                user.Password = newPassword;
                user.MustPasswordChange = false;
                return true;
            }
            return false;
        }

        public bool ValidateUser(User user)
        {
            return user != null && user.IsActive;
        }
    }
}
