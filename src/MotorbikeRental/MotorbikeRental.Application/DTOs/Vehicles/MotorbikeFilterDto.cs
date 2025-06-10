using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Application.DTOs.Vehicles
{
    public class MotorbikeFilterDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
        public int? CategoryId { get; set; }

        [EnumDataType(typeof(MotorbikeStatus), ErrorMessage = "Invalid motorbike status")]
        public MotorbikeStatus? Status { get; set; }

        [MaxLength(50, ErrorMessage = "Brand name cannot exceed 50 characters")]
        public string? Brand { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;
    }
}