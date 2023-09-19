using System.Linq;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore; 

namespace DDD.Infra.Data.Repository
{
    public class FavoriteCategoryRepository : Repository<FavoriteCategory>, IFavoriteCategoryRepository
    {
        public FavoriteCategoryRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}
