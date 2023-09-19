using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;

namespace DDD.Infra.Data.Repository
{
    public class LibraryRepository : Repository<Library>, ILibraryRepository
    {
        public LibraryRepository(ApplicationDbContext context)
            : base(context)
        {

        }
        
    }
}
