using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Core.Entities.General.Pricing
{
    public class PriceList
    {
        public int PriceListId { get; set; }

        [Required(ErrorMessage = "Motorbike ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid motorbike ID")]
        public int MotorbikeId { get; set; } //FK table Motorbike
        public virtual Motorbike Motorbike { get; set; }

        [Required(ErrorMessage = "Hourly rate is required")]
        [Range(0.01, 9999999, ErrorMessage = "Hourly rate must be greater than 0")]
        public decimal HourlyRate { get; set; }//Thuê theo giờ

        [Required(ErrorMessage = "Daily rate is required")]
        [Range(0.01, 9999999, ErrorMessage = "Daily rate must be greater than 0")]
        public decimal DailyRate { get; set; }//Giá theo ngày
    }
}