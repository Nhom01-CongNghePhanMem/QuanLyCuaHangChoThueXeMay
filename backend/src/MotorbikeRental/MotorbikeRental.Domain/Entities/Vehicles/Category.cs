using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Pricing;

namespace MotorbikeRental.Domain.Entities.Vehicles
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(20, ErrorMessage = "Category name cannot exceed 20 characters")]
        public string CategoryName { get; set; }

        public virtual ICollection<Motorbike> Motorbikes { get; set; }
        public virtual Discount Discount { set; get; }
    }
}