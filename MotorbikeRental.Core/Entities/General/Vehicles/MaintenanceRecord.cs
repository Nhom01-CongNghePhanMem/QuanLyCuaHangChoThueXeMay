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
        [Required]
        public int MotorbikeId { get; set; } //FK tới MotorBike Table
        public virtual Motorbike Motorbike { get; set; }
        [Required]
        public DateTime MaintenanceDate { get; set; }          // Ngày bảo dưỡng
        [Required]
        public DateTime? NextMaintenanceDate { get; set; }     // Ngày bảo dưỡng tiếp theo dự kiến
        public MaintenanceTypeStatus MaintenanceTypeStatus { get; set; }    //Loại bảo dương(Định kì, sửa chưa,....)
        public string Description { get; set; } //Mô tả công việc
        [Required]
        public decimal Cost { get; set; } //Chi phí bảo dương(sửa chữa)
        [Required]
        public int EmployeeId { get; set; } // FK Nhân viên bảo dưỡng
        public virtual Employee Employee { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } //Ngày tạo bảng ghi
    }
}