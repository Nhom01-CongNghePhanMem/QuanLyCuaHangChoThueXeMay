using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Core.Entities.General.User
{
    public class UserCredentials
    {
        public int CredentialId { get; set; }
        [Required(ErrorMessage = "Employee ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid EmployeeId")]
        public int EmployeeId { get; set; }             // Foreign Key → Employee
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
        public string Username { get; set; }            // Tên đăng nhập
        [Required(ErrorMessage = "Password hash is required")]
        public string PasswordHash { get; set; }        // Mật khẩu đã mã hóa
        [Required(ErrorMessage = "Salt is required")]
        public string Salt { get; set; }                // Chuỗi salt
        public DateTime? LastLogin { get; set; }     //Thời gian đăng nhập gần nhất
        public virtual Employee Employee { get; set; }
    }
}