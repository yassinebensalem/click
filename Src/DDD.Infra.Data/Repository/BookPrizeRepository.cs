using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;

namespace DDD.Infra.Data.Repository
{
    public class BookPrizeRepository : Repository<PrizeBook>, IPrizeBookRepository
    {
        public BookPrizeRepository(ApplicationDbContext context)
            : base(context)
        {

        }

    }
}
