using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DDD.Application.ILogics;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Models;
using DDD.Infra.CrossCutting.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MipLivrelStore.Helper;
using static DDD.Application.Enum.Constants;
using static DDD.Domain.Common.Constants.State;

namespace MipLivrelStore.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookController : ApiController
    {
        private readonly IBookService _bookService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
        private readonly IAuthorService _authorService;
        private readonly IPromotionService _promotionService; 
        private readonly IFavoriteService _IFavoriteService;
        private readonly ICartService _cartService;
        private readonly ILibraryService _libraryService;
        private readonly IFileManagerLogic _fileManagerLogic;
        private readonly IInvoiceService _InvoiceService;

        public BookController(
            IBookService bookService, UserManager<ApplicationUser> userManager, ICategoryService categoryService,
            ICountryService countryService, ILanguageService languageService, IFavoriteService IFavoriteService,
            IFileManagerLogic fileManagerLogic, ILibraryService libraryService,
            ICartService cartService, IPromotionService promotionService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator, IAuthorService authorService, IInvoiceService InvoiceService) : base(notifications, mediator)
        {
            _bookService = bookService;
            _userManager = userManager;
            _categoryService = categoryService;
            _countryService = countryService;
            _languageService = languageService;
            _authorService = authorService;
            _IFavoriteService = IFavoriteService;
            _cartService = cartService;
            _fileManagerLogic = fileManagerLogic;
            _promotionService = promotionService;
            _InvoiceService = InvoiceService;
            _libraryService = libraryService;
        }

        [HttpGet]
        //[Authorize(Roles = Roles.Admin)]
        [Route("getAll")]
        public IActionResult Get()
        {
            var list = _bookService.GetAll();
            return Response(list);

        }

        [HttpGet]
        [Route("getById")]
        public IActionResult Get(Guid id)
        {
            var bookViewModel = _bookService.GetBookById(id);

            return Response(bookViewModel);
        }

        [HttpGet]
        [Route("pagination")]
        public IActionResult GetByPageIndex(int skip, int take)
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = _bookService.GetAll(skip, take).ToList();
            //var allEditors = _userManager.GetUsersInRoleAsync(UserRoleVM.Editor.ToString()).Result.ToList();
            //var allAuthors = _authorService.GetAllAsVM().ToList();//new List<Author>();//_userManager.GetUsersInRoleAsync(UserRoleVM.Author.ToString()).Result.ToList();
            //var allCategories = _categoryService.GetAll().ToList();
            //var allLanguages = _languageService.GetAll().ToList();
            //var allCountries = _countryService.GetAll().ToList(); 
            //var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
            //var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
            //var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            //var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            //var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
            
            //list = FillBookVMAdditionalInfos.FillBookVMList(list, allAuthors, allEditors, allCategories, allCountries, allLanguages,null,_favoriteBooks,_cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
            ////foreach (var bookVM in list)
            //{
            //    var fav = _IFavoriteService.GetFavoriteBookByUserIdAndBookId(userId, bookVM.Id).FirstOrDefault();
            //    if (fav != null) bookVM.isFavorite = !(fav.Id == Guid.Empty);
            //    var cb = _cartService.GetCartByUserIdAndBookId(userId, bookVM.Id).FirstOrDefault();
            //    if (cb != null) bookVM.inCart = !(cb.Id == Guid.Empty);
            //}
            return Response(list);
        }


        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [Route("Update")]
        public async Task<IActionResult> Put([FromForm] BookViewModel bookViewModel)
        {

            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(bookViewModel);
            }

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

            _bookService.UpdateBook(bookViewModel);

            return Response(bookViewModel);
        }
         
        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [Route("UpdateState")]
        public IActionResult PutBookState([FromBody] BookStatePutVM bookStatePutVM)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(bookStatePutVM);
            }

            _bookService.UpdateBookState(bookStatePutVM);

            return Response(bookStatePutVM);
        }
         
        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        [Route("Delete")]
        public IActionResult Delete(Guid id)
        {
            var DeleteResult = _bookService.DeleteBook(id);

            return Response();
        }
         
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("Add")]
        public async Task<IActionResult> Post([FromForm] BookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(bookViewModel);
            }

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

            _bookService.AddBook(bookViewModel);

            return Response(bookViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AddFavoriteBook")]
        public IActionResult AddFavoriteBook([FromBody] FavoriteBookVM favoriteBookVM)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(favoriteBookVM);
            }

            _IFavoriteService.AddFavoriteBook(favoriteBookVM);

            return Response();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getAllFavoriteBooks")]
        public IActionResult GetFavoriteBookByPageIndex()
        {
            var list = _IFavoriteService.GetAllFavoriteBooks();
            return Response(list);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("PaginationFavoriteBook")]
        public IActionResult GetFavoriteBookByPageIndex(int skip, int take)
        {
            var list = _IFavoriteService.GetAllFavoriteBooks(skip, take);
            return Response(list);
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("deleteFavoriteBook")]
        public IActionResult deleteFavoriteBook(Guid id)
        {
            _IFavoriteService.DeleteFavoriteBook(id);
            return Response();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AddFavoriteCategory")]
        public IActionResult AddFavoriteCategory([FromBody] FavoriteCategoryVM favoriteCategoryVM)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(favoriteCategoryVM);
            }

            _IFavoriteService.AddFavoriteCategory(favoriteCategoryVM);

            return Response();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getAllFavoriteCategorys")]
        public IActionResult GetFavoriteCategoryByPageIndex()
        {
            var list = _IFavoriteService.GetAllFavoriteCategories();
            return Response(list);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("PaginationFavoriteCategory")]
        public IActionResult GetFavoriteCategoryByPageIndex(int skip, int take)
        {
            var list = _IFavoriteService.GetAllFavoriteCategories(skip, take);
            return Response(list);
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("deleteFavoriteCategory")]
        public IActionResult deleteFavoriteCategory(Guid id)
        {
            _IFavoriteService.DeleteFavoriteCategory(id);
            return Response();
        }
         
        [HttpPost]
         [Authorize(Roles = Roles.Admin)]
        [Route("interval")]
        public IActionResult GetBookByEditorInterval([FromBody]BookPostVM bookPostVM)
        {
            return Response(_bookService.GetBookByEditorIdInterval(bookPostVM));
        }

        [HttpGet]
        [Route("AllPromotions")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetAllPromotions()
        {
            var list = _promotionService.GetAll().ToList();

            return Response(list);
        }

        [HttpGet]
        [Route("AllPromotionsWithPagination")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetAllPromotions(int skip, int take)
        {
            var list = _promotionService.GetAll(skip, take).ToList();

            return Response(list);
        }

        [HttpGet]
        [Route("AllPromotionBooks")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetAllPromotionBooks()
        {
            var list = _promotionService.GetAllPromotionBook().ToList();

            return Response(list);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [Route("GetPromotionById")]
        public IActionResult GetPromotion(Guid Id)
        {
            var promotionVM = _promotionService.GetPromotionById(Id);

            return Response(promotionVM);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("AddPromotion")]
        public async Task<IActionResult> AddPromotion([FromForm] PromotionVM promotionVM)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(promotionVM);
            }

            if (promotionVM.Image != null)
            {
                string fileName = Guid.NewGuid().ToString();
                promotionVM.ImagePath = $"{fileName}{Path.GetExtension(promotionVM.Image.FileName)}";
                await _fileManagerLogic.Upload(promotionVM.Image, "assets", promotionVM.ImagePath);
            }

            _promotionService.AddPromotion(promotionVM);

            return Response(promotionVM);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("AddPromotionBook")]
        public IActionResult AddPromotionBook([FromBody] PromotionBookVM promotionBookVM)
        {

            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(promotionBookVM);
            }

            _promotionService.AddPromotionBook(promotionBookVM);

            return Response(promotionBookVM);
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [Route("UpdatePromotion")]
        public async Task<IActionResult> UpdatePromotion([FromForm] PromotionVM promotionVM)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(promotionVM);
            }

            if (promotionVM.Image != null)
            {
                string fileName = Guid.NewGuid().ToString();
                promotionVM.ImagePath = $"{fileName}{Path.GetExtension(promotionVM.Image.FileName)}";
                await _fileManagerLogic.Upload(promotionVM.Image, "assets", promotionVM.ImagePath);
            }

            _promotionService.UpdatePromotion(promotionVM);

            return Response(promotionVM);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete]
        [Route("DeletePromotion")]
        public IActionResult DeletePromotion(Guid Id)
        {
            _promotionService.DeletePromotion(Id);
            return Response();
        }




        // Invoices
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [Route("GetInvoicesByEditor")]
        public IActionResult GetInvociesByEditor( string editorId)
        {
            //get the list of selled books published by this editor
            List<InvoiceVM> list = _InvoiceService.GetByEditorId(0, 14, editorId).OrderByDescending(x=>x.Date).ToList();
            var allCountries = _countryService.GetAll();
            foreach (var l in list)
            {
                l.User.Country = new Country();
                l.User.Country.Name = allCountries.Where(x => x.Id == l.User.CountryId).FirstOrDefault().Name;
            }
            return Response(list);
        }
    }
}
