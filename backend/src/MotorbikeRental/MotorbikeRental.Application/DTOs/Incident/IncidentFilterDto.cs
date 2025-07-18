using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.Incident
{
    public class IncidentFilterDto
    {
        private int pageNumber = 1;
        private int pageSize = 12;
        public int PageNumber
        {
            get => pageNumber;
            set
            {
                if (value >= 1) pageNumber = value;
            }
        }
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (value >= 1 && value <= 100) pageSize = value;
            }
        }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsResolved { get; set; }
    }
}