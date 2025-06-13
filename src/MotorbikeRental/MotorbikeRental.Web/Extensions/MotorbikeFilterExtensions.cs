using MotorbikeRental.Application.DTOs.Vehicles;

namespace MotorbikeRental.Web.Extensions
{
    public static class MotorbikeFilterExtensions
    {
        public static MotorbikeFilterDto Normalize(this MotorbikeFilterDto? filterDto)
        {
            if (filterDto.PageNumber < 1)
                filterDto.PageNumber = 1;
            if (filterDto.PageSize < 1 || filterDto.PageSize > 100)
                filterDto.PageNumber = 12;
            return filterDto; 
        }
    }
}
