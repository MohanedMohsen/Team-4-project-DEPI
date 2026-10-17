using EducationalManagementSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationalManagementSystem.Models
{
    public abstract class User
    {
        public int Id { get; set; }

        public string UserID { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public bool MustPasswordChange { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public abstract UserRole Role { get; }
    }
}
