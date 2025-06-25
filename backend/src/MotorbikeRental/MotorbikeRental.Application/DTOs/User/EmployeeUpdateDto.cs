using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MotorbikeRental.Domain.Enums.UserEnum;

namespace MotorbikeRental.Application.DTOs.User
{
    public class EmployeeUpdateDto
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(30, ErrorMessage = "Full name cannot exceed 30 characters")]
        public string FullName { get; set; }
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be 6–50 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(13, ErrorMessage = "Phone number cannot exceed 13 characters")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Range(0, 999999999, ErrorMessage = "Salary must be positive")]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = "Role ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid role ID")]
        public int RoleId { get; set; } //FK Table Roles

        [Required(ErrorMessage = "Employee status is required")]
        public EmployeeStatus Status { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}