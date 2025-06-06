using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Customers;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Entities.User;
using MotorbikeRental.Domain.Entities.Vehicles;
using MotorbikeRental.Domain.Enums.ContractEnum;

namespace MotorbikeRental.Domain.Entities.Contract
{
    public class RentalContract
    {
        [Required(ErrorMessage = "Contract ID is required")]
        public int ContractId { get; set; } // Mã hợp đồng

        [Range(1, int.MaxValue, ErrorMessage = "Invalid customer ID")]
        public int? CustomerId { get; set; } //Mã khách hàng
        public virtual Customer Customer { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid motorbike ID")]
        public int? MotorbikeId { get; set; } //Mã xe
        public virtual Motorbike Motorbike { get; set; }

        [Required(ErrorMessage = "Deposit amount is required")]
        [Range(0, 999999999, ErrorMessage = "Deposit amount must be positive")]
        public decimal DepositAmount { get; set; } // Tiền cọc

        [Required(ErrorMessage = "Employee ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid employee ID")]
        public int EmployeeId { get; set; } // Nhân viên làm hợp đồng

        public virtual Employee Employee { get; set; }

        [Required(ErrorMessage = "Rental date is required")]
        public DateTime RentalDate { get; set; } //Ngày thuê

        [Required(ErrorMessage = "Expected return date is required")]
        public DateTime ExpectedReturnDate { get; set; } //Ngày, giờ dự kiến trả

        [Required(ErrorMessage = "Total amount is required")]
        [Range(0.01, 999999999, ErrorMessage = "Total amount must be greater than 0")]
        public decimal TotalAmount { get; set; } //Tổng tiền thuê

        public DateTime? ActualReturnDate { get; set; } //Ngày thực tế trả

        [Range(0, 999999999, ErrorMessage = "Late return fee must be positive")]
        public decimal? LateReturnFee { get; set; } //Phí trả muộn

        [Range(0.1, 10.0, ErrorMessage = "Late return fee multiplier must be between 0.1 and 10")]
        public decimal? LateReturnFeeMultiplier { get; set; } = 2.0m;

        [Required(ErrorMessage = "Rental contract status is required")]
        public RentalContractStatus RentalContractStatus { get; set; } //Trạng thái (Đang thuê, đã trả)     

        [Required(ErrorMessage = "Rental type status is required")]
        public RentalTypeStatus RentalTypeStatus { get; set; } //Thuê theo giờ - ngày

        [Required(ErrorMessage = "ID card held status is required")]
        public bool IdCardHeld { get; set; } // Đã giữ cccd?

        [MaxLength(100, ErrorMessage = "Note cannot exceed 100 characters")]
        public string? Note { get; set; } //Ghi chú

        public virtual Incident Incident { get; set; }
        public virtual Payment Payments { get; set; } //Danh sách thanh toán
    }
}