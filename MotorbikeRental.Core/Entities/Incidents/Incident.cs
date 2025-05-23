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

namespace MotorbikeRental.Core.Entities.Incidents
{
    public class Incident //Sự cố
    {
        public int IncidentId { get; set; } //Mã sự cố  
        public int? ContractId { get; set; } //Mã hợp đồng
        public virtual RentalContract RentalContract { get; set; }
        public int? MotorbikeId { get; set; } //Mã xe
        public virtual Motorbike Motorbike { get; set; }
        public DateTime IncidentDate { get; set; } //Ngày bị
        [Required]
        [MaxLength(100)]
        public string Type { get; set; } //Loại sự cố
        [Required]
        [MaxLength(20)]
        public SeverityStatus Severity { get; set; } //Mức độ nghiêm trọng
        [Required]
        [MaxLength(500)]
        public string Description { get; set; } //Mô tả
        public decimal? DamageCost { get; set; } //Chi phí sửa chữa
        
        public bool IsResolved { get; set; } = false; //Đã giải quyết chưa?
        
        public DateTime? ResolvedDate { get; set; }  //Ngày giải quyết
        
        public int? ReportedByEmployeeId { get; set; } //Nhân viên báo cáo
        public virtual Employee ReportedByEmployee { get; set; }
        
        public string? Notes { get; set; } //Ghi chú bổ sung
        
        public DateTime CreatedAt { get; set; } //Ngày tạo bản ghi
        
        // Mối quan hệ với IncidentImage
        public virtual ICollection<IncidentImage> Images { get; set; }
    }
}