using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General;
using MotorbikeRental.Core.Entities.General.Contract;
using MotorbikeRental.Core.Entities.General.User;
using MotorbikeRental.Core.Enums.IncidentEnum;

namespace MotorbikeRental.Core.Entities.General.Incidents
{
    public class Incident //Sự cố
    {
        public int IncidentId { get; set; } //Mã sự cố  

        [Range(1, int.MaxValue, ErrorMessage = "Invalid contract ID")]
        public int? ContractId { get; set; } //Mã hợp đồng
        public virtual RentalContract RentalContract { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid motorbike ID")]
        public int? MotorbikeId { get; set; } //Mã xe
        public virtual Motorbike Motorbike { get; set; }

        [Required(ErrorMessage = "Incident date is required")]
        public DateTime IncidentDate { get; set; } //Ngày bị

        [Required(ErrorMessage = "Incident type is required")]
        [MaxLength(100, ErrorMessage = "Type cannot exceed 100 characters")]
        public string Type { get; set; } //Loại sự cố

        [Required(ErrorMessage = "Severity is required")]
        public SeverityStatus Severity { get; set; } //Mức độ nghiêm trọng

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } //Mô tả

        [Range(0, 999999999, ErrorMessage = "Damage cost must be positive")]
        public decimal? DamageCost { get; set; } //Chi phí sửa chữa

        [Required(ErrorMessage = "Resolution status is required")]
        public bool IsResolved { get; set; } = false; //Đã giải quyết chưa?

        public DateTime? ResolvedDate { get; set; }  //Ngày giải quyết

        [Range(1, int.MaxValue, ErrorMessage = "Invalid employee ID")]
        public int? ReportedByEmployeeId { get; set; } //Nhân viên báo cáo
        public virtual Employee ReportedByEmployee { get; set; }

        [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; } //Ghi chú bổ sung

        [Required(ErrorMessage = "Created date is required")]
        public DateTime CreatedAt { get; set; } //Ngày tạo bản ghi

        // Mối quan hệ với IncidentImage
        public virtual ICollection<IncidentImage> Images { get; set; }
    }
}