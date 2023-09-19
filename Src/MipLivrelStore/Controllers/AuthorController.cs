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
using DDD.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MipLivrelStore.Helper;

namespace MipLivrelStore.Controllers
{
    public class AuthorController : BaseController
    {
        private readonly IJoinRequestService _joinRequestService;
        private readonly IFileManagerLogic _fileManagerLogic;
        private readonly IAuthorService _authorService;
        private readonly ILogger _logger;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;
        private const int PAGINATION_TAKE = 12;
        private readonly IPromoUserService _promoUserService;
        private readonly IPromotionService _promotionService;
        private readonly IFavoriteService _IFavoriteService;
        private readonly ICartService _cartService;
        private readonly ILibraryService _libraryService;


        public AuthorController(
        IFileManagerLogic fileManagerLogic,
         IJoinRequestService joinRequestService,
    IMemoryCache memoryCache,
       UserManager<ApplicationUser> userManager,
       ILoggerFactory loggerFactory,
       INotificationHandler<DomainNotification> notifications,
         IMediatorHandler mediator,
         IBookService bookService,
         ICategoryService categoryService,
         IMapper mapper, ILibraryService libraryService,
         ICountryService countryService, IPromoUserService promoUserService, IPromotionService promotionService,
         ILanguageService languageService, IAuthorService authorService, ICartService cartService, IFavoriteService IFavoriteService, IPrizeService prizeService)

        : base(joinRequestService, fileManagerLogic, memoryCache, userManager, loggerFactory, notifications,
          mediator, bookService, categoryService, mapper, promotionService, countryService, languageService, cartService, authorService, prizeService, libraryService)

        {
            _joinRequestService = joinRequestService;
            _fileManagerLogic = fileManagerLogic;
            _memoryCache = memoryCache;

            _logger = loggerFactory.CreateLogger<AuthorController>();
            _categoryService = categoryService;
            _mapper = mapper;
            _bookService = bookService;
            _countryService = countryService;
            _languageService = languageService;
            _authorService = authorService;
            _IFavoriteService = IFavoriteService;
            _cartService = cartService;
            _libraryService = libraryService;
            _promoUserService = promoUserService;
            _promotionService = promotionService;
        }
        //   [Route("{authorInfos}")]
        [HttpGet]
        public IActionResult GetAuthorDetails(string authorInfos)
        { 
            var authorId = authorInfos.Split("#")[0];
            AuthorVM authorVM = new AuthorVM();
            authorVM.BooksList = new List<BookViewModel>();
            var idauth = Guid.Parse(authorId);
            var authorModel = _authorService.GetAuthorById(idauth);
            //authorModel.AuthorName = $"{authorModel.FirstName}{authorModel.LastName}";
            authorModel.BirthdateAsString = authorModel.Birthdate.ToShortDateString();

            //authorModel.BooksList = _bookService.GetBooksByAuthorId(idauth, 0, 10).ToList();
            authorModel.BooksList = _bookService.GetBooksByAuthorId_WithoutPagination(idauth).ToList();

            var allEditors = base.GetAllEditors().ToList();
            //var allAuthors = base.GetAllAuthors().ToList();
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
            SetLibraryCapacity(authorModel.BooksList, FreeBooksList, AllFreePromotions, userId);

            authorModel.BooksList = FillBookVMAdditionalInfos.FillBookVMList(authorModel.BooksList, allAuthors, allEditors, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

            return View(authorModel);
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
