using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Customers;
using MotorbikeRental.Core.Entities.General.User;
using MotorbikeRental.Core.Entities.Incidents;
using MotorbikeRental.Core.Enums.ContractEnum;

namespace MotorbikeRental.Core.Entities.General.Contract
{
    public class RentalContract
    {
        [Required]
        public int ContractId { get; set; } // Mã hợp đồng
        public int? CustomerId { get; set; } //Mã khách hàng
        public virtual Customer Customer { get; set; }
        public int? MotorbikeId { get; set; } //Mã xe
        public Motorbike Motorbike { get; set; }
        [Required]
        public decimal DepositAmount { get; set; } // Tiền cọc
        [Required]
        public int EmployeeId { get; set; } // Nhân viên làm hợp đồng
        [Required]
        public Employee Employee{ get; set; }
        public DateTime RentalDate { get; set; } //Ngày thuê
        [Required]
        public DateTime ExpectedReturnDate { get; set; } //Ngày, giờ dự kiến trả
        [Required]
        public decimal TotalAmount { get; set; } //Tổng tiền thuê
        public DateTime? ActualReturnDate { get; set; } //Ngày thực tế trả
        public decimal? LateReturnFee { get; set; } //Phí trả muộn
        public decimal? LateReturnFeeMultiplier { get; set; } = 2.0m;
        [Required]
        public RentalContractStatus RentalContractStatus { get; set; } //Trạng thái (Đang thuê, đã trả)     
        [Required]
        public RentalTypeStatus RentalTypeStatus { get; set; } //Thuê theo giờ - ngày
        [Required]
        public bool IdCardHeld { get; set; } // Đã giữ cccd?
        [MaxLength(100)]
        public string? Note { get; set; } //Ghi chú
        public virtual Incident Incident { get; set; }
        public virtual Payment Payments { get; set; } //Danh sách thanh toán
    }
}