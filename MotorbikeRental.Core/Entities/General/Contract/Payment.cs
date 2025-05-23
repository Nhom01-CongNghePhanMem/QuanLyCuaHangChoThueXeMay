using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Pricing;
using MotorbikeRental.Core.Entities.General.User;
using MotorbikeRental.Core.Enums.ContractEnum;

namespace MotorbikeRental.Core.Entities.General.Contract
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int? ContractId { get; set; } //FK Hợp đồng
        public RentalContract RentalContract { get; set; }
        [Required]
        public decimal Amount { get; set; } //Tổng tiền
        [Required]
        public DateTime PaymentDate { get; set; } //Ngày thanh toán
        [Required]
        public PaymentStatus PaymentStatus { get; set; } //Tiền mặt, chuyển khoản?
        public decimal? ContractIndemnity { get; set; } //Tiền phạt vi phạm hợp đồng
        public int? DiscountId { get; set; }  //Mã giảm giá
        public virtual Discount Discount { get; set; }
        public decimal? DiscountAmount { get; set; } //Số tiền giảm
        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}