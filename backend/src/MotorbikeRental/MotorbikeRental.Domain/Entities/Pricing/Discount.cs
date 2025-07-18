using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Vehicles;

namespace MotorbikeRental.Domain.Entities.Pricing
{
    public class Discount //Giảm giá
    {
        public int DiscountId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } //Tên chương trình

        [MaxLength(200)]
        public string? Description { get; set; } //Nôi dung

        [Required]
        [Range(1, 100)]
        public int Value { get; set; } //Giảm theo %

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; } //FK Loại xe
        public virtual Category Category { get; set; }

        [Required]
        public DateTime StartDate { get; set; } // Ngày bắt đầu

        public DateTime? EndDate { get; set; } // Ngày kết thúc (null = không giới hạn)

        [Required]
        public bool IsActive { get; set; } = true; // Đang áp dụng hay không

        [Required]
        public DateTime CreatedAt { get; set; } //Ngày tạo
    }
}