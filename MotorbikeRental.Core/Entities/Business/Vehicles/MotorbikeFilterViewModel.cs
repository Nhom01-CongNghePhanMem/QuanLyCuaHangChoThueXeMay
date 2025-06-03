using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Enums;

namespace MotorbikeRental.Core.Entities.Business.Vehicles
{
    public class MotorbikeFilterViewModel
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