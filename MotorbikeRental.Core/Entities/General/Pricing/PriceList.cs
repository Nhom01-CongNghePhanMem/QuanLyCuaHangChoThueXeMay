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
        [Required]
        public int MotorbikeId { get; set; } //FK table Motorbike
        public virtual Motorbike Motorbike { get; set; }
        [Required]
        public decimal HourlyRate { get; set; }//Thuê theo giờ
        [Required]
        public decimal DailyRate { get; set; }//Giá theo ngày
    }
}