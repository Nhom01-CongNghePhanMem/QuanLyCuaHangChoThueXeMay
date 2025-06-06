namespace MotorbikeRental.Domain.DTOs.Customers
{
    public class CustomerListDto
    {
        public int CustomerId { get; set; } //Id khách hàng
        public string FullName { get; set; } //Tên khách hàng
        public int RentalCount { get; set; } // Số lần thuê
        public string PhoneNumber { get; set; } //Số điện thoại
        public DateTime CreateAt { get; set; } //Ngày tạo 
    }
}