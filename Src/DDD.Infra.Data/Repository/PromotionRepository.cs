using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Specifications;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Infra.Data.Repository
{
    public class PromotionRepository : Repository<Promotion>, IPromotionRepository
    {
        public readonly ApplicationDbContext _context;
        public PromotionRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Book> GetFreeBooks(int index, int pageSize)
        {
            var LinqJoin =from b in _context.Books
                          join bp in _context.PromotionBooks on b.Id equals bp.BookId
                          join p in _context.Promotions on bp.PromotionId equals p.Id
                          where( p.PromotionType == PromotionType.Free && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now
            && !p.IsDeleted && !b.IsDeleted)
                          select b;
            return LinqJoin.Skip(pageSize*index).Take(pageSize).ToList();
        }

        public List<Book> GetFreeBooks()
        {
            var LinqJoin = from b in _context.Books
                           join bp in _context.PromotionBooks on b.Id equals bp.BookId
                           join p in _context.Promotions on bp.PromotionId equals p.Id
                           where (p.PromotionType == PromotionType.Free && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now
             && !p.IsDeleted && !b.IsDeleted)
                           select b;
            return LinqJoin.ToList();
        }
    }
}
