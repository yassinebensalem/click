using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DDD.Application.ILogics;
using DDD.Application.Interfaces;
using DDD.Application.Services;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.CrossCutting.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MipLivrelStore.Helper;

namespace MipLivrelStore.Controllers
{
    [Route("[controller]/[action]")]
    public class PrizedController : BaseController
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;
        private readonly IPrizeService _prizeService;
        private readonly IBookService _bookService;
        private readonly IFavoriteService _IFavoriteService;
        private readonly ICartService _cartService;
        private readonly IAuthorService _authorService;
        private const int PAGINATION_TAKE = 12;
        private readonly ICategoryService _categoryService;
        private readonly ILibraryService _libraryService;
        private readonly IPromotionService _promotionService;
        private readonly IPromoUserService _promoUserService;

        public PrizedController(IFileManagerLogic fileManagerLogic,
           IJoinRequestService joinRequestService,
          IMemoryCache memoryCache,
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           IEmailSender emailSender,
           IUser user,
           ILoggerFactory loggerFactory,
           INotificationHandler<DomainNotification> notifications, IPromotionService promotionService, IPromoUserService promoUserService,
           IMediatorHandler mediator, IBookService bookService, ICategoryService categoryService, ICartService cartService,
           ICountryService countryService, ILanguageService languageService, ILibraryService  libraryService,
           IMapper mapper, IAuthorService authorService, IPrizeService prizeService,
           IFavoriteService IFavoriteService)
            : base(joinRequestService, fileManagerLogic, memoryCache, userManager, loggerFactory, notifications, mediator, bookService,
               categoryService, mapper, promotionService, countryService, languageService, cartService, authorService, prizeService, libraryService)
        {
            _memoryCache = memoryCache;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<PrizedController>();
            _mediator = mediator;
            _prizeService = prizeService;
            _bookService = bookService;
            _IFavoriteService = IFavoriteService;
            _cartService = cartService;
            _authorService = authorService;
            _categoryService = categoryService;
            _libraryService = libraryService;
            _promotionService = promotionService;
            _promoUserService = promoUserService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var list = _prizeService.GetAll().ToList();
                return View("AwardsList", list);
            }
            catch (Exception)
            {
                return Response();
            }
        }

        [Route("{Id}")]
        [HttpGet]
        public IActionResult Details(string Id) //details/id
        {
            var allPrize = base.GetAllPrized().ToList();
            var allPrizebook = base.GetAllPrizeBook().ToList();
            var allEditors = base.GetAllEditors().ToList();
            var allAuthors = base.GetAllAuthorVM().ToList();
            //var allAuthors = base.GetAllAuthors().ToList();
            var allCategories = base.GetAllCategories().ToList();
            var allLanguages = base.GetAllLanguages().ToList();
            var allCountries = base.GetAllCountries().ToList();

            var PrizeDetails = new PrizeDetailsVM();
            PrizeDetails.Prize = allPrize.Find(p => p.Id == Guid.Parse(Id));
            var PrizeBooksModel = allPrizebook.Find(p => p.PrizeId == Guid.Parse(Id));
            var list = allPrizebook.Where(x => x.PrizeId == Guid.Parse(Id));

            //PrizeDetails.EditionYears
            PrizeDetails.EditionYears = list.Select(x => x.Edition).Distinct().ToList();
            PrizeDetails.CurrentEdition = PrizeDetails.EditionYears.Max();
            //PrizeDetails.EditionBooks
            PrizeDetails.EditionBooks = new List<BookViewModel>();
            var booksIds = list.Select(x => x.BookId);
            foreach (var id in booksIds)
            {
                var book = _bookService.GetBookById(id);
                PrizeDetails.EditionBooks.Add(book);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
            var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
            var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
            var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
            SetLibraryCapacity(PrizeDetails.EditionBooks, FreeBooksList, AllFreePromotions, userId);

            FillBookVMAdditionalInfos.FillBookVMList(PrizeDetails.EditionBooks, allAuthors, allEditors, allCategories, allCountries, allLanguages, list.ToList(), _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

            //PrizeDetails.EditionCategories
            PrizeDetails.EditionCategories = new List<CategoryViewModel>();
            var categoriesIDs = PrizeDetails.EditionBooks.Select(x => x.CategoryId);
            foreach (var id in categoriesIDs)
            {
                var cat = base.GetAllCategories().Where(x => x.Id == id).FirstOrDefault();
                PrizeDetails.EditionCategories.Add(cat);
            }

            //PrizeDetails.EditionAuthors
            PrizeDetails.EditionAuthors = new List<AuthorVM>();
            var authorsIDs = PrizeDetails.EditionBooks.Select(x => x.AuthorId);
            foreach (var id in authorsIDs)
            {
                var auth = base.GetAllAuthorVM().Where(x => x.Id == id).FirstOrDefault();
                //var auth = base.GetAllAuthors().Where(x => x.Id == id).FirstOrDefault();
                PrizeDetails.EditionAuthors.Add(auth);
            }

            return View("AwardDetails", PrizeDetails);
        }

        [HttpGet]
        public IActionResult GetPrizedDetail(string id)
        {
            try
            {
                var author = new Author();
                var publisher = new ApplicationUser();
                PrizeBookDetailsVM bookDetailsVM = new PrizeBookDetailsVM();

                var bookId = id.Split("#")[1];
                bookDetailsVM.Book = _bookService.GetBookById(Guid.Parse(bookId));
                bookDetailsVM.RelatedBooks = _bookService.GetRelatedBooksByCategory(bookDetailsVM.Book.CategoryId, 0, 7).ToList();
                var allEditors = base.GetAllEditors().ToList();
                var allAuthors = base.GetAllAuthorVM().ToList();
                //var allAuthors = base.GetAllAuthors().ToList();
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
                SetLibraryCapacity(bookDetailsVM.RelatedBooks, FreeBooksList, AllFreePromotions, userId);

                FillBookVMAdditionalInfos.FillBookVMList(bookDetailsVM.RelatedBooks, allAuthors, allEditors, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //foreach (var bookVM in bookDetailsVM.RelatedBooks)
                //{
                //    var fav = _IFavoriteService.GetFavoriteBookByUserIdAndBookId(userId, bookVM.Id).FirstOrDefault();
                //    if (fav != null) bookVM.isFavorite = !(fav.Id == Guid.Empty);
                //    var c = _cartService.GetCartByUserIdAndBookId(userId, bookVM.Id).FirstOrDefault();
                //    if (c != null) bookVM.inCart = !(c.Id == Guid.Empty);
                //}
                FillBookVMAdditionalInfos.FillBookVMModel(bookDetailsVM.Book, allAuthors, allEditors, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

                return PartialView(bookDetailsVM);
            }
            catch (Exception)
            {
                return Response();
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
                            if (promoUser == null) continue;
                            book.CanBeAddedToLib = promoUser.BooksTakenCount < promo.MaxFreeBooks;
                        }
                    }
                }
            }
        }

    }
}
