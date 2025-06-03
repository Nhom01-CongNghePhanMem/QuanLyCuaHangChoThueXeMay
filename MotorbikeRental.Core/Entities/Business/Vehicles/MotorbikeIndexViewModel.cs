using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.Business.Pagination;
using MotorbikeRental.Core.Enums;

namespace MotorbikeRental.Core.Entities.Business.Vehicles
{
    public class MotorbikeIndexViewModel
    {
        public IEnumerable<CategoryViewModel> CategoryViewModels { get; set; }
        public IEnumerable<string> Brands { get; set; }
        public IEnumerable<MotorbikeStatus> motorbikeStatuses { get; set; } = Enum.GetValues<MotorbikeStatus>();
        public PaginatedDataViewModel<MotorbikeListViewModel> PaginatedDataViewModel { get; set; }
    }
}