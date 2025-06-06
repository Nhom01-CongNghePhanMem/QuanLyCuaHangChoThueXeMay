using MotorbikeRental.Domain.Entities.Contract;
using MotorbikeRental.Domain.Interfaces.IRepositories.IContractRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.ContractRepositories
{
    public class RentalContractRepository : BaseRepository<RentalContract>, IRentalContractRepository
    {
        public RentalContractRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
    }
}