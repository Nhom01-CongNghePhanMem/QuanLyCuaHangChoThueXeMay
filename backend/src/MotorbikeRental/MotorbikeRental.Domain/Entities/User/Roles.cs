using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Domain.Entities.User
{
    public class Roles
    {
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters")]
        public string RoleName { get; set; }

        [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
        public string? Description { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}