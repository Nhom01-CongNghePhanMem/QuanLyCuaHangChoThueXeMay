using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Pricing;

namespace MotorbikeRental.Core.Entities.General
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