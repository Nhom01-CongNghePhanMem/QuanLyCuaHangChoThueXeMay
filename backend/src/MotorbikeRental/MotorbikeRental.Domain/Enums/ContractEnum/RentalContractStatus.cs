namespace MotorbikeRental.Domain.Enums.ContractEnum
{
    public enum RentalContractStatus
    {
        Active = 0,      // Đang thuê
        Completed = 1,   // Đã hoàn thành
        Cancelled = 2,   // Đã hủy
        Overdue = 3      // Quá hạn
    }
}