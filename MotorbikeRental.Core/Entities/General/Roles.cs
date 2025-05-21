using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorbikeRental.Core.Entities.General
{
    public class Roles
    {
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}