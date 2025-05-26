using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Core.Entities.Business.Pagination
{
    public class PaginatedDataViewModel<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
        public PaginatedDataViewModel(IEnumerable<T> data, int totalCount)
        {
            this.Data = data;
            this.TotalCount = totalCount;
        }
    }
}