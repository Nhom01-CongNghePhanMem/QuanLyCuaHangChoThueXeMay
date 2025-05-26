using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotorbikeRental.Core.Entities.General.Contract;
using MotorbikeRental.Core.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Infrastructure.Data;

namespace MotorbikeRental.Infrastructure.Repositories.ContractRepositories
{
    public class RentalContractRepository : BaseRepository<RentalContract> , IRentalContractRepository
    {
        public RentalContractRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext){}
    }
}