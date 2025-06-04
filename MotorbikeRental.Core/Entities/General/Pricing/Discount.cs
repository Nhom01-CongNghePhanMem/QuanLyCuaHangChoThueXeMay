using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Core.Entities.General.Pricing
{
    public class Discount //Giảm giá
    {
        public int DiscountId { get; set; }

        [Required(ErrorMessage = "Discount name is required")]
        [MaxLength(30, ErrorMessage = "Name cannot exceed 30 characters")]
        public string Name { get; set; } //Tên chương trình

        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; } //Nôi dung

        [Required(ErrorMessage = "Discount value is required")]
        [Range(1, 100, ErrorMessage = "Discount value must be between 1 and 100 percent")]
        public int Value { get; set; } //Giảm theo %

        [Required(ErrorMessage = "Category ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid category ID")]
        public int CategoryId { get; set; } //FK Loại xe
        public virtual Category Category { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; } // Ngày bắt đầu

        public DateTime? EndDate { get; set; } // Ngày kết thúc (null = không giới hạn)

        [Required(ErrorMessage = "Active status is required")]
        public bool IsActive { get; set; } = true; // Đang áp dụng hay không

        [Required(ErrorMessage = "Created date is required")]
        public DateTime CreatedAt { get; set; } //Ngày tạo
    }
}