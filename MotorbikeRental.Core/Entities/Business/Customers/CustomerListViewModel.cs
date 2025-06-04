using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Core.Entities.Business.Customers
{
    public class CustomerListViewModel
    {
        public int CustomerId { get; set; } //Id khách hàng
        public string FullName { get; set; } //Tên khách hàng
        public int RentalCount { get; set; } // Số lần thuê
        public string PhoneNumber { get; set; } //Số điện thoại
        public DateTime CreateAt { get; set; } //Ngày tạo 
    }
}