using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DDD.Application.ILogics;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Models;
using DDD.Infra.CrossCutting.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MipLivrelStore.Helper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using static DDD.Domain.Common.Constants.State;

namespace MipLivrelStore.Controllers
{
    public class BookController : BaseController
    {
        SearchResultVM searchResultVM = new SearchResultVM();
        private readonly IConfiguration _configuration;
        private readonly IJoinRequestService _joinRequestService;
        private readonly IFileManagerLogic _fileManagerLogic;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;
        private readonly IAuthorService _authorService;
        private readonly IFavoriteService _IFavoriteService;
        private readonly ICartService _cartService;
        private readonly IPrizeService _prizeService;
        private const int PAGINATION_TAKE = 12;
        private ICompositeViewEngine _viewEngine;
        private readonly IPromotionService _promotionService;
        private readonly IPromoUserService _promoUserService;
        private readonly ILibraryService _libraryService;
        List<PromotionVM> promotionsList = new List<PromotionVM>();
        List<PromotionBook> promotionsBookList = new List<PromotionBook>();

        public BookController(
            IFileManagerLogic fileManagerLogic,
             IJoinRequestService joinRequestService,
        IMemoryCache memoryCache, ICompositeViewEngine viewEngine,
           UserManager<ApplicationUser> userManager,
           ILoggerFactory loggerFactory,
           INotificationHandler<DomainNotification> notifications,
             IMediatorHandler mediator,
             IBookService bookService,
             ICategoryService categoryService,
             IFavoriteService IFavoriteService, IPromoUserService promoUserService,
             IMapper mapper, IPromotionService promotionService,
             ICountryService countryService, ILibraryService libraryService,
             ILanguageService languageService, IAuthorService authorService, ICartService cartService, IConfiguration configuration, IPrizeService prizeService)

            : base(joinRequestService, fileManagerLogic, memoryCache, userManager, loggerFactory, notifications,
              mediator, bookService, categoryService, mapper, promotionService, countryService, languageService, cartService, authorService, prizeService, libraryService)
        {
            _joinRequestService = joinRequestService;
            _fileManagerLogic = fileManagerLogic;
            _memoryCache = memoryCache;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<BookController>();
            _categoryService = categoryService;
            _mapper = mapper;
            _bookService = bookService;
            _countryService = countryService;
            _languageService = languageService;
            _authorService = authorService;
            _IFavoriteService = IFavoriteService;
            _cartService = cartService;
            _configuration = configuration;
            _prizeService = prizeService;
            _viewEngine = viewEngine;
            _promotionService = promotionService;
            _libraryService = libraryService;
            _promoUserService = promoUserService;
        }

