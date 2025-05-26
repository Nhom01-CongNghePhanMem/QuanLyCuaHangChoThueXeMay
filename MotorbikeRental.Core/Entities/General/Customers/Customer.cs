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
        [Required]
        [MaxLength(30)]
        public string FullName { get; set; } //Tên khách hàng
        [Required]
        [MaxLength(20)]
        public string IdNumber { get; set; } //Số CCCD
        [Required]
        [MaxLength(13)]
        public string PhoneNumber { get; set; } //Số điện thoại
        public string? Email { get; set; } //Email
        [Required]
        public DateTime CreateAt { get; set; } //Ngày tạo 
        public virtual ICollection<RentalContract> RentalContracts { get; set; }
    }
}