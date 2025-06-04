using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.User;
using MotorbikeRental.Core.Enums.VehicleEnum;

namespace MotorbikeRental.Core.Entities.General.Vehicles
{
    public class MaintenanceRecord
    {
        public int MaintenanceRecordId { get; set; }

        [Required(ErrorMessage = "Motorbike ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid motorbike ID")]
        public int MotorbikeId { get; set; } //FK tới MotorBike Table
        public virtual Motorbike Motorbike { get; set; }

        [Required(ErrorMessage = "Maintenance date is required")]
        public DateTime MaintenanceDate { get; set; }          // Ngày bảo dưỡng

        [Required(ErrorMessage = "Next maintenance date is required")]
        public DateTime? NextMaintenanceDate { get; set; }     // Ngày bảo dưỡng tiếp theo dự kiến

        [Required(ErrorMessage = "Maintenance type is required")]
        public MaintenanceTypeStatus MaintenanceTypeStatus { get; set; }    //Loại bảo dương(Định kì, sửa chưa,....)

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } //Mô tả công việc

        [Required(ErrorMessage = "Cost is required")]
        [Range(0, 999999999, ErrorMessage = "Cost must be positive")]
        public decimal Cost { get; set; } //Chi phí bảo dương(sửa chữa)

        [Required(ErrorMessage = "Employee ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid employee ID")]
        public int EmployeeId { get; set; } // FK Nhân viên bảo dưỡng
        public virtual Employee Employee { get; set; }

        [Required(ErrorMessage = "Created date is required")]
        public DateTime CreatedAt { get; set; } //Ngày tạo bảng ghi
    }
}