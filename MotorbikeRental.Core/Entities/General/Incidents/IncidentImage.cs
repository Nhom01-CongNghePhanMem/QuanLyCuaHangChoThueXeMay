using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Core.Entities.General.Incidents
{
    public class IncidentImage
    {
        public int ImageId { get; set; }
        [Required]
        public int IncidentId { get; set; } //Mã sự cố
        public Incident Incident { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}