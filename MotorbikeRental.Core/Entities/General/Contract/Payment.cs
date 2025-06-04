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
        public virtual RentalContract RentalContract { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 999999999, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; } //Tổng tiền

        [Required(ErrorMessage = "Payment date is required")]
        public DateTime PaymentDate { get; set; } //Ngày thanh toán

        [Required(ErrorMessage = "Payment status is required")]
        public PaymentStatus PaymentStatus { get; set; } //Tiền mặt, chuyển khoản?

        [Range(0, 999999999, ErrorMessage = "Contract indemnity must be positive")]
        public decimal? ContractIndemnity { get; set; } //Tiền phạt vi phạm hợp đồng

        [Range(1, int.MaxValue, ErrorMessage = "Invalid discount ID")]
        public int? DiscountId { get; set; }  //Mã giảm giá
        public virtual Discount Discount { get; set; }

        [Range(0, 999999999, ErrorMessage = "Discount amount must be positive")]
        public decimal? DiscountAmount { get; set; } //Số tiền giảm

        [Required(ErrorMessage = "Employee ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid employee ID")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}