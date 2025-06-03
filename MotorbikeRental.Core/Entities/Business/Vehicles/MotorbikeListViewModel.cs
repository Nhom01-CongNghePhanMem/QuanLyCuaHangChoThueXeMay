using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Enums;

namespace MotorbikeRental.Core.Entities.Business.Vehicles
{
    public class MotorbikeListViewModel
    {
        public int MotorbikeId { get; set; }
        public string MotorbikeName { get; set; }
        public string CategoryName { get; set; }
        public string Brand { get; set; }
        public string? ImageUrl { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal DailyRate { get; set; }
        public MotorbikeStatus Status { get; set; } //Thông tin xe
    }
}