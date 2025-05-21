using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Enums;

namespace MotorbikeRental.Core.Entities.General
{
    public class Employee
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public DateTime DeteOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Salary { get; set; }
        public int RoleId{ get; set; }
        public Roles Roles { get; set; }
        public EmployeeStatus Status { get; set; }
    }
}