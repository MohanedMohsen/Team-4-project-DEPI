using System;
using EducationalManagementSystem.Interfaces;
using EducationalManagementSystem.Repositories;

class Program
{
    static void Main(string[] args)
    {
        IAuthService auth = new AuthService();

        while (true)
        {
            Console.WriteLine("\n=== Educational Management System ===");
            Console.WriteLine("1. Sign Up");
            Console.WriteLine("2. Login");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? string.Empty;

            if (choice == "0")
            {
                Console.WriteLine("Goodbye.");
                break;
            }

            if (choice == "1")
            {
                Console.Write("Enter full name: ");
                string fullName = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter email: ");
                string email = Console.ReadLine() ?? string.Empty;

                var result = auth.SignUp(fullName, email);
                if (result.UserId != null)
                {
                    Console.WriteLine($"User ID: {result.UserId}");
                    Console.WriteLine($"Temporary Password: {result.tempPassword}");
                }
                continue;
            }

            if (choice == "2")
            {
                Console.Write("Enter User ID: ");
                string userId = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter Password: ");
                string password = Console.ReadLine() ?? string.Empty;

                var loggedUser = auth.Login(userId, password);
                if (loggedUser != null)
                {
                    Console.WriteLine("Login completed successfully.");
                }
                continue;
            }

            Console.WriteLine("Invalid option.");
        }
    }
}