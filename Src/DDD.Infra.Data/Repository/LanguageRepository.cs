using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace DDD.Infra.Data.Repository
{
 public   class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public readonly ApplicationDbContext _context;
        public LanguageRepository(ApplicationDbContext context)
         : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

        }

        public async Task<IEnumerable<Language>> GetUsedLanguages()
        {
            var query = from l in _context.Languages
                        join b in _context.Books on l.Id equals b._LanguageId
                        where (b.IsDeleted==false)
                        orderby l.Name
                        select l;

           return await query.Distinct().ToListAsync();
        }
    }
}
