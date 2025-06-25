using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Domain.Entities.User
{
    public class UserCredentials : IdentityUser<int>
    {
        public int? RoleId { get; set; } //FK Table Roles
        public virtual Roles Roles { get; set; }
        public DateTime? LastLogin { get; set; }     //Thời gian đăng nhập gần nhất
        public virtual Employee Employee { get; set; }
    }
}