        [HttpGet]
        public IActionResult DiscountBooks()
        {
            SearchResultVM _searchResultVM = new SearchResultVM();
            _searchResultVM.BooksList = _promotionService.GetDiscountBooks(0, PAGINATION_TAKE);
            var allCategories = base.GetAllCategories().ToList();
            var allAuthors = base.GetAllAuthorVM().ToList();
            //var allAuthors = base.GetAllAuthors().ToList();
            var allLanguages = base.GetAllLanguages().ToList();
            var allEditors = base.GetAllEditors().ToList();
            var allCountries = base.GetAllCountries().ToList();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
            var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
            var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
            var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
            _SetLibraryCapacity(_searchResultVM.BooksList, FreeBooksList, AllFreePromotions, userId);

            var languages = _searchResultVM.BooksList.Select(b => b.LanguageId);
            var authors = _searchResultVM.BooksList.Select(b => b.AuthorId).Distinct();
            var categories = _searchResultVM.BooksList.Select(b => b.CategoryId);

            _searchResultVM.KeyWord = "DiscountBooks";
            _searchResultVM.CategoriesList = allCategories.Where(c => categories.Distinct().Contains(c.Id));
            _searchResultVM.AuthorsList = allAuthors.Distinct().Where(c => authors.Contains(c.Id));
            _searchResultVM.LanguagesList = allLanguages.Distinct().Where(c => languages.Contains(c.Id));

            FillBookVMAdditionalInfos.FillBookVMList(_searchResultVM.BooksList, _searchResultVM.AuthorsList.ToList(),
                allEditors, _searchResultVM.CategoriesList.ToList(), allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

            return PartialView("PromotionBooks", _searchResultVM);
        }

        [HttpPost]
        public async Task<IActionResult> LoadPromoBookList([FromBody] DDD.Application.Specifications.PagedBooks pagedBooks)
        {
            var allEditors = base.GetAllEditors().ToList();
            var allAuthors = base.GetAllAuthorVM().ToList();
            var allCategories = base.GetAllCategories().ToList();
            var allLanguages = base.GetAllLanguages().ToList();
            var allCountries = base.GetAllCountries().ToList();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
            var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
            var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
            var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();

            SearchResultVM _searchResultVM = new SearchResultVM();

            if (pagedBooks.PromotionType == PromotionType.Discount)
                _searchResultVM.BooksList = _promotionService.GetDiscountBooks(pagedBooks.PageIndex, PAGINATION_TAKE);
            else _searchResultVM.BooksList = (List<BookViewModel>) await _bookService.GetPagedBooks(pagedBooks);

            var languages = _searchResultVM.BooksList.Select(b => b.LanguageId);
            var authors = _searchResultVM.BooksList.Select(b => b.AuthorId).Distinct();
            var categories = _searchResultVM.BooksList.Select(b => b.CategoryId);

            _searchResultVM.CategoriesList = allCategories.Where(c => categories.Contains(c.Id));
            _searchResultVM.AuthorsList = allAuthors.Where(c => authors.Contains(c.Id));
            _searchResultVM.LanguagesList = allLanguages.Where(c => languages.Contains(c.Id));//.GroupBy(x => x.Name).Select(y => y.First());

            _SetLibraryCapacity(_searchResultVM.BooksList, FreeBooksList, AllFreePromotions, userId);

            FillBookVMAdditionalInfos.FillBookVMList(_searchResultVM.BooksList, _searchResultVM.AuthorsList.ToList(),
                allEditors, _searchResultVM.CategoriesList.ToList(), allCountries, _searchResultVM.LanguagesList.ToList(),
                null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

            var booksGridView = await RenderPartialViewToString("../Home/GridViewSearchResult", _searchResultVM.BooksList);
            var booksListView = await RenderPartialViewToString("../Home/ListViewSearchResult", _searchResultVM.BooksList);

            if (_searchResultVM.BooksList.Count() > 0)
            {
                return Json(new
                {
                    Status = 1,
                    BooksCount = _searchResultVM.BooksList.Count(),
                    Authors = _searchResultVM.AuthorsList,
                    Categories = _searchResultVM.CategoriesList,
                    Languages = _searchResultVM.LanguagesList,
                    BooksGridView = booksGridView,
                    BooksListView = booksListView
                });
            }
            else
            {
                return Json(new
                {
                    Status = 0
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> FreeBooks()
        {
            SearchResultVM _searchResultVM = new SearchResultVM();
            _searchResultVM.BooksList = (List<BookViewModel>)await _bookService.GetPagedBooks(new DDD.Application.Specifications.PagedBooks { IsPromotedBook = true, PageIndex = 0, PageSize= 12}); ;
            // _searchResultVM.BooksList = _promotionService._GetFreeBooks(0, PAGINATION_TAKE).ToList();
            var allCategories = base.GetAllCategories().ToList();
            var allAuthors = await _authorService.GetUsedAuthors();
            var allLanguages = await _languageService.GetUsedLanguages();
            var allEditors = base.GetAllEditors().ToList();
            var allCountries = base.GetAllCountries().ToList();

            var languages = _searchResultVM.BooksList.Select(b => b.LanguageId).Distinct().ToList();
            var authors = _searchResultVM.BooksList.Select(b => b.AuthorId).Distinct().ToList();
            var categories = _searchResultVM.BooksList.Select(b => b.CategoryId).Distinct().ToList();

            _searchResultVM.CategoriesList = allCategories.Distinct();
            _searchResultVM.AuthorsList = allAuthors;
            _searchResultVM.LanguagesList = allLanguages;

            _searchResultVM.KeyWord = "FreeBooks";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
            var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
            var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
            var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
            _SetLibraryCapacity(_searchResultVM.BooksList, FreeBooksList, AllFreePromotions, userId);

            FillBookVMAdditionalInfos.FillBookVMList(_searchResultVM.BooksList, _searchResultVM.AuthorsList.ToList(), allEditors, _searchResultVM.CategoriesList.ToList(), allCountries, _searchResultVM.LanguagesList.ToList(), null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

            return PartialView("PromotionBooks", _searchResultVM);
        }

        public List<CategoryViewModel> ListCategory(string CategoryName)
        {
            return _categoryService.ListCategory(CategoryName);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //var AuthorsList = _userManager.GetUsersInRoleAsync(UserRoleVM.Author.ToString()).Result;
            var categories = _categoryService.GetAll().ToList();
            ViewBag.categories = categories;

            var language = _languageService.GetAll().ToList();
            ViewBag.language = language;

            var country = _countryService.GetAll().ToList();
            ViewBag.country = country;

            var appUser = _userManager.GetUsersInRoleAsync(Roles.Author).Result;
            ViewBag.appUser = appUser;

            return PartialView();
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] BookViewModel bookViewModel)
        {
            var appUser = _userManager.FindByNameAsync(User.Identity.Name);
            bookViewModel.PublisherId = appUser.Result.Id;

            string fileName = Guid.NewGuid().ToString();
            if (bookViewModel.PDFFile != null)
            {
                bookViewModel.PDFPath = $"{fileName}{Path.GetExtension(bookViewModel.PDFFile.FileName)}";
                _fileManagerLogic.Upload(bookViewModel.PDFFile, "bookspdf", bookViewModel.PDFPath);
            }
            if (bookViewModel.CoverFile != null)
            {
                bookViewModel.CoverPath = $"{fileName}{Path.GetExtension(bookViewModel.CoverFile.FileName)}";
                _fileManagerLogic.Upload(bookViewModel.CoverFile, "bookscover", bookViewModel.CoverPath);
            }

            _bookService.AddBook(bookViewModel);
            return Json(Ok(new
            {
                success = true,
                data = bookViewModel
            }));

        }

        [HttpGet]
        public IActionResult GetBookDetails(string BookInfos)
        {
            try
            {
                var author = new Author();
                var publisher = new ApplicationUser();
                BookDetailsVM bookDetailsVM = new BookDetailsVM();

                bookDetailsVM.BookListVM = new List<BookViewModel>();
                var bookId = BookInfos.Split("#")[0];
                bookDetailsVM.Book = _bookService.GetBookById(Guid.Parse(bookId));
                bookDetailsVM.RelatedBooks = _bookService.GetRelatedBooksByCategory(bookDetailsVM.Book.CategoryId, 0, PAGINATION_TAKE).ToList();

                var allPrizebook = base.GetAllPrizeBook().ToList();
                var allEditors = base.GetAllEditors().ToList();
                var allAuthors = base.GetAllAuthorVM().ToList();
                //var allAuthors = base.GetAllAuthors().ToList();
                var allCategories = base.GetAllCategories().ToList();
                var allLanguages = base.GetAllLanguages().ToList();
                var allCountries = base.GetAllCountries().ToList();

                foreach (var prizeVM in allPrizebook)
                {
                    var book = _bookService.GetBookById(prizeVM.BookId);
                    bookDetailsVM.BookListVM.Add(book);
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
                var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
                var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
                var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
                var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
                var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();

                _SetLibraryCapacity(bookDetailsVM.RelatedBooks, FreeBooksList, AllFreePromotions, userId);
                _SetLibraryCapacity(bookDetailsVM.BookListVM, FreeBooksList, AllFreePromotions, userId);
                _SetLibraryCapacity(new List<BookViewModel> { bookDetailsVM.Book }, FreeBooksList, AllFreePromotions, userId);

                FillBookVMAdditionalInfos.FillBookVMList(bookDetailsVM.RelatedBooks, allAuthors, allEditors, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
                FillBookVMAdditionalInfos.FillBookVMList(bookDetailsVM.BookListVM, allAuthors, allEditors, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

                //bookDetailsVM.RelatedBooks = FillBookVMAdditionalInfos.FillBookVMList(bookDetailsVM.RelatedBooks, allAuthors, allEditors, allCategories, allCountries, allLanguages);
                //bookDetailsVM.BookListVM = FillBookVMAdditionalInfos.FillBookVMList(bookDetailsVM.BookListVM, allAuthors, allEditors, allCategories, allCountries, allLanguages);

                //Set Promotion of books
                //FillBookVMAdditionalInfos.SetPromotionOfBooks(bookDetailsVM.RelatedBooks, DiscountedBooksList, FreeBooksList);
                //FillBookVMAdditionalInfos.SetPromotionOfBooks(bookDetailsVM.BookListVM, DiscountedBooksList, FreeBooksList);
                //FillBookVMAdditionalInfos.SetPromotionOfBooks(new List<BookViewModel> { bookDetailsVM.Book }, DiscountedBooksList, FreeBooksList);

                //if (FreeBooksList.Where(b => b.Id == bookDetailsVM.Book.Id).FirstOrDefault() != null)
                //    bookDetailsVM.Book.PromotionsPercentage = 100;
                //check if in library
                var library = _libraryService.GetLibraryByUserIdAndBookId(userId, bookDetailsVM.Book.Id).FirstOrDefault();
                if (library != null)
                {
                    bookDetailsVM.Book.inLibrary = !(library.Id == Guid.Empty);
                    bookDetailsVM.Book.LibraryId = !(library.Id == Guid.Empty) ? library.Id.ToString() : "-1";
                }

                //check if book is in favorite
                foreach (var bookVM in bookDetailsVM.RelatedBooks)
                {
                    var fav = _IFavoriteService.GetFavoriteBookByUserIdAndBookId(userId, bookVM.Id).FirstOrDefault();
                    if (fav != null) bookVM.isFavorite = !(fav.Id == Guid.Empty);
                    //var c = _cartService.GetCartByUserIdAndBookId(userId, bookVM.Id).FirstOrDefault(); 
                }
                FillBookVMAdditionalInfos.FillBookVMModel(bookDetailsVM.Book, allAuthors, allEditors, allCategories, allCountries, allLanguages);

                var fb = _IFavoriteService.GetFavoriteBookByUserIdAndBookId(userId, bookDetailsVM.Book.Id).FirstOrDefault();
                if (fb != null) bookDetailsVM.Book.isFavorite = !(fb.Id == Guid.Empty);
                //var cb = allCartItems.Find(x => x.BookVM.Id == bookDetailsVM.Book.Id);
                //_cartService.GetCartByUserIdAndBookId(userId, bookDetailsVM.Book.Id).FirstOrDefault();
                //if (cb != null) bookDetailsVM.Book.inCart = !(cb.BookVM.Id == Guid.Empty);
                if (User.Identity.IsAuthenticated)
                {
                    var allCartItems = base.GetCartItems().ToList();
                    if (allCartItems != null && allCartItems.Count > 0)
                    {
                        var ccb = allCartItems.Find(x => x.CartVM.Book.Id == bookDetailsVM.Book.Id);
                        if (ccb != null) bookDetailsVM.Book.inCart = !(ccb.CartVM.Book.Id == Guid.Empty);
                    }
                }

                //ViewBag.FacebookShareUrl = $"{Request.Scheme}://{Request.Host}/Book/GetBookDetails/{bookDetailsVM.Book.Title.Replace(" ", "-")}#{bookDetailsVM.Book.Id}";
                ViewBag.FacebookShareUrl = $"https://miplivrel.azurewebsites.net/Book/GetBookDetails/{bookDetailsVM.Book.Title.Replace(" ", "-")}#{bookDetailsVM.Book.Id}";
                ViewBag.FacebookShareImage = $"https://miplivrelstorage.blob.core.windows.net/bookscover/{bookDetailsVM.Book.CoverPath}";
                ViewBag.FacebookShareDescription = bookDetailsVM.Book.Description.Length > 30 ? bookDetailsVM.Book.Description.Substring(0, 30) + "..." : bookDetailsVM.Book.Description;

                return PartialView(bookDetailsVM);
            }
            catch (Exception exp)
            {
                return Response();
            }
        }

        [HttpPost]
        public JsonResult AddBookToFavorite(string BookId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                BookId = JsonConvert.DeserializeObject<string>(BookId);
                var existingBook = _IFavoriteService.GetFavoriteBookByUserIdAndBookId(userId, Guid.Parse(BookId)).FirstOrDefault();
                if (existingBook == null)
                {
                    var favoriteBookVM = new FavoriteBookVM();
                    favoriteBookVM.Id = Guid.NewGuid();
                    favoriteBookVM.UserId = userId;
                    favoriteBookVM.BookId = Guid.Parse(BookId);
                    _IFavoriteService.AddFavoriteBook(favoriteBookVM);
                    return Json(Ok(new
                    {
                        success = true,
                        data = favoriteBookVM.Id
                    }));
                }
                else
                    return new JsonResult(new { message = "Book_Exists_already" })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
            }
            catch (Exception ex)
            {
                return Json(BadRequest(new
                {
                    message = ex.Message
                }));
            }
        }

        [HttpDelete]
        public JsonResult DeleteBookFromFavorite(string BookId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                BookId = JsonConvert.DeserializeObject<string>(BookId);
                var fb = _IFavoriteService.GetFavoriteBookByUserIdAndBookId(userId, Guid.Parse(BookId)).FirstOrDefault();
                _IFavoriteService.DeleteFavoriteBook(fb.Id);
                return Json(Ok(new
                {
                    success = true
                }));
            }
            catch (Exception ex)
            {
                return Json(BadRequest(new { message = ex.Message }));
            }
        }

        [HttpPost]
        public JsonResult AddBookToCart(string BookId)
        {
            BookId = JsonConvert.DeserializeObject<string>(BookId);
            //check if book is already in cart list
            var existingBook = new CartViewModel();
            var list = base.GetCartItems();
            if (list != null && list.Count != 0) existingBook = list.Find(x => x.CartVM.Book.Id == Guid.Parse(BookId));
            if (existingBook == null || existingBook.CartVM == null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartVM = new CartVM();
                cartVM.Id = Guid.NewGuid();
                cartVM.UserId = userId;
                cartVM.BookId = Guid.Parse(BookId);
                var addResult = _cartService.AddBookToCart(cartVM);
                List<CartViewModel> CartList = base.SetCartCacheableItems();

                return Json(Ok(new
                {
                    success = true,
                    data = cartVM.Id,
                    count = CartList.Count,
                }));
            }
            else
                return new JsonResult(new { message = "Book_Exists_already" })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError

                };
        }

        [HttpDelete]
        public JsonResult DeleteBookFromCart(string BookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            BookId = JsonConvert.DeserializeObject<string>(BookId);
            var cb = _cartService.GetCartByUserIdAndBookId(userId, Guid.Parse(BookId)).FirstOrDefault();
            if (cb != null)
            {
                var deleteResult = _cartService.DeleteBookFromCart(cb.Id);
                if (deleteResult)
                {
                    base.SetCartCacheableItems();
                    //CartList = base.UpdateCartItems();
                }
                var list = base.GetCartItems();

                return Json(Ok(new
                {
                    success = true,
                    count = list.Count
                }));
            }
            else
            {
                return Json(NotFound(new { message = "Sorry, book was not found." }));
            }
        }

        [HttpPost]
        public JsonResult AddBookToLibrary(string BookId, string PromotionId)
        {
            try
            {//add to table Library
                //BookId = JsonConvert.DeserializeObject<string>(BookId);
                var _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                    var promoUser = _promoUserService.GetPromoUserByUserIdAndPromoId(_currentUser.Id, new Guid(PromotionId)).FirstOrDefault();
                var promo = _promotionService.GetPromotionById(new Guid(PromotionId));
                bool MaxReached = false;

                if (promoUser != null) MaxReached = promoUser.BooksTakenCount < promo.MaxFreeBooks;
                if (!MaxReached)
                {
                    LibraryVM libraryVM = new LibraryVM();
                    libraryVM.Id = Guid.NewGuid();
                    libraryVM.BookId = Guid.Parse(BookId);
                    libraryVM.UserId = _currentUser.Id;
                    _libraryService.AddBookToLibrary(libraryVM);
                    //add this user and promotion to table PromoUser in case it's not existing

                    //if this book is in a free promotion, increment the BooksTakenCount in table PromoUser
                    var promoUserList = _promoUserService.GetPromoUserByUserId(_currentUser.Id).ToList();
                    if (promoUserList.IsNullOrEmpty())
                    {
                        PromoUserVM promoUserVM = new PromoUserVM()
                        {
                            UserId = _currentUser.Id,
                            PromotionId = Guid.Parse(PromotionId),
                            BooksTakenCount = 1,
                        };
                        _promoUserService.AddPromoUser(promoUserVM);
                    }
                    foreach (var pu in promoUserList)
                    {
                        pu.BooksTakenCount++;
                        _promoUserService.UpdatePromoUser(pu);
                        //var promo = _promotionService.GetPromotionById(pu.PromotionId);
                        //if (promo.MaxFreeBooks >= pu.BooksTakenCount) MaxReached = true;
                    }
                    return Json(Ok(new
                    {
                        success = true,
                        data = libraryVM.Id,
                        maxReached = MaxReached
                    })); ;
                }
                //if (promoUserList.Any())
                //{}
                return Json(Ok(new
                {
                    success = true,
                    maxReached = MaxReached
                })); ;
            }
            catch (Exception ex)
            {
                return Json(BadRequest(new { message = "operation failed" }));
            }
        }

        [HttpPost]
        public JsonResult UpdateCurrentPage(UpdateLibraryVM updateLibraryVM)
        {
            //add to table Library
            var _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            updateLibraryVM.UserId = _currentUser.Id;
            _libraryService.UpdateCurrentPage(updateLibraryVM);

            return Json(Ok(new
            {
                success = true,
                data = updateLibraryVM.Id
            }));
        }

        [HttpDelete]
        public JsonResult DeleteBookFromLibrary(string BookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            BookId = JsonConvert.DeserializeObject<string>(BookId);
            var x = _libraryService.GetLibraryByUserIdAndBookId(userId, Guid.Parse(BookId)).FirstOrDefault();
            if (x != null)
            {
                var deleteResult = _libraryService.DeleteBookFromLibrary(x.Id);
                if (deleteResult)
                {
                    base.SetCartCacheableItems();
                    return Json(Ok(new
                    {
                        success = true
                    }));
                }
                else
                    return Json(BadRequest(new { message = "Sorry, book was not found." }));
            }
            else
            {
                return Json(NotFound(new { message = "Sorry, book was not found." }));
            }
        }


        [HttpGet]
        public IActionResult Update(string BookId)
        {
            ViewBag.categories = base.GetAllCategories().ToList();
            ViewBag.language = base.GetAllLanguages().ToList();
            ViewBag.country = base.GetAllCountries().ToList();
            ViewBag.authorsList = base.GetAllAuthorVM().ToList();
            //ViewBag.authorsList = base.GetAllAuthors().ToList();
            var existingBook = _bookService.GetBookById(Guid.Parse(BookId));
            return PartialView(existingBook);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook(BookViewModel bookViewModel)
        {
            bookViewModel.Status = DDD.Domain.Common.Constants.State.BookState.published;
            var appUser = _userManager.FindByNameAsync(User.Identity.Name);
            bookViewModel.PublisherId = appUser.Result.Id;
            try
            {
                string fileName = Guid.NewGuid().ToString();
                if (bookViewModel.PDFFile != null)
                {
                    bookViewModel.PDFPath = $"{fileName}{Path.GetExtension(bookViewModel.PDFFile.FileName)}";
                    await _fileManagerLogic.Upload(bookViewModel.PDFFile, "bookspdf", bookViewModel.PDFPath);
                }
                if (bookViewModel.CoverFile != null)
                {
                    bookViewModel.CoverPath = $"{fileName}{Path.GetExtension(bookViewModel.CoverFile.FileName)}";
                    await _fileManagerLogic.Upload(bookViewModel.CoverFile, "bookscover", bookViewModel.CoverPath);
                }
                var updateResult = _bookService.UpdateBook(bookViewModel);
            }
            catch (Exception ex)
            {
                return Json(NotFound(new
                {
                    success = false,
                    data = ex
                }));
            }

            return Json(Ok(new
            {
                success = true,
                data = "Book updated is successful"
            }));
        }

        [HttpDelete]
        public JsonResult deleteBook(string BookId)
        {
            BookId = JsonConvert.DeserializeObject<string>(BookId);
            try
            {
                var deleteResult = _bookService.DeleteBook(Guid.Parse(BookId));
            }
            catch (Exception ex)
            {
                return Json(NotFound(new
                {
                    success = false,
                    data = ex
                }));
            }

            return Json(Ok(new
            {
                success = true,
                data = "new book create is successful"
            }));
        }

        [HttpGet]
        public ActionResult GetCartList()
        {
            var list = base.GetCartItems();
            //set promotion for each book (item.CartVM.Book.BusinessPrice)
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
            //var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
            //SetLibraryCapacity(_searchResultVM.BooksList, FreeBooksList, AllFreePromotions, userId);
            foreach (var item in list)
            {
                FillBookVMAdditionalInfos.SetPromotionOfBooks(new List<BookViewModel> { item.CartVM.Book }, DiscountedBooksList, FreeBooksList);
            }
            return PartialView("../MyAccount/CartListPartialView", list);
        }


        [HttpGet]
        public JsonResult GetBookCartNumber()
        {
            var _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;

            List<CartViewModel> list = new List<CartViewModel>();
            if (HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
            {
                list = base.GetCartItems();
            }
            return Json(new
            {
                count = list.Count
            });
        }

        [HttpGet]
        public async Task<IActionResult> NewBooks(DDD.Application.Specifications.PagedBooks pagedBooks = null)
        {
            try
            {
                if (pagedBooks == null) pagedBooks = new DDD.Application.Specifications.PagedBooks();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var allCategories = base.GetAllCategories().ToList();
                var allAuthors = await _authorService.GetUsedAuthors();
                var allLanguages = await _languageService.GetUsedLanguages();
                var allEditors = base.GetAllEditors().ToList();
                var allCountries = base.GetAllCountries().ToList();

                SearchResultVM _searchResultVM = new SearchResultVM();
                _searchResultVM.BooksList = (List<BookViewModel>)await _bookService.GetPagedBooks(pagedBooks);

                var languages = _searchResultVM.BooksList.Select(b => b.LanguageId).Distinct().ToList();
                var authors = _searchResultVM.BooksList.Select(b => b.AuthorId).Distinct().ToList();
                var categories = _searchResultVM.BooksList.Select(b => b.CategoryId).Distinct().ToList();

                _searchResultVM.CategoriesList = allCategories.Distinct();
                _searchResultVM.AuthorsList = allAuthors;
                _searchResultVM.LanguagesList = allLanguages;

                var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
                var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
                var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
                var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
                var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
                var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
                _SetLibraryCapacity(_searchResultVM.BooksList, FreeBooksList, AllFreePromotions, userId);

                FillBookVMAdditionalInfos.FillBookVMList(_searchResultVM.BooksList, _searchResultVM.AuthorsList.ToList(), allEditors, _searchResultVM.CategoriesList.ToList(),
                    allCountries, _searchResultVM.LanguagesList.ToList(), null, _favoriteBooks, _cartItems, _libraryItems,
                    DiscountedBooksList, FreeBooksList);

                return PartialView(_searchResultVM);
            }
            catch (Exception ex)
            {
                return Json(NotFound(new
                {
                    success = false,
                    data = ex
                }));
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoadNewBookList(int currentPageIndex)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var allCategories = base.GetAllCategories().ToList();
                var allAuthors = base.GetAllAuthorVM().ToList();
                var allLanguages = base.GetAllLanguages().ToList();
                var allEditors = base.GetAllEditors().ToList();
                var allCountries = base.GetAllCountries().ToList();

                SearchResultVM _searchResultVM = new SearchResultVM();
                _searchResultVM.BooksList = _bookService.GetNewBooks(currentPageIndex * PAGINATION_TAKE, PAGINATION_TAKE).ToList();
                var languages = _searchResultVM.BooksList.Select(b => b.LanguageId).Distinct().ToList();
                var authors = _searchResultVM.BooksList.Select(b => b.AuthorId).Distinct().ToList();
                var categories = _searchResultVM.BooksList.Select(b => b.CategoryId).Distinct().ToList();

                _searchResultVM.CategoriesList = allCategories.Distinct();
                _searchResultVM.AuthorsList = allAuthors.Distinct();
                _searchResultVM.LanguagesList = allLanguages.Distinct();//.GroupBy(x => x.Name).Select(y => y.First());

                var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
                var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
                var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
                var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
                var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
                var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
                _SetLibraryCapacity(_searchResultVM.BooksList, FreeBooksList, AllFreePromotions, userId);

                FillBookVMAdditionalInfos.FillBookVMList(_searchResultVM.BooksList, _searchResultVM.AuthorsList.ToList(), allEditors, _searchResultVM.CategoriesList.ToList(),
                    allCountries, _searchResultVM.LanguagesList.ToList(), null, _favoriteBooks, _cartItems, _libraryItems,
                    DiscountedBooksList, FreeBooksList);

                var booksGridView = await RenderPartialViewToString("../Home/GridViewSearchResult", _searchResultVM.BooksList);
                var booksListView = await RenderPartialViewToString("../Home/ListViewSearchResult", _searchResultVM.BooksList);

                if (_searchResultVM.BooksList.Count() > 0)
                {
                    return Json(new
                    {
                        Status = 1,
                        BooksCount = _searchResultVM.BooksList.Count(),
                        Authors = _searchResultVM.AuthorsList,
                        Categories = _searchResultVM.CategoriesList,
                        Languages = _searchResultVM.LanguagesList,
                        BooksGridView = booksGridView,
                        BooksListView = booksListView
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    data = ex
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> FiltredBookList([FromBody] DDD.Application.Specifications.PagedBooks pagedBooks)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var allCategories = base.GetAllCategories().ToList();
                var allAuthors = base.GetAllAuthorVM().ToList();
                var allLanguages = base.GetAllLanguages().ToList();
                var allEditors = base.GetAllEditors().ToList();
                var allCountries = base.GetAllCountries().ToList();

                SearchResultVM _searchResultVM = new SearchResultVM();
                _searchResultVM.BooksList = (List<BookViewModel>)await _bookService.GetPagedBooks(pagedBooks);
                var languages = _searchResultVM.BooksList.Select(b => b.LanguageId).Distinct().ToList();
                var authors = _searchResultVM.BooksList.Select(b => b.AuthorId).Distinct().ToList();
                var categories = _searchResultVM.BooksList.Select(b => b.CategoryId).Distinct().ToList();

                _searchResultVM.CategoriesList = allCategories.Distinct();
                _searchResultVM.AuthorsList = allAuthors.Where(c => authors.Contains(c.Id)).ToList();
                _searchResultVM.LanguagesList = allLanguages.Where(c => languages.Contains(c.Id));

                var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
                var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
                var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
                var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
                var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
                var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
                _SetLibraryCapacity(_searchResultVM.BooksList, FreeBooksList, AllFreePromotions, userId);

                FillBookVMAdditionalInfos.FillBookVMList(_searchResultVM.BooksList, _searchResultVM.AuthorsList.ToList(), allEditors, _searchResultVM.CategoriesList.ToList(),
                    allCountries, _searchResultVM.LanguagesList.ToList(), null, _favoriteBooks, _cartItems, _libraryItems,
                    DiscountedBooksList, FreeBooksList);

                var booksGridView = await RenderPartialViewToString("../Home/GridViewSearchResult", _searchResultVM.BooksList);
                var booksListView = await RenderPartialViewToString("../Home/ListViewSearchResult", _searchResultVM.BooksList);

                if (_searchResultVM.BooksList.Count() > 0)
                {
                    return Json(new
                    {
                        Status = 1,
                        BooksCount = _searchResultVM.BooksList.Count(),
                        Authors = _searchResultVM.AuthorsList,
                        Categories = _searchResultVM.CategoriesList,
                        Languages = _searchResultVM.LanguagesList,
                        BooksGridView = booksGridView,
                        BooksListView = booksListView
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    data = ex
                });
            }
        }

        [HttpGet]
        public IActionResult GetSingleBook(string bookId)
        {
            BookDetailsVM bookDetailsVM = new BookDetailsVM();

            bookDetailsVM.Book = _bookService.GetBookById(Guid.Parse(bookId));

            var allEditors = base.GetAllEditors().ToList();
            var allAuthors = base.GetAllAuthorVM().ToList();
            var allCategories = base.GetAllCategories().ToList();
            var allLanguages = base.GetAllLanguages().ToList();
            var allCountries = base.GetAllCountries().ToList();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
            var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
            var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            var FreeBooksList = _promotionService.GetFreeBooks(0, 30);

            //check if in library
            var library = _libraryService.GetLibraryByUserIdAndBookId(userId, bookDetailsVM.Book.Id).FirstOrDefault();
            if (library != null)
            {
                bookDetailsVM.Book.inLibrary = !(library.Id == Guid.Empty);
                bookDetailsVM.Book.LibraryId = !(library.Id == Guid.Empty) ? library.Id.ToString() : "-1";
            }
            FillBookVMAdditionalInfos.FillBookVMModel(bookDetailsVM.Book, allAuthors, allEditors, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
            return PartialView("PartialSingleBook", bookDetailsVM.Book);
        }

        #region privateMethods
        private async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.FindView(ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
        private void _SetLibraryCapacity(List<BookViewModel> bookList, List<BookViewModel> freeBooksList,
            List<PromotionVM> freePromotions, string userId)
        {
            if (freeBooksList.Any())
            {
                var FreeBooks = bookList.Where(b => freeBooksList.Any(x => x.Id == b.Id));
                if (FreeBooks.Any())
                {
                    FreeBooks.ForEach(b => { b.PromotionsPercentage = 100; });
                }
                foreach (var promo in freePromotions)
                {
                    var promoUser = _promoUserService.GetPromoUserByUserIdAndPromoId(userId, promo.Id).FirstOrDefault();

                    //var promotionBooks = promo.PromotionBook.Where(x => x.BookId == book.Id);
                    var promotionBooks = bookList.Where(b => promo.PromotionBook.Any(x => x.BookId == b.Id));
                    if (promotionBooks.Any())
                    {
                        promotionBooks.ForEach(b =>
                        {
                            if (promoUser == null)
                                b.CanBeAddedToLib = true;
                            else
                                b.CanBeAddedToLib = promoUser.BooksTakenCount < promo.MaxFreeBooks;
                            if(b.CanBeAddedToLib && promo.PromotionType==PromotionType.Free) b.PromotionsPercentage = 100;
                            b.PromotionId = promo.Id;
                        });
                        //book.PromotionId = promo.Id;
                        //get PromoUser by promoId and userId

                    }
                }
            }
            foreach (var book in bookList)
            {
                if (freeBooksList != null)
                {
                    if (freeBooksList.Count > 0)
                    {
                        var FreeBook = freeBooksList.Find(x => x.Id == book.Id);
                        book.PromotionsPercentage = FreeBook != null ? 100 : book.PromotionsPercentage;
                        //set attribute CanBeAddedToLib of this free book (bookVM)
                        //if the user haven't reached the max of the promotion (PromoUserVM)
                        //get the MaxFree of this promotion (promotionVM)
                        foreach (var promo in freePromotions)
                        {
                            var promotionBook = promo.PromotionBook.Where(x => x.BookId == book.Id);
                            if (promotionBook == null) continue;
                            book.PromotionId = promo.Id;
                            //get PromoUser by promoId and userId
                            var promoUser = _promoUserService.GetPromoUserByUserIdAndPromoId(userId, promo.Id).FirstOrDefault();
                            if (promoUser == null)
                                book.CanBeAddedToLib = true;
                            else
                                book.CanBeAddedToLib = promoUser.BooksTakenCount < promo.MaxFreeBooks;
                        }
                    }
                }
            }
        }
        private void SetLibraryCapacity(List<BookViewModel> bookList, List<BookViewModel> freeBooksList,
            List<PromotionVM> freePromotions, string userId)
        {
            foreach (var book in bookList)
            {
                if (freeBooksList != null)
                {
                    if (freeBooksList.Count > 0)
                    {
                        var FreeBook = freeBooksList.Find(x => x.Id == book.Id);
                        book.PromotionsPercentage = FreeBook != null ? 100 : book.PromotionsPercentage;
                        //set attribute CanBeAddedToLib of this free book (bookVM)
                        //if the user haven't reached the max of the promotion (PromoUserVM)
                        //get the MaxFree of this promotion (promotionVM)
                        foreach (var promo in freePromotions)
                        {
                            var promotionBook = promo.PromotionBook.Where(x => x.BookId == book.Id);
                            if (promotionBook == null) continue;
                            book.PromotionId = promo.Id;
                            //get PromoUser by promoId and userId
                            var promoUser = _promoUserService.GetPromoUserByUserIdAndPromoId(userId, promo.Id).FirstOrDefault();
                            if (promoUser == null)
                                book.CanBeAddedToLib = true;
                            else
                                book.CanBeAddedToLib = promoUser.BooksTakenCount < promo.MaxFreeBooks;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
