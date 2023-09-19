using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using DDD.Application.EventSourcedNormalizers;
using DDD.Application.Interfaces;
using DDD.Application.Specifications;
using DDD.Application.ViewModels;
using DDD.Domain.Commands;
using DDD.Domain.Common.Pagination;
using DDD.Domain.Core.Bus;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Domain.Specifications;
using DDD.Infra.Data.Context;
using DDD.Infra.Data.Repository;
using DDD.Infra.Data.Repository.EventSourcing;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static DDD.Domain.Common.Constants.GlobalConstant;

namespace DDD.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler Bus;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        public BookService(IMapper mapper, IBookRepository bookRepository, IEventStoreRepository eventStoreRepository,
           ICategoryService categoryService, UserManager<ApplicationUser> userManager,
            IMediatorHandler bus)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
            _eventStoreRepository = eventStoreRepository;
            Bus = bus;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        //start implementing needed services
        public IEnumerable<BookViewModel> GetAll()
        {
            return _bookRepository.GetAll().ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<BookViewModel> GetFreeBooks(int currentPageIndex, int take)
        {
            return _bookRepository.GetAll(new FreeBookSpecification(currentPageIndex, take)).ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<BookViewModel> GetNewBooks(int currentPageIndex, int take)
        {
            return _bookRepository.GetAll(new NewBooksSpecification(currentPageIndex, take)).ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<BookViewModel> GetAll(int skip, int take)
        {
            return _bookRepository.GetAll(new BookFilterPaginatedSpecification(skip, take))
                .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        }

        public BookViewModel GetBookById(Guid Id)
        {
            return _mapper.Map<BookViewModel>(_bookRepository.GetById(Id));
        }

        public async Task<IEnumerable<BookViewModel>> GetPagedBooks(Specifications.PagedBooks booksFilter)
        {
            var filter = ToFiltersAsync(booksFilter);
            var books = await _bookRepository.GetPagedBooks(filter, booksFilter.PageIndex, booksFilter.PageSize).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<BookViewModel>>(books);
        }

        public bool AddBook(BookViewModel bookViewModel)
        {
            try
            {
                var registerCommand = _mapper.Map<RegisterNewBookCommand>(bookViewModel);
                Bus.SendCommand(registerCommand);
                //_dbContext.SaveChanges();
                //_bookRepository.Add(bookViewModel);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool UpdateBook(BookViewModel bookViewModel)
        {
            try
            {
                var updateCommand = _mapper.Map<UpdateBookCommand>(bookViewModel);
                Bus.SendCommand(updateCommand);
                //_bookRepository.Update(b);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool UpdateBookState(BookStatePutVM bookStatePutVM)
        {
            try
            {
                var updateCommand = _mapper.Map<UpdateBookStateCommand>(bookStatePutVM);
                Bus.SendCommand(updateCommand);
                //_bookRepository.Update(b);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool DeleteBook(Guid Id)
        {
            try
            {
                var removeCommand = new RemoveBookCommand(Id);
                Bus.SendCommand(removeCommand);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public IList<BookHistoryData> GetAllHistory(Guid id)
        {
            return BookHistory.ToJavaScriptBookHistory(_eventStoreRepository.All(id));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerable<BookViewModel> GetBookByTitleAndDescription(string Book, int CurrentPageIndex, int take)
        {
            var list = _bookRepository.GetAll(new BookTitleAndDescriptionSpecification(Book, CurrentPageIndex, take))
                .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        public IEnumerable<BookViewModel> GetBookByAuthor(string Author, int CurrentPageIndex, int take)
        {
            var list = _bookRepository.GetAll(new AuthorSpecification(Author, CurrentPageIndex, take))
                .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        //public IEnumerable<BookViewModel> GetBookByAuthorId(Guid Id)
        //{
        //    var list = _bookRepository.GetAll()
        //        .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        //    return list;
        //}

        public IEnumerable<BookViewModel> GetBookByEditor(string Editor, int CurrentPageIndex, int take)
        {
            var list = _bookRepository.GetAll(new EditorSpecification(Editor, CurrentPageIndex, take))
                .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        //public IEnumerable<BookViewModel> GetBookByEditorId(string Id, int CurrentPageIndex, int take)
        //{
        //    var list = _bookRepository.GetAll(new EditorByIdSpecification(Id, CurrentPageIndex, take))
        //        .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        //    return list;
        //}

        public IEnumerable<BookViewModel> GetBooksByEditorId(string EditorId, int CurrentPageIndex, int take)
        {
            var list = _bookRepository.GetAll(new EditorIdSpecification(EditorId, CurrentPageIndex, take))
                .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        public IEnumerable<BookViewModel> GetBooksByAuthorId(Guid AuthorId, int CurrentPageIndex, int take)
        {
            var list = _bookRepository.GetAll(new AuthorIdSpecification(AuthorId, CurrentPageIndex, take))
                .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        public IEnumerable<BookViewModel> GetBooksByAuthorId_WithoutPagination(Guid AuthorId)
        {
            var list = _bookRepository.GetAll(new AuthorIdSpecification_WithoutPagination(AuthorId))
                .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        public IEnumerable<BookViewModel> GetBookByCategory(string Category, int CurrentPageIndex, int take)
        {
            var list = _bookRepository.GetAll(new CategorySpecification(Category, CurrentPageIndex, take))
                .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        public IEnumerable<BookViewModel> GetRelatedBooksByCategory(Guid categoryId, int current, int take)
        {
            var list = _bookRepository.GetAll(new RelatedBooksSpecification(categoryId, current, take))
            .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        public IEnumerable<BookViewModel> GetTunisianBook(int CurrentPageIndex, int take)
        {
            var list = _bookRepository.GetAll(new TunisianBooksSpecification(CurrentPageIndex, take))
                .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        public IEnumerable<BookViewModel> GetForeignBook(int current, int take)
        {
            var list = _bookRepository.GetAll(new ForeignBooksSpecification(current, take))
            .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            return list;
        }

        public IEnumerable<BookViewModel> GetBookByEditorIdInterval(BookPostVM bookPostVM)
        {
            DateTime fromDate;
            DateTime toDate;
            DateTime.TryParse(bookPostVM.FromDate, out fromDate);
            DateTime.TryParse(bookPostVM.ToDate, out toDate);

            if (string.IsNullOrEmpty(bookPostVM.PublisherId))
                return _bookRepository.GetAll(new BookbyIntervalSpecification(fromDate, toDate)).ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
            else
                return _bookRepository.GetAll(new BookbyEditeurIntervalSpecification(fromDate, toDate, bookPostVM.PublisherId)).ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<BookViewModel> GetAllBookPublished(ISpecification<Book> spec)
        {
            return _bookRepository.GetAllBookPublished(spec).ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        }

        public IEnumerable<BookViewModel> GetAllBookPublished(int skip, int take)
        {
            return _bookRepository.GetAllBookPublished(new BookFilterPaginatedSpecification(skip, take))
                .ProjectTo<BookViewModel>(_mapper.ConfigurationProvider);
        }
        public async Task<IEnumerable<BookViewModel>> SearchBooksByTitle(string searchKey)
        {
            var books = await _bookRepository.SearchBooksByTitle(searchKey).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<BookViewModel>>(books);
        }

        public async Task<double> GetPublishedBookCount(Specifications.PagedBooks booksFilter)
        {
            var filter = ToFiltersAsync(booksFilter);
            var booksCount = await _bookRepository.GetPublishedBooksCount(filter);
            return booksCount;
        }
        public async Task<double> GetSelledBookCount()
        {
            //var booksCount = await _bookRepository.GetBooksCountByFilter(filter);
            //return booksCount;
            return 0;
        }
        public async Task<double> GetTotalSelledBooksCountByFilter(Specifications.DashboardFilter dashboardFilter)
        {
            var filter = ToDashboardFiltersAsync(dashboardFilter);
            var booksCount = await _bookRepository.GetTotalSelledBooksCountByFilter(filter);
            return booksCount;
        }

        public async Task<PagedItems<PagedSelledBooksVM>> GetTopSelledBooks(int index, int size)
        {
            var result = await _bookRepository.GetTopSelledBooks(index,size);
            var books= _mapper.Map<List<PagedSelledBooksVM>>(result.Items);

            return new PagedItems<PagedSelledBooksVM>(
                items: books,
                totalItems: result.TotalItems,
                currentPage: result.CurrentPage,
                pageSize: result.PageSize
            );
        }
        public async Task<PagedItems<PagedBestPublishersVM>> GetTopPublishers(int index, int size)
        {
            var result = await _bookRepository.GetTopPublishers(index, size);
            var books = _mapper.Map<List<PagedBestPublishersVM>>(result.Items);

            return new PagedItems<PagedBestPublishersVM>(
            items: books,
                totalItems: result.TotalItems,
                currentPage: result.CurrentPage,
                pageSize: result.PageSize
            );
        }

        public async Task<PagedItems<PagedBestSubscribersVM>> GetTopSubscribers(int index, int size)
        {
            var result = await _bookRepository.GetTopSubscribers(index, size);
            var books = _mapper.Map<List<PagedBestSubscribersVM>>(result.Items);

            return new PagedItems<PagedBestSubscribersVM>(
            items: books,
                totalItems: result.TotalItems,
                currentPage: result.CurrentPage,
                pageSize: result.PageSize
            );
        }

        public async Task<PagedItems<PagedSubscriberPurchaseVM>> GetSubscriberPurchaseDetails(int index, int size, string userId)
        {
            var result = await _bookRepository.GetSubscriberPurchaseDetails(index, size, userId);
            var books = _mapper.Map<List<PagedSubscriberPurchaseVM>>(result.Items);

            return new PagedItems<PagedSubscriberPurchaseVM>(
            items: books,
                totalItems: result.TotalItems,
                currentPage: result.CurrentPage,
                pageSize: result.PageSize
            );
        }
        public async Task<List<SelledBooksChartVM>> GetDaySelledBooksByFilter(Specifications.DashboardFilter dashboardFilter)
        {
            var filter = ToDashboardFiltersAsync(dashboardFilter);
            var books = await _bookRepository.GetDaySelledBooksByFilter(filter);
            var minDate = books.Min(g => g.Date.Date);
            var maxDate = books.Max(g => g.Date.Date);
            var range = GetDateRange(minDate, maxDate);

            var result = from d in range
                         from item in books.Where(g => g.Date.Date == d)
                                               .DefaultIfEmpty()
                         select new SelledBooksChartVM
                         {
                             Date = d,
                             Count = item?.Count ?? 0,
                             Value = item?.Value ?? 0
                         };
            return _mapper.Map<List<SelledBooksChartVM>>(result);
        }

        #region Private
        private static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            var date = startDate.Date;

            while (date <= endDate.Date)
            {
                yield return date;
                date = date.AddDays(1);
            }
        }
        private Domain.Specifications.PagedBooks ToFiltersAsync(Specifications.PagedBooks booksFilter)
        {
            return new Domain.Specifications.PagedBooks
            {
                EditorId = booksFilter.EditorId,
                Categories = booksFilter.Categories,
                Authors = booksFilter.Authors,
                Languages = booksFilter.Languages,
                IsPromotedBook = booksFilter.IsPromotedBook,
                PromotionType = booksFilter.PromotionType,
                SearchKeyType = _mapper.Map<SearchEnum>(booksFilter.SearchKeyType),
                SearchKeyText = booksFilter.SearchKeyText,
            };
        }

        private Domain.Specifications.DashboardFilter ToDashboardFiltersAsync(Specifications.DashboardFilter dashboardFilter)
        {
            return new Domain.Specifications.DashboardFilter
            {
                BookId = dashboardFilter.BookId,
                EditorId = dashboardFilter.EditorId,
                AuthorId = dashboardFilter.AuthorId,
                LanguageId = dashboardFilter.LanguageId,
                Date = dashboardFilter.Date,
                FromDate = dashboardFilter.FromDate,
                ToDate = dashboardFilter.ToDate,
            };
        }
        #endregion
    }
}
