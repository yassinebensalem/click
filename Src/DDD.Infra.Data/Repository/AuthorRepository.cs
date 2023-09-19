using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DDD.Infra.Data.Repository
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Author>> GetUsedAuthors()
        {
            var query = from a in _context.Authors
                        join b in _context.Books on a.Id equals b.AuthorId
                        where (b.IsDeleted == false && a.IsDeleted==false)
                        orderby a.FirstName, a.LastName
                        select a;

            return await query.Distinct().ToListAsync();
        }
    }
}
