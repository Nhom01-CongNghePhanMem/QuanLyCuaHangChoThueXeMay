using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Domain.Entities.User
{
    public class UserCredentials : IdentityUser<int>
    {
        public int? RoleId { get; set; } //FK Table Roles
        public virtual Roles Roles { get; set; }
        [StringLength(20, MinimumLength = 3)]
        [Required]
        public string UserName { get; set; }            // Tên đăng nhập
        [Required]
        [StringLength(13)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }        // Mật khẩu đã mã hóa
        public DateTime? LastLogin { get; set; }     //Thời gian đăng nhập gần nhất
        public virtual Employee Employee { get; set; }
    }
}