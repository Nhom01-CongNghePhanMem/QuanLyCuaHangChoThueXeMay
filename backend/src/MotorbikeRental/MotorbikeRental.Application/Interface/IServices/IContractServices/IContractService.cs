using MotorbikeRental.Application.DTOs.ContractDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorbikeRental.Application.Interface.IServices.IContractServices
{
    public interface IContractService
    {
        Task<RentalPriceResponseDto> CalculateRentalPrice(RentalPriceRequestDto priceRequestDto, CancellationToken cancellationToken = default);
    }
}
