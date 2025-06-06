using System.ComponentModel.DataAnnotations;

namespace MotorbikeRental.Domain.DTOs.Vehicles
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(20, ErrorMessage = "Category name cannot exceed 20 characters")]
        public string CategoryName { get; set; }
    }
}