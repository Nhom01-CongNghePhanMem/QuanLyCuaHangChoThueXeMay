using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Contract;

namespace MotorbikeRental.Core.Entities.General.Customers
{
    public class Customer
    {
        public int CustomerId { get; set; } //Id khách hàng
        
        [Required(ErrorMessage = "Full name is required")]
        [MaxLength(30, ErrorMessage = "Full name cannot exceed 30 characters")]
        public string FullName { get; set; } //Tên khách hàng
        
        [Required(ErrorMessage = "ID number is required")]
        [MaxLength(20, ErrorMessage = "ID number cannot exceed 20 characters")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "ID number must be 12 digits")]
        public string IdNumber { get; set; } //Số CCCD
        
        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(13, ErrorMessage = "Phone number cannot exceed 13 characters")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } //Số điện thoại
        
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        public string? Email { get; set; } //Email
        
        [Required(ErrorMessage = "Create date is required")]
        public DateTime CreateAt { get; set; } //Ngày tạo 
        
        public virtual ICollection<RentalContract> RentalContracts { get; set; }
    }
}