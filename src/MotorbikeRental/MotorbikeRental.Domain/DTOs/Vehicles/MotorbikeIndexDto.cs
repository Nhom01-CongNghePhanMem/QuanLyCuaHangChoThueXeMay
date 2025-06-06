using MotorbikeRental.Domain.DTOs.Pagination;
using MotorbikeRental.Domain.Enums.VehicleEnum;

namespace MotorbikeRental.Domain.DTOs.Vehicles
{
    public class MotorbikeIndexDto
    {
        public IEnumerable<CategoryDto> CategoryViewModels { get; set; }
        public IEnumerable<string> Brands { get; set; }
        public IEnumerable<MotorbikeStatus> motorbikeStatuses { get; set; } = Enum.GetValues<MotorbikeStatus>();
        public PaginatedDataDto<MotorbikeListDto> PaginatedDataViewModel { get; set; }
    }
}