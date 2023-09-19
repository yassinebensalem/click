using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DDD.Application.ILogics;
using DDD.Application.Interfaces;

using DDD.Application.Services;
using DDD.Application.Specifications;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Models;
using DDD.Infra.CrossCutting.Identity.Models.AccountViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MipLivrelStore.Helper;
using static DDD.Application.Enum.Constants;

namespace MipLivrelStore.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IJoinRequestService _joinRequestService;
        private readonly IFileManagerLogic _fileManagerLogic;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IBookService _bookService;
        private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;
        private readonly IConfiguration _configuration;
        private const int PAGINATION_TAKE = 12;
        private readonly IFavoriteService _IFavoriteService;
        private readonly ICartService _cartService;
        private readonly ILibraryService _libraryService;
        private readonly IPromoUserService _promoUserService;
        private readonly IPromotionService _promotionService;
        private ICompositeViewEngine _viewEngine;

        public CategoryController(
            UserManager<ApplicationUser> userManager,
            IJoinRequestService joinRequestService,
        IFileManagerLogic fileManagerLogic,
        IMemoryCache memoryCache,
           IBookService bookService,
           IStringLocalizer<CategoryController> stringLocalizer, IStringLocalizer<SharedResource> sharedLocalizer,
           ICategoryService categoryService,
           ICompositeViewEngine viewEngine,
           ILoggerFactory loggerFactory, ICartService cartService,
           INotificationHandler<DomainNotification> notifications, IFavoriteService IFavoriteService,
           IMediatorHandler mediator, ICountryService countryService, ILanguageService languageService, IAuthorService authorService,
        IMapper mapper, ILibraryService libraryService, IPromotionService promotionService,
        IConfiguration configuration, IPrizeService prizeService, IPromoUserService promoUserService
        )
           : base(joinRequestService, fileManagerLogic, memoryCache, userManager, loggerFactory, notifications, mediator, bookService,
               categoryService, mapper, promotionService, countryService, languageService, cartService, authorService, prizeService, libraryService)

        {
            _joinRequestService = joinRequestService;
            _fileManagerLogic = fileManagerLogic;
            _categoryService = categoryService;
            _userManager = userManager;
            //_logger = logger;
            _bookService = bookService;
            _countryService = countryService;
            _languageService = languageService;
            _authorService = authorService;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _mediator = mediator;
            _IFavoriteService = IFavoriteService;
            _cartService = cartService;
            _configuration = configuration;
            _viewEngine = viewEngine;
            _libraryService = libraryService;
            _promotionService = promotionService;
            _promoUserService = promoUserService;
        }

        [Route("Categories/{CategoryName}")]
        [HttpGet]
        public async Task<IActionResult> Index(string CategoryName)
        {
            var allAuthors = await _authorService.GetUsedAuthors();
            var allLanguages = await _languageService.GetUsedLanguages();
            var allEditors = base.GetAllEditors().ToList();
            var allCountries = base.GetAllCountries().ToList();
            var allCategories = base.GetAllCategories().ToList();
            CategoryDetailsVM categoryDetailsVM = new CategoryDetailsVM();

            switch (CategoryName)
            {
                case "TunisianEditions":
                    {
                        categoryDetailsVM.BooksList = _bookService.GetTunisianBook(categoryDetailsVM.currentPageIndex * PAGINATION_TAKE, PAGINATION_TAKE).ToList();

                        break;
                    }
                case "ForeignEditions":
                    {
                        categoryDetailsVM.BooksList = _bookService.GetForeignBook(categoryDetailsVM.currentPageIndex * PAGINATION_TAKE, PAGINATION_TAKE).ToList();
                        break;
                    }
                default:
                    {
                        //categoryDetailsVM.BooksList =(List<DDD.Application.ViewModels.BookViewModel>)await _bookService.GetPagedBooks(pagedBooks);
                        categoryDetailsVM.BooksList = _bookService.GetBookByCategory(CategoryName, categoryDetailsVM.currentPageIndex * PAGINATION_TAKE, PAGINATION_TAKE).ToList();
                        break;
                    }
            }
            categoryDetailsVM.CurrentCategory = getCategory(CategoryName);

            var languages = categoryDetailsVM.BooksList.Select(b => b.LanguageId).Distinct().ToList();
            var authors = categoryDetailsVM.BooksList.Select(b => b.AuthorId).Distinct().ToList();
            var categories = categoryDetailsVM.BooksList.Select(b => b.CategoryId).Distinct().ToList();

            categoryDetailsVM.CategoriesList =allCategories.Distinct().ToList();
            categoryDetailsVM.AuthorsList = (List<AuthorVM>)allAuthors;
            categoryDetailsVM.LanguagesList = allLanguages;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
            var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
            var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
            var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
            SetLibraryCapacity(categoryDetailsVM.BooksList, FreeBooksList, AllFreePromotions, userId);

            FillBookVMAdditionalInfos.FillBookVMList(categoryDetailsVM.BooksList,
                categoryDetailsVM.AuthorsList, allEditors, categoryDetailsVM.CategoriesList, allCountries,
                categoryDetailsVM.LanguagesList.ToList(), null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

            return View("CategoryDetails", categoryDetailsVM);
        }

        private string getCategory(string currentCategory)
        {
            var categoryList = _categoryService.GetAllUsedCategories().ToList();
            foreach (var category in categoryList)
            {
                Dictionary<string, string> CategoryNames = JsonSerializer.Deserialize<Dictionary<string, string>>(category.CategoryName);

                var cats = CategoryNames.Values.ToList();
                if (cats.Contains(currentCategory))
                {
                    var catz = CategoryNames.Where(x => x.Key == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName).ToList()[0].Value;
                    currentCategory = catz;
                    break;
                }
            }

            return currentCategory;
        }

        [Route("Categories/LoadCategoryBookList")]
        [HttpPost]
        public async Task<IActionResult> LoadCategoryBookList([FromBody] PagedBooks pagedBooks)
        {

                var allEditors = base.GetAllEditors().ToList();
                var allAuthors = base.GetAllAuthorVM().ToList();
                //var allAuthors = base.GetAllAuthors().ToList();
                var allCategories = base.GetAllCategories().ToList();
                var allLanguages = base.GetAllLanguages().ToList();
                var allCountries = base.GetAllCountries().ToList();
                CategoryDetailsVM categoryDetailsVM = new CategoryDetailsVM();
               
                categoryDetailsVM.BooksList =(List<BookViewModel>) await _bookService.GetPagedBooks(pagedBooks);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
                var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
                var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
                var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
                var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
                var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
                SetLibraryCapacity(categoryDetailsVM.BooksList, FreeBooksList, AllFreePromotions, userId);

                FillBookVMAdditionalInfos.FillBookVMList(categoryDetailsVM.BooksList, allAuthors, allEditors, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

                var categories = categoryDetailsVM.BooksList.Select(b => b.CategoryId);
                var languages = categoryDetailsVM.BooksList.Select(b => b.LanguageId);
                var authors = categoryDetailsVM.BooksList.Select(b => b.AuthorId).Distinct();

                categoryDetailsVM.CategoriesList = allCategories.Distinct().Where(c => categories.Contains(c.Id)).ToList();
                categoryDetailsVM.LanguagesList = allLanguages.Distinct().Where(c => languages.Contains(c.Id)).GroupBy(x => x.Name).Select(y => y.First());
                categoryDetailsVM.AuthorsList = allAuthors.Where(c => authors.Contains(c.Id)).ToList();
                categoryDetailsVM.CurrentCategory = categoryDetailsVM.KeyWord;

                var booksGridView = await RenderPartialViewToString("../Home/GridViewSearchResult", categoryDetailsVM.BooksList);
                var booksListView = await RenderPartialViewToString("../Home/ListViewSearchResult", categoryDetailsVM.BooksList);

                if (categoryDetailsVM.BooksList.Count() > 0)
                {
                    return Json(new
                    {
                        Status = 1,
                        BooksCount = categoryDetailsVM.BooksList.Count(),
                        Authors = categoryDetailsVM.AuthorsList,
                        Categories = categoryDetailsVM.CategoriesList,
                        Languages = categoryDetailsVM.LanguagesList,
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

        public int PageCount(string CategoryName)
        {
            var ConnString = _configuration.GetConnectionString("DefaultConnection");
            string SqlString = "";
            //string parameterName = "categoryName";
            SqlString = "Select count(b.Id) as Numb From Books b,Categories c" +
                                     " Where c.CategoryName LIKE '%' + @categoryName + '%' and c.Id = b.CategoryId ";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(SqlString, conn))
                {
                    cmd.Parameters.AddWithValue("categoryName", CategoryName);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return (int)reader["Numb"];
                    }
                }
            }
            return 1;
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

        #endregion
    }
}
