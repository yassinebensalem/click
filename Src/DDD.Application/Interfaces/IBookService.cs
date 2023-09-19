using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.Specifications;
using DDD.Application.ViewModels;
using DDD.Domain.Common.Pagination;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Specifications;
using static DDD.Domain.Common.Constants.State;
using DashboardFilter = DDD.Application.Specifications.DashboardFilter;
using PagedBooks = DDD.Application.Specifications.PagedBooks;

namespace DDD.Application.Interfaces
{
    public interface IBookService : IDisposable
    {
        IEnumerable<BookViewModel> GetAll();
        IEnumerable<BookViewModel> GetAllBookPublished(ISpecification<Book> spec);
        IEnumerable<BookViewModel> GetFreeBooks(int currentPageIndex,int take);
        IEnumerable<BookViewModel> GetNewBooks(int currentPageIndex,int take);
        IEnumerable<BookViewModel> GetAll(int skip, int take);
        IEnumerable<BookViewModel> GetAllBookPublished(int skip, int take);
        BookViewModel GetBookById(Guid Id);
        bool AddBook(BookViewModel bookViewModel);
        bool UpdateBook(BookViewModel bookViewModel);
        bool UpdateBookState(BookStatePutVM bookStatePutVM);
        bool DeleteBook(Guid id);
        IList<BookHistoryData> GetAllHistory(Guid id);
        IEnumerable<BookViewModel> GetBookByCategory(string Category, int CurrentPageIndex, int take);
        IEnumerable<BookViewModel> GetTunisianBook( int CurrentPageIndex, int take);
        IEnumerable<BookViewModel> GetForeignBook( int CurrentPageIndex, int take);
        IEnumerable<BookViewModel> GetBookByTitleAndDescription(string keyWord, int CurrentPageIndex, int take);
        IEnumerable<BookViewModel> GetBookByAuthor(string keyWord, int CurrentPageIndex, int take);
        //IEnumerable<BookViewModel> GetBookByAuthorId(Guid Id);
        IEnumerable<BookViewModel> GetBookByEditor(string keyWord, int CurrentPageIndex, int take);
        //IEnumerable<BookViewModel> GetBookByEditorId(string Id, int CurrentPageIndex, int take);
        IEnumerable<BookViewModel> GetBooksByEditorId(string EditorId, int CurrentPageIndex, int take);
        IEnumerable<BookViewModel> GetBooksByAuthorId(Guid AuthorId, int CurrentPageIndex, int take);
        IEnumerable<BookViewModel> GetBooksByAuthorId_WithoutPagination(Guid AuthorId);
        IEnumerable<BookViewModel> GetRelatedBooksByCategory(Guid categoryId, int current, int take);
        IEnumerable<BookViewModel> GetBookByEditorIdInterval(BookPostVM bookPostVM);
        Task<IEnumerable<BookViewModel>> GetPagedBooks(PagedBooks booksFilter);
        Task<IEnumerable<BookViewModel>> SearchBooksByTitle(string searchKey);
        Task<double> GetPublishedBookCount(PagedBooks booksFilter);
        Task<double> GetSelledBookCount();
        Task<List<SelledBooksChartVM>> GetDaySelledBooksByFilter(DashboardFilter dashboardFilter);
        Task<double> GetTotalSelledBooksCountByFilter(DashboardFilter dashboardFilter);
        Task<PagedItems<PagedSelledBooksVM>> GetTopSelledBooks(int index, int size);
        Task<PagedItems<PagedBestPublishersVM>> GetTopPublishers(int index, int size);
        Task<PagedItems<PagedBestSubscribersVM>> GetTopSubscribers(int index, int size);
        Task<PagedItems<PagedSubscriberPurchaseVM>> GetSubscriberPurchaseDetails(int index, int size, string userId);
    }
}
