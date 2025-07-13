using MotorbikeRental.Domain.Enums.ContractEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.DTOs.ContractDto
{
    public class RentalPriceRequestDto
    {
        [Required(ErrorMessage = "Motorbike ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Motorbike ID must be a positive integer")]
        public int MotorbikeId { get; set; }
        [Required(ErrorMessage = "Rental date is required")]
        public DateTime RentalDate { get; set; }
        [Required(ErrorMessage = "Expected return date is required")]
        public DateTime ExpectedReturnDate { get; set; }
        [Required(ErrorMessage = "Rental type status is required")]
        public RentalTypeStatus RentalTypeStatus { get; set; }
    }
}
