using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Contract;
using MotorbikeRental.Core.Entities.General.Pricing;
using MotorbikeRental.Core.Entities.General.Vehicles;
using MotorbikeRental.Core.Entities.Incidents;
using MotorbikeRental.Core.Enums;
using MotorbikeRental.Core.Enums.VehicleEnum;

namespace MotorbikeRental.Core.Entities.General
{
    public class Motorbike
    {
        public int MotorbikeId { get; set; }
        [Required]
        public int CategoryId { get; set; } //FK tới Category table
        public Category Category { get; set; }
        [Required]
        [MaxLength(15)]
        public string LicensePlate { get; set; } // Biển số xe
        [Required]
        [MaxLength(20)]
        public string Brand { get; set; }
        [Required]
        public int Year { get; set; } //Năm sản xuất
        public string? Color { get; set; }
        [Required]
        public int EngineCapacity { get; set; } //Dung tích động cơ
        [Required]
        public string ChassisNumber { get; set; } //Số khung 
        [Required]
        public string EngineNumber { get; set; } //Số máy
        [MaxLength(100)]
        public string? Description { get; set; } // Mô tả
        [Required]
        public MotorbikeConditionStatus MotorbikeConditionStatus { get; set; } //Tình trạng xe(mới, cũ, ...)
        public string? ImageUrl { get; set; }
        public decimal? Mileage { get; set; } // Số km đã đi
        [Required]
        public MotorbikeStatus Status { get; set; } //Thông tin xe
        public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } //Hồ sơ bảo trì
        public virtual ICollection<RentalContract> RentalContracts { get; set; } //Hơp đồng thuê xe với khách
        public virtual ICollection<Incident> Incidents { get; set; }  //Sự cố
        public virtual PriceList PriceList { get; set; }
    }
}