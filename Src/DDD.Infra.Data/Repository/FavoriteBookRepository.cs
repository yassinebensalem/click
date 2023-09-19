using System.Linq;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore; 

namespace DDD.Infra.Data.Repository
{
    public class FavoriteBookRepository : Repository<FavoriteBook>, IFavoriteBookRepository
    {
        public FavoriteBookRepository(ApplicationDbContext context)
            : base(context)
        {

        } 
    }
}
