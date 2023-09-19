using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;

namespace DDD.Infra.Data.Repository
{
    public class PromoUserRepository : Repository<PromoUser>, IPromoUserRepository
    {
        public PromoUserRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}
