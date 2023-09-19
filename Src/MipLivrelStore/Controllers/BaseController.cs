using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using AutoMapper;
using DDD.Application.ILogics;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MipLivrelStore.Helper;
using MipLivrelStore.Models;
using static DDD.Application.Enum.Constants;

namespace MipLivrelStore.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BaseController : Controller
    {

        private readonly IJoinRequestService _joinRequestService;
        private readonly IFileManagerLogic _fileManagerLogic;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;
        private readonly ICartService _cartService;
        private readonly IAuthorService _authorService;
        private readonly IPrizeService _prizeService;
        private readonly ILibraryService _libraryService;
        private readonly IPromotionService _promotionService;


        protected BaseController(

            IJoinRequestService joinRequestService,
        IFileManagerLogic fileManagerLogic,
           IMemoryCache memoryCache,
           UserManager<ApplicationUser> userManager,
           ILoggerFactory loggerFactory,
           INotificationHandler<DomainNotification> notifications,
           IMediatorHandler mediator,
           IBookService bookService,
           ICategoryService categoryService,
           IMapper mapper, IPromotionService promotionService,
           ICountryService countryService,
           ILanguageService languageService,
           ICartService cartService,
           IAuthorService authorService,
            IPrizeService prizeService,
            ILibraryService  libraryService)


        {

            _joinRequestService = joinRequestService;
            _fileManagerLogic = fileManagerLogic;
            _memoryCache = memoryCache;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<MyAccountController>();
            _notifications = (DomainNotificationHandler)notifications;
            _mediator = mediator;
            _categoryService = categoryService;
            _mapper = mapper;
            _bookService = bookService;
            _countryService = countryService;
            _languageService = languageService;
            _fileManagerLogic = fileManagerLogic;
            _cartService = cartService;
            _authorService = authorService;
            _prizeService = prizeService;
            _libraryService = libraryService;
            _promotionService = promotionService;
        }

        protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();

        protected bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }

        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifications.GetNotifications().Select(n => n.Value)
            });
        }

        protected void NotifyModelStateErrors()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.RaiseEvent(new DomainNotification(code, message));
        }

        protected void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(result.ToString(), error.Description);
            }
        }

        #region Helpers

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        protected IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        protected IActionResult GetUsersByRole(string role)
        {
            return Response(new { UsersList = _userManager.GetUsersInRoleAsync(role) });
        }

        protected void RemoveCategoriesFromCache()
        {
            _memoryCache.Remove(Constants.CategoriesCacheKey);
        }

        protected IEnumerable<CategoryViewModel> GetAllCategories()
        {
            if (!_memoryCache.TryGetValue(Constants.CategoriesCacheKey, out IEnumerable<CategoryViewModel> categories))
            {
                //calling the server
                //var categoryList = _categoryService.GetAll().Where(cat => cat.Status != false).ToList();
                var categoryList = _categoryService.GetAllUsedCategories().ToList();
                foreach (var category in categoryList)
                {
                    Dictionary<string, string> CategoryNames = JsonSerializer.Deserialize<Dictionary<string, string>>(category.CategoryName);
                    var cat = CategoryNames.Where(x => x.Key == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName).ToList()[0].Value;
                    category.CategoryName = cat.Trim();
                }
                categories = categoryList;

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(2),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                };
                //setting cache entries
                _memoryCache.Set(Constants.CategoriesCacheKey, categories, cacheExpiryOptions);
            }
            return categories.OrderBy(x => x.CategoryName);
        }

        protected IEnumerable<PrizeBookVM> GetAllPrizeBook()
        {
            if (!_memoryCache.TryGetValue(Constants.PrizebookCacheKey, out IEnumerable<PrizeBookVM> PrizeBook))
            {
                //calling the server

                PrizeBook = _prizeService.GetAllPrizeBook().ToList();
                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };
                //setting cache entries
                _memoryCache.Set(Constants.PrizebookCacheKey, PrizeBook, cacheExpiryOptions);
            }
            return PrizeBook;
        }

        protected IList<Author> GetAllAuthors()
        {
            if (!_memoryCache.TryGetValue(Constants.AuthorsCacheKey, out IList<Author> authors))
            {
                //calling the server
                authors = _authorService.GetAll().ToList();//new List<Author>();//_userManager.GetUsersInRoleAsync(UserRoleVM.Author.ToString()).Result.ToList();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(2),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                };

                if (authors.Count > 0)
                {
                    _memoryCache.Set(Constants.AuthorsCacheKey, authors, cacheExpiryOptions);
                }
                //setting cache entries

            }
            return authors;
        }
        
        protected IList<AuthorVM> GetAllAuthorVM()
        {
            if (!_memoryCache.TryGetValue(Constants.AuthorsCacheKey, out IList<AuthorVM> authors))
            {
                //calling the server
                authors = _authorService.GetAllAsVM().ToList();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(2),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                };

                if (authors.Count > 0)
                {
                    _memoryCache.Set(Constants.AuthorsCacheKey, authors, cacheExpiryOptions);
                }
                //setting cache entries

            }
            return authors;
        }

        protected IList<ApplicationUser> GetAllEditors()
        {
            if (!_memoryCache.TryGetValue(Constants.EditorsCacheKey, out IList<ApplicationUser> editors))
            {
                //calling the server
                editors = _userManager.GetUsersInRoleAsync(UserRoleVM.Editor.ToString()).Result.Where(e => e.isActive).ToList();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(3),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                };

                if (editors.Count > 0)
                {
                    _memoryCache.Set(Constants.EditorsCacheKey, editors, cacheExpiryOptions);
                }
                //setting cache entries

            }
            return editors;
        }

        protected IEnumerable<PrizeVM> GetAllPrized()
        {
            if (!_memoryCache.TryGetValue(Constants.PrizedCacheKey, out IEnumerable<PrizeVM> Prized))
            {
                //calling the server
                Prized = _prizeService.GetAll().ToList();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                if (Prized.Any())
                {
                    _memoryCache.Set(Constants.PrizedCacheKey, Prized, cacheExpiryOptions);
                }
                //setting cache entries

            }
            return Prized;
        }


        protected IEnumerable<LanguageViewModel> GetAllLanguages()
        {
            if (!_memoryCache.TryGetValue(Constants.LanguagesCacheKey, out IEnumerable<LanguageViewModel> languages))
            {
                //calling the server
                languages = _languageService.GetAll().ToList();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(20),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(15)
                };

                if (languages.Any())
                {
                    _memoryCache.Set(Constants.LanguagesCacheKey, languages, cacheExpiryOptions);
                }
                //setting cache entries

            }
            return languages;
        }
        protected IEnumerable<CountryViewModel> GetAllCountries()
        {
            if (!_memoryCache.TryGetValue(Constants.CountriesCacheKey, out IEnumerable<CountryViewModel> countries))
            {
                //calling the server
                countries = _countryService.GetAll().ToList();

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(20),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(15)
                };

                if (countries.Any())
                {
                    _memoryCache.Set(Constants.CountriesCacheKey, countries, cacheExpiryOptions);
                }
                //setting cache entries

            }
            return countries;
        }

        protected List<CartViewModel> GetCartItems()
        {
            if (!_memoryCache.TryGetValue(Constants.CartCacheKey, out List<CartViewModel> CartList))
            {
                CartList = SetCartCacheableItems();
                //setting cache entries(IEnumerable<CartViewModel>)
            }
            return CartList;
        }

        protected List<CartViewModel> GetCartItemsFromCache()
        {
            if (_memoryCache.TryGetValue(Constants.CartCacheKey, out List<CartViewModel> CartList))
            {
                return CartList;
            }

            return new List<CartViewModel>();
        }

        protected List<CartViewModel> UpdateCartItems()
        {
            List<CartViewModel> CartList = new List<CartViewModel>();
            CartList = SetCartCacheableItems();
            //setting cache entries(IEnumerable<CartViewModel>)
            return CartList;
        }

        protected List<CartViewModel> SetCartCacheableItems()
        {
            List<CartViewModel> CartList = new List<CartViewModel>();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _cartItems = _cartService.GetCartsByUserId(userId);
            //var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            //var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            //var FreeBooksList = _promotionService.GetFreeBooks(0, 30); var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList(); SetLibraryCapacity(_searchResultVM.BooksList, FreeBooksList, AllFreePromotions, userId);

            CartList = new List<CartViewModel>();
            var allAuthors = GetAllAuthorVM().ToList();
            //var allAuthors = GetAllAuthors().ToList();
            var allEditors = GetAllEditors().ToList();
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            foreach (var item in _cartItems)
            {
                var _CartViewModel = new CartViewModel();
                _CartViewModel.CartVM = item;
                FillBookVMAdditionalInfos.FillBookVMModel(_CartViewModel.CartVM.Book, allAuthors, allEditors, null, null, null, null, null, null,null, DiscountedBooksList);
                CartList.Add(_CartViewModel);
            }
            //setting up cache options
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(1),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromSeconds(1)
            };

            if (CartList.Any())
            {
                _memoryCache.Set(Constants.CartCacheKey, CartList, cacheExpiryOptions);
            }

            return CartList;
        }

        protected KeyValuePair<string, string> GetPaymentInfos()
        {
            _memoryCache.TryGetValue(Constants.PaymentCacheKey, out KeyValuePair<string, string> paymentInfos);

            return paymentInfos;
        }

        protected void AddPaymentInfosCacheableItems(KeyValuePair<string, string> keyValuePair)
        {

            //setting up cache options
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(Constants.PaymentCacheKey, keyValuePair, cacheExpiryOptions);
        }

        protected void SetResultIndicatorMPGS(string resultIndicator)
        {

            //setting up cache options
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(Constants.MPGSPaymentCacheKey, resultIndicator);
        }
        protected string GetResultIndicatorMPGS()
        {
            _memoryCache.TryGetValue(Constants.MPGSPaymentCacheKey, out string resultIndicator);

            return resultIndicator;
        }
        protected void SetOrderNumberMPGS(string orderNumber)
        {

            //setting up cache options
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(Constants.MPGSOrderNumberCacheKey, orderNumber);
        }
        protected string GetOrderNumberMPGS()
        {
            _memoryCache.TryGetValue(Constants.MPGSOrderNumberCacheKey, out string orderNumber);

            return orderNumber;
        }
        protected void SetOrderAmountMPGS(string orderAmount)
        {

            //setting up cache options
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            _memoryCache.Set(Constants.MPGSOrderAmountCacheKey, orderAmount);
        }
        protected string GetOrderAmountMPGS()
        {
            _memoryCache.TryGetValue(Constants.MPGSOrderAmountCacheKey, out string orderAmount);

            return orderAmount;
        }
        #endregion
    }
}
