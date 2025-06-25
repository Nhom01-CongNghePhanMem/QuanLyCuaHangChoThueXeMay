using System.ComponentModel.DataAnnotations;
using MotorbikeRental.Domain.Entities.Pricing;

namespace MotorbikeRental.Domain.Entities.Vehicles
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(20)]
        public string CategoryName { get; set; }

        public virtual ICollection<Motorbike> Motorbikes { get; set; }
        public virtual Discount Discount { set; get; }
    }
}