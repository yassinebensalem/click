using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Domain.Common.Pagination;
using DDD.Domain.Models;
using DDD.Domain.Specifications;

namespace DDD.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IQueryable<Book> GetAllBookPublished(ISpecification<Book> spec);
        Task<IEnumerable<Book>> GetPagedBooks(PagedBooks booksFilter, int pageIndex, int pageSize);
        Task<IEnumerable<Book>> SearchBooksByTitle(string searchKey);
        Task<long> GetPublishedBooksCount(PagedBooks booksFilter);
        //Task<long> GetSelledBooksCount();
        Task<long> GetTotalSelledBooksCountByFilter(DashboardFilter filters);
        Task<List<SelledBooksChart>> GetDaySelledBooksByFilter(DashboardFilter filters);
        Task<PagedItems<PagedSelledBooks>> GetTopSelledBooks(int index, int size);
        Task<PagedItems<PagedBestPublishers>> GetTopPublishers(int index, int size);
        Task<PagedItems<PagedBestSubscribers>> GetTopSubscribers(int index, int size);
        Task<PagedItems<PagedSubscriberPurchase>> GetSubscriberPurchaseDetails(int index, int size, string userId);
    }
}
