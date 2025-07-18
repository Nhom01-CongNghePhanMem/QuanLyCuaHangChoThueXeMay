using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.Incident
{
    public class IncidentListDto
    {
        public int IncidentId { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime CreateAt { get; set; }
        public string Type { get; set; }
        public decimal? DamageCost { get; set; }
        public IEnumerable<string>? Images { get; set; }
    }
}