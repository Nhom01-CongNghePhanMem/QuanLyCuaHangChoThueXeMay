using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Domain.Entities.User
{
    public class UserCredentials
    {
        public int CredentialId { get; set; }
        [Required(ErrorMessage = "Employee ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid EmployeeId")]
        public int EmployeeId { get; set; }             // Foreign Key → Employee
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }            // Tên đăng nhập
        [Required(ErrorMessage = "Password hash is required")]
        public string PasswordHash { get; set; }        // Mật khẩu đã mã hóa
        public DateTime? LastLogin { get; set; }     //Thời gian đăng nhập gần nhất
        public virtual Employee Employee { get; set; }
    }
}