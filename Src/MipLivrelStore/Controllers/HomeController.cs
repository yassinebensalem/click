using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Castle.Core.Smtp;
using DDD.Application.ILogics;
using DDD.Application.Interfaces;
using DDD.Application.Services;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Models;
using DDD.Infra.CrossCutting.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
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
using Newtonsoft.Json.Linq;
using static DDD.Application.Enum.Constants;
using DDD.Infra.CrossCutting.Identity.Extensions;
using IEmailSender = DDD.Infra.CrossCutting.Identity.Services.IEmailSender;
using DDD.Domain.Interfaces;
using DDD.Application.Specifications;
using iTextSharp.text;

namespace MipLivrelStore.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IJoinRequestService _joinRequestService;
        private readonly IFileManagerLogic _fileManagerLogic;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private const int PAGINATION_TAKE = 12;
        SearchResultVM searchResultVM = new SearchResultVM();
        private readonly IStringLocalizer<HomeController> _stringLocalizer;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private ICompositeViewEngine _viewEngine;
        private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;
        private readonly IMapper _mapper;
        private readonly IAuthorService _authorService;
        private readonly IConfiguration _configuration;
        private readonly IFavoriteService _IFavoriteService;
        private readonly ICartService _cartService;
        private readonly IPrizeService _prizeService;
        private readonly IPromoUserService _promoUserService;
        private readonly IPromotionService _promotionService;
        private readonly ILibraryService _libraryService;
        private readonly IEmailSender _emailSender;

        public HomeController(

            UserManager<ApplicationUser> userManager,
             IJoinRequestService joinRequestService,
            IFileManagerLogic fileManagerLogic,
             IMemoryCache memoryCache,
           IBookService bookService,
           IStringLocalizer<HomeController> stringLocalizer, IStringLocalizer<SharedResource> sharedLocalizer,
           ICategoryService categoryService,
           ICompositeViewEngine viewEngine,
           ILoggerFactory loggerFactory,
           INotificationHandler<DomainNotification> notifications,
          IFavoriteService IFavoriteService, ICartService cartService,
          IPromotionService promotionService, IPromoUserService promoUserService,
           IMediatorHandler mediator, ICountryService countryService, ILanguageService languageService, IAuthorService authorService,
           IMapper mapper, ILibraryService libraryService,
        IConfiguration configuration, IPrizeService prizeService, IEmailSender emailSender
        )
           : base(joinRequestService, fileManagerLogic, memoryCache, userManager, loggerFactory, notifications, mediator, bookService,
               categoryService, mapper, promotionService, countryService, languageService, cartService, authorService, prizeService, libraryService)
        {

            _userManager = userManager;
            _joinRequestService = joinRequestService;
            _fileManagerLogic = fileManagerLogic;
            _memoryCache = memoryCache;
            _bookService = bookService;
            _categoryService = categoryService;
            _stringLocalizer = stringLocalizer;
            _sharedLocalizer = sharedLocalizer;
            _viewEngine = viewEngine;
            _countryService = countryService;
            _languageService = languageService;
            _authorService = authorService;
            _promotionService = promotionService;
            _configuration = configuration;
            _IFavoriteService = IFavoriteService;
            _cartService = cartService;
            _prizeService = prizeService;
            _libraryService = libraryService;
            _promoUserService = promoUserService;
            _emailSender = emailSender;
        }

        [HttpGet("Subscription")]
        public IActionResult Subscription()
        {
            return PartialView();
        }
       
        [HttpPost]
        public async Task<JsonResult> Autocomplete(string Prefix)
        {
        var books= await _bookService.SearchBooksByTitle(Prefix);
            return Json(books);
        }
        public IActionResult Index()
        {
            var allPrizebook = base.GetAllPrizeBook().ToList();
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

            PrizeBookDetailsVM prizeBookDetailsVM = new();
            var author = new Author();

            IndexVM indexVM = new IndexVM();
            indexVM.PrizeBookVM = new PrizeBookVM();
            indexVM.PrizeBookVM.BookListVM = new List<PrizeDetailsVM>();
            indexVM.PrizeBookVM.AuthorsList = new List<AuthorVM>();

            var selectedCategories = allCategories.OrderBy(arg => Guid.NewGuid()).Take(2).Select(arg => arg.Id).ToList();
            indexVM.Cat1books = new List<BookViewModel>();
            indexVM.Cat2books = new List<BookViewModel>();
            indexVM.Books = new List<BookViewModel>();
            while (selectedCategories.Count>=2 && (selectedCategories[0] == selectedCategories[1] || indexVM.Cat1books.Count < 5 || indexVM.Cat2books.Count < 5))
            {
                //    selectedCategories[0] = new Random().Next(allCategories.Count());
                //    selectedCategories[1] = new Random().Next(allCategories.Count());
                selectedCategories = allCategories.OrderBy(arg => Guid.NewGuid()).Take(2).Select(arg => arg.Id).ToList();
                if (selectedCategories[0] == selectedCategories[1]) continue;
                indexVM.Cat1books = _bookService.GetRelatedBooksByCategory(selectedCategories[0], 0, PAGINATION_TAKE).ToList();
                indexVM.Cat2books = _bookService.GetRelatedBooksByCategory(selectedCategories[1], 0, PAGINATION_TAKE).ToList();
            }
            //indexVM.Cat1books = _bookService.GetRelatedBooksByCategory(selectedCategories[0], 0, 15).ToList();
            //indexVM.Cat2books = _bookService.GetRelatedBooksByCategory(selectedCategories[1], 0, 15).ToList();
            int index= new Random().Next(0, 20);
            indexVM.Books = _bookService.GetNewBooks(index, 10).ToList();
            while (indexVM.Books.Count<10)
            {
                index = new Random().Next(0, 20);
                indexVM.Books = _bookService.GetNewBooks(index, 10).ToList();
                if(indexVM.Books.Count <10)
                {
                    break;
                }
            }

            indexVM.PromotionsList = _promotionService.GetPromotionsByDateRange(DateTime.Now, DateTime.Now).ToList();

            foreach (var prizeVM in allPrizebook)
            {
                if (FreeBooksList.Exists(x => x.Id == prizeVM.BookId)) continue;

                var _PrizeDetailsVM = new PrizeDetailsVM();
                //find single book
                _PrizeDetailsVM.SingleBook = _bookService.GetBookById(prizeVM.BookId);
                if (allEditors != null && allEditors.Count > 0)
                {
                    var editor = allEditors.Find(e => e.Id == _PrizeDetailsVM.SingleBook.PublisherId);
                    _PrizeDetailsVM.SingleBook.BusinessPrice = Math.Round(_PrizeDetailsVM.SingleBook.Price * (editor.RateOnOriginalPrice/100), 2);
                }

                //find the prize giver to this book
                _PrizeDetailsVM.Prize = _prizeService.GetPrizeById(prizeVM.PrizeId);
                indexVM.PrizeBookVM.BookListVM.Add(_PrizeDetailsVM);
            }

            prizeBookDetailsVM.AuthorsList = base.GetAllAuthorVM().ToList();
            //prizeBookDetailsVM.AuthorsList = base.GetAllAuthors().ToList();
            //prizeBookDetailsVM.AuthorsList = new List<Author>();

            FillBookVMAdditionalInfos.FillBookVMList(indexVM.Cat1books, allAuthors, allEditors, allCategories, allCountries, allLanguages, allPrizebook, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
            FillBookVMAdditionalInfos.FillBookVMList(indexVM.Cat2books, allAuthors, allEditors, allCategories, allCountries, allLanguages, allPrizebook, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
            FillBookVMAdditionalInfos.FillBookVMList(indexVM.Books, allAuthors, allEditors, allCategories, allCountries, allLanguages, allPrizebook, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

            SetLibraryCapacity(indexVM.Cat1books, FreeBooksList, AllFreePromotions, userId);
            SetLibraryCapacity(indexVM.Cat2books, FreeBooksList, AllFreePromotions, userId);
            SetLibraryCapacity(indexVM.Books, FreeBooksList, AllFreePromotions, userId);

            indexVM.EditorsList = base.GetAllEditors().Where(x => !string.IsNullOrWhiteSpace(x.PhotoPath) && x.isActive).OrderBy(x => x.RaisonSocial).ToList();

            return View(indexVM);
        }

        public IActionResult ComingSoon()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddJoinRequest(JoinRequestVM joinRequestVM)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(joinRequestVM);
            }
            try
            {
                //check if entered email is already used
                var ExistingJR = _joinRequestService.GetJoinRequestByEmail(joinRequestVM.Email);
                var ExistingUser = _userManager.FindByEmailAsync(joinRequestVM.Email).Result;
                var ExistingAuthor = _authorService.GetAuthorByEmail(joinRequestVM.Email);
                if (ExistingJR == null && ExistingUser == null && ExistingAuthor == null)
                {
                    _joinRequestService.AddJoinRequest(joinRequestVM);
                    await _emailSender.SendContactEmailAsync(joinRequestVM.Email, joinRequestVM.FirstName, joinRequestVM.LastName, joinRequestVM.Subject, joinRequestVM.Description);
                }
                else
                {
                    NotifyError("Email_Already_Used", "This Email is Already Used");
                    return Response();
                }
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
                data = ""
            }));
        }

        [HttpPost]
        public async Task<IActionResult> competitionRequest([FromBody] JoinRequestVM joinRequestVM)
        {
            try
            {
                var desiredBooks = joinRequestVM.DesiredBooks.Select(book => book.Title).ToArray();
                if (desiredBooks.Any())
                {
                    joinRequestVM.Description = string.Join("/", desiredBooks);
                }
                joinRequestVM.Subject = "Concours « سمّعني كتاب »";
                joinRequestVM.CountryId = 226;

                _joinRequestService.AddJoinRequest(joinRequestVM);
                await _emailSender.SendCompetitionEmailAsync(joinRequestVM.Email, joinRequestVM.VoucherNumber, joinRequestVM.VoucherValue, joinRequestVM.Subject, joinRequestVM.Description);
               
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
                data = ""
            }));
        }
        [HttpPost]
        public async Task<IActionResult> MothersDayRequest([FromBody] JoinRequestVM joinRequestVM)
        {
            try
            {
                var desiredBooks = joinRequestVM.DesiredBooks.Select(book => book.Title).ToArray();
                if (desiredBooks.Any())
                {
                    joinRequestVM.Description = string.Join("/", desiredBooks);
                }
                joinRequestVM.Subject = "Fête des mères";
                joinRequestVM.CountryId = 226;

                _joinRequestService.AddJoinRequest(joinRequestVM);
                await _emailSender.SendMothersDayEmailAsync(joinRequestVM.Email, joinRequestVM.ReceiverEmail, joinRequestVM.Subject, joinRequestVM.Description);

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
                data = ""
            }));
        }
        [HttpPost]
        public async Task<IActionResult> laureatRequest([FromBody] JoinRequestVM joinRequestVM)
        {
            try
            {
                var desiredBooks = joinRequestVM.DesiredBooks.Select(book => book.Title).ToArray();
                if (desiredBooks.Any())
                {
                    joinRequestVM.Description = string.Join("/", desiredBooks);
                }
                joinRequestVM.Subject = "Lauréat Institut supérieure des langues de Tunis 22/23";
                joinRequestVM.CountryId = 226;

                _joinRequestService.AddJoinRequest(joinRequestVM);
                await _emailSender.SendLaureatEmailAsync(joinRequestVM.Email, joinRequestVM.VoucherNumber, joinRequestVM.VoucherValue, joinRequestVM.Subject, joinRequestVM.Description);

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
                data = ""
            }));
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            base.RemoveCategoriesFromCache();

            //var newURL = culture.Substring(0, 2);
            if (returnUrl.Contains("/Category/"))
            {
                var toBeTranslatedCategory = returnUrl.Substring(returnUrl.IndexOf("/Category/") + "/Category/".Length).Trim();
                var categoryList = _categoryService.GetAllUsedCategories().ToList();
                foreach (var category in categoryList)
                {
                    Dictionary<string, string> CategoryNames = JsonSerializer.Deserialize<Dictionary<string, string>>(category.CategoryName);
                    var oldCat = CategoryNames.Where(x => x.Key == Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName).ToList()[0].Value.Trim();
                    if (oldCat == toBeTranslatedCategory)
                    {
                        var newCat = CategoryNames.Where(x => x.Key == culture.Substring(0, 2)).ToList()[0].Value.Trim();
                        returnUrl = returnUrl.Replace(oldCat, newCat);
                        return LocalRedirect(returnUrl);
                    }
                }
            }

            return LocalRedirect(returnUrl);
        }

        private readonly static string reservedCharacters = "!*'();:@&=+$,/?%[]";

        public static string UrlEncode(string value)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            var sb = new StringBuilder();

            foreach (char @char in value)
            {
                if (reservedCharacters.IndexOf(@char) == -1)
                    sb.Append(@char);
                else
                    sb.AppendFormat("%{0:X2}", (int)@char);
            }
            return sb.ToString();
        }

        public IActionResult RenderEmail(JoinRequestVM joinRequestVM)
        {
            return PartialView(joinRequestVM);
        }

        #region SearchEngine
        [HttpPost]
        public async Task<IActionResult> SearchResult([FromBody] SearchVM searchVM)
        {
            PagedBooks pagedBooks = new DDD.Application.Specifications.PagedBooks { PageIndex = searchVM.currentPageIndex, PageSize = 12, SearchKeyType = (SearchEnum)searchVM.SearchType, SearchKeyText = searchVM.KeyWord };
            return PartialView("SearchResult", await searchEngine(pagedBooks));
        }

        [HttpPost]
        public async Task<IActionResult> PartialSearchResult([FromBody] PagedBooks pagedBooks)
        {
            var searchResultVM = await searchEngine(pagedBooks);
            var booksGridView = await RenderPartialViewToString("GridViewSearchResult", searchResultVM.BooksList);

            var booksListView = await RenderPartialViewToString("ListViewSearchResult", searchResultVM.BooksList);


            if (searchResultVM.BooksList.Count() > 0)
            {

                return Json(new
                {
                    Status = 1,
                    BooksCount = searchResultVM.BooksList.Count(),
                    Authors = searchResultVM.AuthorsList,
                    Categories = searchResultVM.CategoriesList,
                    Languages = searchResultVM.LanguagesList,
                    BooksGridView = booksGridView,
                    BooksListView = booksListView
                    //,BooksView = booksView
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

        private async Task<SearchResultVM> searchEngine(PagedBooks pagedBooks)
        {
                searchResultVM.BooksList = (List<BookViewModel>)await _bookService.GetPagedBooks(pagedBooks);

                var allEditors = base.GetAllEditors().ToList();
                var allCategories = base.GetAllCategories().ToList();
                var allAuthors = await _authorService.GetUsedAuthors();
                var allLanguages = await _languageService.GetUsedLanguages();
                var allCountries = base.GetAllCountries().ToList();

                var categories = searchResultVM.BooksList.Select(b => b.CategoryId);
                var languages = searchResultVM.BooksList.Select(b => b.LanguageId);
                var authors = searchResultVM.BooksList.Select(b => b.AuthorId).Distinct();
                searchResultVM.currentPageIndex = pagedBooks.PageIndex;
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
                var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
                var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
                var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
                var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
                var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
                SetLibraryCapacity(searchResultVM.BooksList, FreeBooksList, AllFreePromotions, userId);


                searchResultVM.CategoriesList = allCategories.Distinct();
                searchResultVM.AuthorsList = allAuthors;
                searchResultVM.LanguagesList = allLanguages;
                searchResultVM.KeyWord = pagedBooks.SearchKeyText;
                searchResultVM.SearchType = (int)pagedBooks.SearchKeyType;

                FillBookVMAdditionalInfos.FillBookVMList(searchResultVM.BooksList, searchResultVM.AuthorsList.ToList(), allEditors, allCategories, allCountries, searchResultVM.LanguagesList.ToList(), null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);

                return searchResultVM;
        }
        #endregion

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

        public int EditorPageCount(string editorId)
        {
            var ConnString = _configuration.GetConnectionString("DefaultConnection");
            string SqlString = "";

            SqlString = "Select count(b.Id) as Numb From Books as b, Authors as a Where b.PublisherId LIKE @editorId " +
                "and b.IsDeleted = 'false' " +
                "and b.AuthorId = a.id and b.Status = 3" +
                "and a.isDeleted = 'false'";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(SqlString, conn))
                {
                    cmd.Parameters.AddWithValue("editorId", editorId);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return (int)reader["Numb"];
                    }
                }
            }
            return 0;
        }

        #endregion

        public IActionResult GetEditorsList()
        {

            var allEditors = base.GetAllEditors().OrderBy(x => x.RaisonSocial).ToList();
            var _EditorsDetails = new EditorsDetails();
            _EditorsDetails.EditorsList = allEditors;
            //var _EditorVM = new SingleEditorDetailsVM();
            var _editorsList = new List<SingleEditorDetailsVM>();
            //_editorsList= allEditors
            foreach (ApplicationUser editor in allEditors)
            {
                var _EditorVM = new SingleEditorDetailsVM();
                _EditorVM.Editor = editor;
                //get the count of published books using raw SQL
                _EditorVM.PublishedBooksCount = EditorPageCount(_EditorVM.Editor.Id);

                _editorsList.Add(_EditorVM);
            }
            /*
             public class SingleEditorDetailsVM
            {
                public ApplicationUser Editor { get; set; }
                public List<BookViewModel> PublishedBooks { get; set; }
                public int PublishedBooksCount { get; set; }
            }
             */
            return View("EditorsList", _editorsList);
        }

        [HttpGet]
        public async Task<IActionResult> GetEditorDetails(string EditorInfos)
        {
            var editorId = EditorInfos;//.Split("#")[0];
            var _EditorVM = new SingleEditorDetailsVM();
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

            _EditorVM.Editor = _userManager.FindByIdAsync(editorId).Result;
            _EditorVM.PublishedBooks =(List<BookViewModel>) await _bookService.GetPagedBooks(new DDD.Application.Specifications.PagedBooks { PageIndex = 0, PageSize= 12, EditorId= _EditorVM.Editor.Id }); 
            var authors = _EditorVM.PublishedBooks.Select(b => b.AuthorId).Distinct();
            searchResultVM.AuthorsList = allAuthors.Where(c => authors.Contains(c.Id));
            FillBookVMAdditionalInfos.FillBookVMList(_EditorVM.PublishedBooks, searchResultVM.AuthorsList.ToList(), new List<ApplicationUser> { _EditorVM.Editor }, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
            SetLibraryCapacity(_EditorVM.PublishedBooks, FreeBooksList, AllFreePromotions, userId);

            return View(_EditorVM);
        }

        [HttpPost]
        public async Task<IActionResult> LoadEditorBookList(string EditorId, int currentPageIndex)
        {
            var _EditorVM = new SingleEditorDetailsVM();
            _EditorVM.PublishedBooks = (List<BookViewModel>)await _bookService.GetPagedBooks(new DDD.Application.Specifications.PagedBooks { PageIndex = currentPageIndex, PageSize = PAGINATION_TAKE, EditorId = EditorId });
            _EditorVM.Editor = _userManager.FindByIdAsync(EditorId).Result;

            if (_EditorVM.PublishedBooks.Count() > 0)
            {
                var editorId = EditorId;//.Split("#")[0];
                var allAuthors = base.GetAllAuthorVM().ToList();
                //var allAuthors = base.GetAllAuthors().ToList();
                var allCategories = base.GetAllCategories().ToList();
                var allLanguages = base.GetAllLanguages().ToList();
                var allCountries = base.GetAllCountries().ToList();

                var authors = _EditorVM.PublishedBooks.Select(b => b.AuthorId).Distinct();
                searchResultVM.AuthorsList = allAuthors.Where(c => authors.Contains(c.Id));

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
                var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
                var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
                var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
                var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
                var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
                SetLibraryCapacity(_EditorVM.PublishedBooks, FreeBooksList, AllFreePromotions, userId);

                FillBookVMAdditionalInfos.FillBookVMList(_EditorVM.PublishedBooks, searchResultVM.AuthorsList.ToList(), new List<ApplicationUser> { _EditorVM.Editor }, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
                var booksGridView = await RenderPartialViewToString("GridViewSearchResult", _EditorVM.PublishedBooks);

                return Json(new
                {
                    Status = 1,
                    BooksCount = _EditorVM.PublishedBooks.Count(),
                    BooksGridView = booksGridView
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

        public IActionResult AboutUs()
        {
            return View();
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

