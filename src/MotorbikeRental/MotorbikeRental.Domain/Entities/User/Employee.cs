using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.UserEnum;

namespace MotorbikeRental.Domain.Entities.User
{
    public class Employee
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Username cannot exceed 20 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(30, ErrorMessage = "Full name cannot exceed 30 characters")]
        public string FullName { get; set; }

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
        [Required(ErrorMessage = "Credential Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid CredentialId")]
        public int CredentialId { get; set; }
        public virtual UserCredentials UserCredentials { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500, ErrorMessage = "Avatar URL cannot exceed 500 characters")]
        public string? Avatar { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Range(0, 999999999, ErrorMessage = "Salary must be positive")]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = "Role ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid role ID")]
        public int RoleId { get; set; } //FK Table Roles
        public virtual Roles Roles { get; set; }

        [Required(ErrorMessage = "Employee status is required")]
        public EmployeeStatus Status { get; set; }

        public virtual ICollection<MaintenanceRecord>? MaintenanceRecords { get; set; }
        public virtual ICollection<RentalContract> RentalContracts { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
    }
}