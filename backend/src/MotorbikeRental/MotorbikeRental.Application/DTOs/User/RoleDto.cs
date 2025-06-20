using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Application.DTOs.User
{
    public class RoleDto
    {
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters")]
        public string RoleName { get; set; }

        [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
        public string? Description { get; set; }
    }
}