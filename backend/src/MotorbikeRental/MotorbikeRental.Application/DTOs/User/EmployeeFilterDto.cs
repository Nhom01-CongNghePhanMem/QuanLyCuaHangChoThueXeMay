using MotorbikeRental.Domain.Enums.UserEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.User
{
    public class EmployeeFilterDto
    {
        [MaxLength(50, ErrorMessage = "Cannot exceed 50 characters")]
        public string? Search { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber { get; set; } = 1;
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 12;
        public int? RoleId { get; set; }
        public EmployeeStatus? Status { get; set; } = null;
    }
}
