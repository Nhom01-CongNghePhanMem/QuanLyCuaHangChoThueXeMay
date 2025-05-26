using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Contract;
using MotorbikeRental.Core.Entities.General.Vehicles;
using MotorbikeRental.Core.Entities.General.Incidents;
using MotorbikeRental.Core.Enums;

namespace MotorbikeRental.Core.Entities.General.User
{
    public class Employee
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }
        [Required]
        [StringLength(30)]
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Avatar { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public decimal? Salary { get; set; }
        [Required]
        public int RoleId { get; set; } //FK Table Roles
        public virtual Roles Roles { get; set; }
        [Required]
        public EmployeeStatus Status { get; set; }
        public virtual ICollection<MaintenanceRecord>? MaintenanceRecords { get; set; }
        public virtual ICollection<RentalContract> RentalContracts { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
    }
}