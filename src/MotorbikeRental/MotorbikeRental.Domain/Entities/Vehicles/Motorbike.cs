using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Entities.Incidents;
using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Domain.Entities.Vehicles
{
    public class Motorbike
    {
        public int MotorbikeId { get; set; }
        [Required(ErrorMessage = "MotorbikeName is required")]
        [MaxLength(50)]
        public string MotorbikeName { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid category ID")]
        public int CategoryId { get; set; } //FK tới Category table
        public virtual Category Category { get; set; }

        [Required(ErrorMessage = "License plate is required")]
        [MaxLength(15, ErrorMessage = "License plate cannot exceed 15 characters")]
        [RegularExpression(@"^[0-9]{2}[A-Z]{1,2}[0-9]{4,5}$", ErrorMessage = "Invalid license plate format")]
        public string LicensePlate { get; set; } // Biển số xe

        [Required(ErrorMessage = "Brand is required")]
        [MaxLength(20, ErrorMessage = "Brand cannot exceed 20 characters")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(1990, 2030, ErrorMessage = "Year must be between 1990 and 2030")]
        public int Year { get; set; } //Năm sản xuất

        [MaxLength(20, ErrorMessage = "Color cannot exceed 20 characters")]
        public string? Color { get; set; }

        [Required(ErrorMessage = "Engine capacity is required")]
        [Range(50, 2000, ErrorMessage = "Engine capacity must be between 50cc and 2000cc")]
        public int EngineCapacity { get; set; } //Dung tích động cơ

        [Required(ErrorMessage = "Chassis number is required")]
        [MaxLength(20, ErrorMessage = "Chassis number cannot exceed 20 characters")]
        public string ChassisNumber { get; set; } //Số khung 

        [Required(ErrorMessage = "Engine number is required")]
        [MaxLength(20, ErrorMessage = "Engine number cannot exceed 20 characters")]
        public string EngineNumber { get; set; } //Số máy

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
        public string? Description { get; set; } // Mô tả

        [Required(ErrorMessage = "Motorbike condition status is required")]
        public MotorbikeConditionStatus MotorbikeConditionStatus { get; set; } //Tình trạng xe(mới, cũ, ...)

        [Url(ErrorMessage = "Invalid URL format")]
        [MaxLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        public string? ImageUrl { get; set; }

        [Range(0, 999999, ErrorMessage = "Mileage must be positive")]
        public decimal? Mileage { get; set; } // Số km đã đi

        [Required(ErrorMessage = "Motorbike status is required")]
        public MotorbikeStatus Status { get; set; } //Thông tin xe

        public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } //Hồ sơ bảo trì
        public virtual ICollection<RentalContract> RentalContracts { get; set; } //Hơp đồng thuê xe với khách
        public virtual ICollection<Incident> Incidents { get; set; }  //Sự cố
        public virtual PriceList PriceList { get; set; } //Bảng giá thuê xe
    }
}