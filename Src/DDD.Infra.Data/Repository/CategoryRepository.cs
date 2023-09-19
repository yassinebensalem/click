using System.Linq;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
namespace DDD.Infra.Data.Repository
{
  public  class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context)
            :  base(context)
         {

        }
    }
}
