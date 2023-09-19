using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Specifications;
using DDD.Domain.Common.Constants;
using DDD.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static DDD.Domain.Common.Constants.State;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using DDD.Domain.Common.Pagination;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DDD.Infra.Data.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<IEnumerable<Book>> GetPagedBooks(PagedBooks booksFilter, int pageIndex, int pageSize)
        {
            var query = GetFilteredBooks(booksFilter);


            // Si la size est pas superieur a 0 pas la peine de faire une nouvelle query a la base de donné.
            return (pageSize > 0) ?
                 await query.Skip(pageSize * pageIndex).Take(pageSize).ToArrayAsync().ConfigureAwait(false)
                :
                 await query.ToArrayAsync().ConfigureAwait(false);
        }
        public virtual IQueryable<Book> GetAllBookPublished(ISpecification<Book> spec)
        {
            return base.GetAll(spec).Where(b => b.Status == Domain.Common.Constants.State.BookState.published);
        }


        #region Filtrage & Tri Pagination
        private IQueryable<Book> GetFilteredBooks(PagedBooks filters)
        {
            var query = GetBooksWithAllChildEntities(filters);
            query = FilterByEditor(filters, query);
            //query = FilterByUserIds(filters, query);
            query = FilterByCategory(filters, query);
            query = FilterByAuthor(filters, query);
            query = FilterByLanguage(filters, query);
            query = FilterByPromotion(filters, query);

            query = FilterBySearchKey(filters, query);
            query = OrderBy(filters, query);

            return query;
        }
        private static IQueryable<Book> FilterByEditor(PagedBooks filters, IQueryable<Book> query)
            => filters.EditorId?.Any() == true
                ? query.Where(b => b.PublisherId.Equals(filters.EditorId))
                : query;
        private static IQueryable<Book> FilterByCategory(PagedBooks filters, IQueryable<Book> query)
        {
            return filters.Categories?.Any() == true
                ? query.Where(b => filters.Categories.Contains(b.CategoryId.ToString()))
                : query;
        }
        private static IQueryable<Book> FilterByAuthor(PagedBooks filters, IQueryable<Book> query)
        {
            return filters.Authors?.Any() == true
                ? query.Where(b => filters.Authors.Contains(b.AuthorId.ToString()))
                : query;
        }
        private static IQueryable<Book> FilterByLanguage(PagedBooks filters, IQueryable<Book> query)
        {
            return filters.Languages?.Any() == true
                ? query.Where(b => filters.Languages.Contains(b._LanguageId))
                : query;
        }
        private IQueryable<Book> FilterByPromotion(PagedBooks filters, IQueryable<Book> query)
        {
            return filters.IsPromotedBook == true
                ? query
                 .Join(_context.PromotionBooks, b => b.Id, bp => bp.BookId, (b, bp) => new { b, bp })
                 .Join(_context.Promotions, r => r.bp.PromotionId, p => p.Id, (r, p) => new { r.b, p })
                 .Where(rp => rp.p.PromotionType == PromotionType.Free && rp.p.StartDate <= DateTime.Now && rp.p.EndDate >= DateTime.Now
             && !rp.p.IsDeleted && !rp.b.IsDeleted)
                 .Select(rp => rp.b)
                : query;

        }
        private static IQueryable<Book> OrderBy(PagedBooks filters, IQueryable<Book> query)
        {
            var orderedQuery = query.OrderByDescending(b => b.PublicationDate);

            return orderedQuery;
        }
        private static IQueryable<Book> FilterBySearchKey(PagedBooks filters, IQueryable<Book> query)
        {
            return (!string.IsNullOrEmpty(filters.SearchKeyText))
                ? query.Where(b => (filters.SearchKeyType == GlobalConstant.SearchEnum.Author) ? (b.Author.FirstName.Trim().ToUpper().Contains(filters.SearchKeyText.ToUpper())
            || b.Author.LastName.Trim().ToUpper().Contains(filters.SearchKeyText.ToUpper())
            || string.Concat(string.Concat(b.Author.FirstName.Trim().ToUpper(), " "), b.Author.LastName.Trim().ToUpper()).Contains(filters.SearchKeyText.ToUpper()))
                : (filters.SearchKeyType == GlobalConstant.SearchEnum.Category) ? b.Category.CategoryName.ToUpper().Contains(filters.SearchKeyText.ToUpper())
                 : (filters.SearchKeyType == GlobalConstant.SearchEnum.Editor) ? b.Publisher.RaisonSocial.ToUpper().Contains(filters.SearchKeyText.ToUpper())
                 : b.Title.ToUpper().Contains(filters.SearchKeyText.ToUpper()))
            : query;

        }
        private IQueryable<Book> GetBooksWithAllChildEntities(PagedBooks filters) =>
           _context.Books.Where(b => !b.Author.IsDeleted
            && b.Status == State.BookState.published
            && !b.IsDeleted)
        .Include(c => c.Author)
        .Include(c => c.Category)
        .Include(b => b.Publisher);
        public List<Book> GetFreeBooks(int index, int pageSize)
        {
            var LinqJoin = from b in _context.Books
                           join bp in _context.PromotionBooks on b.Id equals bp.BookId
                           join p in _context.Promotions on bp.PromotionId equals p.Id
                           where (p.PromotionType == PromotionType.Free && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now
             && !p.IsDeleted && !b.IsDeleted)
                           select b;
            return LinqJoin.Skip(pageSize * index).Take(pageSize).ToList();
        }

        public async Task<IEnumerable<Book>> SearchBooksByTitle(string searchKey)
        {
            return await _context.Books.Where(b => !b.Author.IsDeleted
             && b.Status == State.BookState.published
             && !b.IsDeleted && b.Title.StartsWith(searchKey)).
         Include(c => c.Author).Select(b => new Book(b.Id, b.Title)
         ).ToListAsync();
        }

        #endregion Filtrage & Tri

        #region Filtrage & Tri Dashboard
        //published
        public async Task<long> GetPublishedBooksCount(PagedBooks booksFilter)
        {
            var count = _context.Books.Where(b => !b.Author.IsDeleted
            && b.Status == State.BookState.published
            && !b.IsDeleted).Count();
            return count;
        }

        public async Task<long> GetSelledBooksCount(PagedBooks booksFilter)
        {
            var count = _context.Invoices.Where(i => !i.IsDeleted).Count();
            return count;
        }
        public async Task<long> GetTotalSelledBooksCountByFilter(DashboardFilter filters)
        {
            return (from b in _context.Books
                    join inv in _context.Invoices on b.Id equals inv.BookId
                    where (!b.IsDeleted
                    && (filters.BookId != null ? inv.BookId == new Guid(filters.BookId) : true)
                    && (filters.EditorId != null ? b.PublisherId == filters.EditorId : true)
                    && (filters.AuthorId != null ? b.AuthorId == new Guid(filters.AuthorId) : true)
                    && (filters.LanguageId != null ? b._LanguageId == filters.LanguageId : true)
                    && (filters.FromDate != null ? inv.Date >= filters.FromDate : true)
                    && (filters.ToDate != null ? inv.Date <= filters.ToDate : true)
                    && (filters.Date != null ? inv.Date == filters.Date : true)
                    )
                    select inv).Count();
        }
        public async Task<List<SelledBooksChart>> GetDaySelledBooksByFilter(DashboardFilter filters)
        {
            return await _context.Books
                .Include(b => b.Invoices.Where(inv => (filters.BookId != null ? inv.BookId == new Guid(filters.BookId) : true)
                 && (filters.FromDate != null ? inv.Date.Date >= filters.FromDate : true)
                        && (filters.ToDate != null ? inv.Date.Date <= filters.ToDate : true)
                        && (filters.Date != null ? inv.Date == filters.Date : true))
                )
                .Include(a => a.Author)
                .Where(b =>
                    (filters.EditorId != null ? b.PublisherId == filters.EditorId : true)
                    && (filters.AuthorId != null ? b.AuthorId == new Guid(filters.AuthorId) : true)
                    && (filters.LanguageId != null ? b._LanguageId == filters.LanguageId : true)
                )
                .SelectMany(b=>b.Invoices)
                 .GroupBy(inv=>inv.Date.Date)

                .Select(b => new SelledBooksChart
                {
                    Date=b.Key,
                    Count = b.Count(),
                   Value= b.Sum(s => s.Price)
                })
                .Where(inv=>
                  (filters.FromDate != null ? inv.Date.Date >= filters.FromDate : true)
                        && (filters.ToDate != null ? inv.Date.Date <= filters.ToDate : true)
                        && (filters.Date != null ? inv.Date == filters.Date : true)
                )
                 .OrderBy(inv => inv.Date.Date)
                .ToListAsync();
        }

        public async Task<PagedItems<PagedSelledBooks>> GetTopSelledBooks(int index, int size)
        {
            var query=  _context.Books
                 .Include(b => b.Invoices)
                 .Include(a => a.Author)
                 .Where(b => b.Invoices.Count() > 0)
                 .OrderByDescending(b => b.Invoices.Count())
                 .Select(b => new PagedSelledBooks
                 {
                     Title = b.Title,
                     BookId = b.Id.ToString(),
                     AuthorName = b.Author.FirstName + " " + b.Author.LastName,
                     Count = b.Invoices.Count()

                 });

            var totalItems = await query.CountAsync().ConfigureAwait(false);

            // Si la size est pas superieur a 0 pas la peine de faire une nouvelle query a la base de donné.
            return (size > 0) ?
                new(items: await query.Skip(size * (index)).Take(size).ToListAsync().ConfigureAwait(false),
                    totalItems: totalItems,
                    currentPage: index,
                    pageSize: size)
                :
                new(items: new List<PagedSelledBooks>(),
                    totalItems: totalItems,
                    currentPage: index,
                    pageSize: size);
        }
        public async Task<PagedItems<PagedBestPublishers>> GetTopPublishers(int index, int size)
        {
            var query=  _context.Users
                 .Include(b => b.Books)
                 .ThenInclude(a => a.Invoices)

                 .Where(u => u.Books.Any(b => b.Invoices.Count() > 0))

                .Select(b => new PagedBestPublishers
                {
                    PublisherId = b.Id.ToString(),
                    RaisonSociale = b.RaisonSocial,
                    Count = b.Books.SelectMany(b => b.Invoices).Count(),
                })
                .OrderByDescending(b => b.Count);

            var totalItems = await query.CountAsync().ConfigureAwait(false);

            // Si la size est pas superieur a 0 pas la peine de faire une nouvelle query a la base de donné.
            return (size > 0) ?
                new(items: await query.Skip(size * (index)).Take(size).ToListAsync().ConfigureAwait(false),
                    totalItems: totalItems,
                    currentPage: index,
                    pageSize: size)
                :
                new(items: new List<PagedBestPublishers>(),
                    totalItems: totalItems,
                    currentPage: index,
                    pageSize: size);
        }

        public async Task<PagedItems<PagedBestSubscribers>> GetTopSubscribers(int index, int size)
        {
            //var query = _context.Users
            //    .Join(_context.Invoices, u => u.Id, i => i.UserId, (u, i) => new { u, i })
            //    .GroupBy(g => g.i.UserId)
            //     .Where(i => i.Count() > 0)
            //       .Select(g => new PagedBestSubscribers
            //       {
            //           UserId = g.Key,
            //         //  Name = g.Select(g => g.u).FirstOrDefault().FirstName + " " + g.Select(g => g.u).FirstOrDefault().LastName,
            //           Count = g.Count(),

            //           Value = g.Sum(g => g.i.Price)
            //       })
            //    .OrderByDescending(b => b.Count);

            var query = _context.Users
               .Include(u=>u.Invoices)
                 .Where(u => u.Invoices.Count() > 0)

                .Select(u => new PagedBestSubscribers
                {
                    UserId = u.Id.ToString(),
                    Name = u.FirstName.Trim()+" "+u.LastName.Trim(),
                    Count = u.Invoices.Count(),
                    Value = u.Invoices.Sum(i => i.Price)
                })
                .OrderByDescending(b => b.Count);


            var totalItems = await query.CountAsync().ConfigureAwait(false);

            // Si la size est pas superieur a 0 pas la peine de faire une nouvelle query a la base de donné.
            return (size > 0) ?
                new(items: await query.Skip(size * (index)).Take(size).ToListAsync().ConfigureAwait(false),
                    totalItems: totalItems,
                    currentPage: index,
                    pageSize: size)
                :
                new(items: new List<PagedBestSubscribers>(),
                    totalItems: totalItems,
                    currentPage: index,
                    pageSize: size);

            //    .Select(b => new PagedBestPublishers
            //    {
            //        PublisherId = b.Id.ToString(),
            //        RaisonSociale = b.RaisonSocial,
            //        Count = b.Books.SelectMany(b => b.Invoices).Count(),
            //    })
            //    .OrderByDescending(b => b.Count);

            //var totalItems = await query.CountAsync().ConfigureAwait(false);

            //// Si la size est pas superieur a 0 pas la peine de faire une nouvelle query a la base de donné.
            //return (size > 0) ?
            //    new(items: await query.Skip(size * (index)).Take(size).ToListAsync().ConfigureAwait(false),
            //        totalItems: totalItems,
            //        currentPage: index,
            //        pageSize: size)
            //    :
            //    new(items: new List<PagedBestPublishers>(),
            //        totalItems: totalItems,
            //        currentPage: index,
            //        pageSize: size);
        }


        public async Task<PagedItems<PagedSubscriberPurchase>> GetSubscriberPurchaseDetails(int index, int size, string userId)
        {
            var query = _context.Users.Where(u=>u.Id==userId)
                .Join(_context.Invoices, u => u.Id, i => i.UserId, (u, i) => new { u, i })
                .Join(_context.Books, g => g.i.BookId, b => b.Id, (g, b) => new { g, b })
                .Join(_context.Users, r => r.b.PublisherId, p => p.Id, (r, p) => new { r, p })
                   .Select(f => new PagedSubscriberPurchase
                   {
                       PublisherName = f.p.RaisonSocial,
                       BookTitle = f.r.b.Title,
                       Date=f.r.g.i.Date,
                       Price=f.r.g.i.Price
                   }).OrderByDescending(r=>r.Date);
            var totalItems = await query.CountAsync().ConfigureAwait(false);

            // Si la size est pas superieur a 0 pas la peine de faire une nouvelle query a la base de donné.
            return (size > 0) ?
                new(items: await query.Skip(size * (index)).Take(size).ToListAsync().ConfigureAwait(false),
                    totalItems: totalItems,
                    currentPage: index,
                    pageSize: size)
                :
                new(items: new List<PagedSubscriberPurchase>(),
                    totalItems: totalItems,
                    currentPage: index,
                    pageSize: size);

            //    .Select(b => new PagedBestPublishers
            //    {
            //        PublisherId = b.Id.ToString(),
            //        RaisonSociale = b.RaisonSocial,
            //        Count = b.Books.SelectMany(b => b.Invoices).Count(),
            //    })
            //    .OrderByDescending(b => b.Count);

            //var totalItems = await query.CountAsync().ConfigureAwait(false);

            //// Si la size est pas superieur a 0 pas la peine de faire une nouvelle query a la base de donné.
            //return (size > 0) ?
            //    new(items: await query.Skip(size * (index)).Take(size).ToListAsync().ConfigureAwait(false),
            //        totalItems: totalItems,
            //        currentPage: index,
            //        pageSize: size)
            //    :
            //    new(items: new List<PagedBestPublishers>(),
            //        totalItems: totalItems,
            //        currentPage: index,
            //        pageSize: size);
        }

        #endregion Filtrage & Tri Dashboard
    }
}
