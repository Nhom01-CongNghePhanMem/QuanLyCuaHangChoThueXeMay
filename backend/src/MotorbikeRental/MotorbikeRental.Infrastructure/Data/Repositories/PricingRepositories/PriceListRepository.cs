using MotorbikeRental.Domain.Entities.Pricing;
using MotorbikeRental.Domain.Interfaces.IRepositories.IPricingRepositories;
using MotorbikeRental.Infrastructure.Data.Contexts;

namespace MotorbikeRental.Infrastructure.Data.Repositories.PricingRepositories
{
    public class PriceListRepository : BaseRepository<PriceList>, IPriceListRepository
    {
        public PriceListRepository(MotorbikeRentalDbContext motorbikeRentalDbContext) : base(motorbikeRentalDbContext) { }
    }
}