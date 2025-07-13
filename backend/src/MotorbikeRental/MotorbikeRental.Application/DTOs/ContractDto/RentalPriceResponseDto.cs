using MotorbikeRental.Domain.Enums.ContractEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.ContractDto
{
    public class RentalPriceResponseDto
    {
        public int MotorbikeId { get; set; }
        public decimal RentalPrice { get; set; }
        public decimal DepositAmount { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public RentalTypeStatus RentalTypeStatus { get; set; }
    }
}
