using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Core.Enums
{
    public enum MotorbikeStatus
    {
        Available = 0,        // Xe có sẵn, có thể cho thuê
        Rented = 1,           // Xe đang được thuê
        UnderMaintenance = 2, // Xe đang bảo trì/sửa chữa
        Reserved = 3,         // Xe đã được đặt trước
        OutOfService = 4,     // Xe không còn hoạt động
        Damaged = 5,          // Xe bị hư hỏng
    }
}