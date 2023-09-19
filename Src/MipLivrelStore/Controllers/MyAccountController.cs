using System;
using System.Collections.Generic;
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
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.CrossCutting.Identity.Authorization;
using DDD.Infra.CrossCutting.Identity.Extensions;
using DDD.Infra.CrossCutting.Identity.Models.AccountViewModels;
using DDD.Infra.CrossCutting.Identity.Services;
using DDD.Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MipLivrelStore.Helper;
using static DDD.Application.Enum.Constants;
using static DDD.Domain.Common.Constants.State;
using DDD.Domain.Common.Constants;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Net.Http;
using Microsoft.AspNetCore.Localization;
using System.Text.Json;
using DDD.Application.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Facebook;
using Newtonsoft.Json;
using DDD.Application.Services;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using System.Linq.Expressions;

namespace MipLivrelStore.Controllers
{
    [Route("[controller]/[action]")]
    public class MyAccountController : BaseController
    {
        private readonly IJoinRequestService _joinRequestService;
        private readonly IFileManagerLogic _fileManagerLogic;
        private const int PAGINATION_TAKE = 12;
        SearchResultVM searchResultVM = new SearchResultVM();
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUser _user;
        private readonly ILogger _logger;
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;
        private readonly ILanguageService _languageService;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;
        private readonly IAuthorService _authorService;
        private readonly IFavoriteService _IFavoriteService;
        private readonly ICartService _cartService;
        private ICompositeViewEngine _viewEngine;
        private readonly ILibraryService _libraryService;
        private readonly IInvoiceService _InvoiceService;
        private readonly IPromoUserService _promoUserService;
        private readonly IPromotionService _promotionService;
        private readonly IConfiguration _configuration;

        string FB_AppID = string.Empty;
        string FB_AppSecret = string.Empty;

        public MyAccountController(
         IFileManagerLogic fileManagerLogic,
           IJoinRequestService joinRequestService,
        IMemoryCache memoryCache,
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           IEmailSender emailSender,
           IUser user, IPromotionService promotionService, IPromoUserService promoUserService,
           ILoggerFactory loggerFactory,
           IInvoiceService InvoiceService, IConfiguration configuration,
           INotificationHandler<DomainNotification> notifications,
           IMediatorHandler mediator, IBookService bookService, ICategoryService categoryService, IFavoriteService IFavoriteService, ICartService cartService,
           ICountryService countryService, ILanguageService languageService, ILibraryService libraryService,
           ICompositeViewEngine viewEngine,
           IMapper mapper, IAuthorService authorService, IPrizeService prizeService)

            : base(joinRequestService, fileManagerLogic, memoryCache, userManager, loggerFactory, notifications, mediator, bookService,
               categoryService, mapper, promotionService, countryService, languageService, cartService, authorService, prizeService, libraryService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _user = user;
            _logger = loggerFactory.CreateLogger<MyAccountController>();
            _bookService = bookService;
            _categoryService = categoryService;
            _mapper = mapper;
            _countryService = countryService;
            _languageService = languageService;
            _fileManagerLogic = fileManagerLogic;
            _authorService = authorService;
            _joinRequestService = joinRequestService;
            _IFavoriteService = IFavoriteService;
            _cartService = cartService;
            _viewEngine = viewEngine;
            _libraryService = libraryService;
            _InvoiceService = InvoiceService;
            _memoryCache = memoryCache;
            _configuration = configuration;
            _promotionService = promotionService;
            _promoUserService = promoUserService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnurl = null)
        {
            //request a redirect to the external login provider
            var redirecturl = Url.Action("ExternalLoginCallback", "MyAccount", new { ReturnUrl = returnurl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirecturl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnurl = null, string remoteError = null)
        {

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                //return RedirectToAction(nameof(Login));
                /*added by me */
                returnurl = returnurl ?? Url.Content("/");
                return LocalRedirect(returnurl);
            }

            //Sign in the user with this external login provider, if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true);
            if (result.Succeeded)
            {
                //update any authentication tokens
                await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                /*added by me */
                returnurl = returnurl ?? Url.Content("~/");

                //return LocalRedirect(returnurl);
                return Redirect(returnurl);
            }
            else
            {
                //If the user does not have account, then we will ask the user to create an account.
                ViewData["ReturnUrl"] = returnurl;
                ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var userName = info.Principal.FindFirstValue(ClaimTypes.Name);
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                //var country = info.Principal.FindFirstValue(ClaimTypes.Country);
                var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                string picture = string.Empty;


                if (info.ProviderDisplayName.ToLower() == "facebook")
                {
                    picture = $"https://graph.facebook.com/{identifier}/picture?type=large";
                    return View("ExternalLoginConfirmation", new ExternalLoginViewModel { Email = email, FirstName = firstName, LastName = lastName, PhotoPath = picture });
                }
                else /*(info.ProviderDisplayName.ToLower() == "google")*/
                {

                    var user = new ApplicationUser
                    {
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        CountryId = 1,
                        EmailConfirmed = true,
                        CreatedAt= DateTime.Now
                    };
                    user.PhotoPath = info.Principal.FindFirstValue("image");
                    user.UserName = email;
                    var result1 = await _userManager.CreateAsync(user);
                    if (result1.Succeeded)
                    {
                        ViewBag.ProviderDisplayName = info.ProviderDisplayName;
                        await _userManager.AddToRoleAsync(user, UserRoleVM.Subscriber.ToString());
                        var result2 = await _userManager.AddLoginAsync(user, info);
                        if (result2.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: true);
                            await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                            return LocalRedirect(returnurl);

                        }

                    }
                    return LocalRedirect(returnurl);
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string? returnurl = null)
        {
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                //get the info about the user from external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;

                    return Json(new { Result = "infoError", url = "/MyAccount/Error" });
                }
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CountryId = 1,
                    EmailConfirmed = true,
                    PhotoPath = model.PhotoPath,
                    CreatedAt= DateTime.Now
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    ViewBag.ProviderDisplayName = info.ProviderDisplayName;
                    await _userManager.AddToRoleAsync(user, UserRoleVM.Subscriber.ToString());
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: true);
                        await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                        return Json(new { Result = "registrationSuccess", url = returnurl });
                        //return LocalRedirect(returnurl);
                    }

                }
                else
                {
                    var errorsTab = result.Errors.ToArray();
                    foreach (var error in errorsTab) NotifyError("Error_" + error.Code.ToUpper(), error.Code);
                    return Response();
                }
                ModelState.AddModelError("Email", "Error occured");
            }
            ViewData["ReturnUrl"] = returnurl;
            return View(model);
        }

        [HttpGet]
        public IActionResult AccountDetails()
        {
            var country = base.GetAllCountries().ToList();
            if (country != null)
            {
                ViewBag.country = country;
            }

            return PartialView();
        }

        [HttpGet]
        public IActionResult CompleteAccountDetails(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                ModelState.AddModelError("Email", "Email not found");
                return BadRequest("Email not found");
            }

            //var appUser = await _userManager.FindByEmailAsync(userEmail);
            //if(appUser is null)
            //{
            //    ModelState.AddModelError("User", "User with given email was not found");
            //    return BadRequest("User with given email was not found");
            //}

            var country = base.GetAllCountries().ToList();
            if (country != null)
            {
                ViewBag.country = country;
            }

            var model = new RegisterViewModel
            {
                Email = userEmail,
            };

            return View(model);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccount(RegisterViewModel model)
        {
            var currentUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
            if (model.NewPassword != null)
            {
                var verifyCurrentPassword = await _userManager.CheckPasswordAsync(currentUser, model.Password);
                if (!verifyCurrentPassword)
                {
                    NotifyError("WrongPassword", "WrongPassword");
                    return Response();
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    NotifyModelStateErrors();
                    var errors = ModelState.Select(x => x.Value.Errors)
                               .Where(y => y.Count > 0)
                               .ToList();
                    return Response();
                }
                else
                {
                    //UpdatePassword
                    var updatePasswordResult = await _userManager.ChangePasswordAsync(currentUser, model.Password, model.NewPassword);
                    if (updatePasswordResult.Succeeded)
                    {
                        currentUser.PhoneNumber = model.PhoneNumber;
                        currentUser.FirstName = model.FirstName;
                        currentUser.LastName = model.LastName;
                        currentUser.UserName = model.Email;
                        currentUser.Address = model.Address;
                        currentUser.CountryId = model.CountryId>0?model.CountryId: null;
                        currentUser.IdFiscal = model.IdFiscal;
                        currentUser.RaisonSocial = model.RaisonSocial;

                        if (model.PhotoFile != null)
                        {
                            // UplodePhoto
                            if (model.PhotoFile != null)
                            {
                                try
                                {
                                    string fileName = Guid.NewGuid().ToString();
                                    currentUser.PhotoPath = $"{fileName}{Path.GetExtension(model.PhotoFile.FileName)}";
                                    await _fileManagerLogic.Upload(model.PhotoFile, "bookscover", currentUser.PhotoPath);
                                }
                                catch (Exception exp)
                                {
                                    return new JsonResult(new { message = "Cannot upload image file !" })
                                    {
                                        StatusCode = (int)HttpStatusCode.InternalServerError
                                    };
                                }
                            }
                        }

                        // UpdateUser
                        var updateUser = await _userManager.UpdateAsync(currentUser);

                        if (updateUser.Succeeded)
                        {
                            return Json(Ok(new
                            {
                                success = true,
                                //  data = 
                            }));
                        }
                        else
                        {
                            AddIdentityErrors(updateUser);
                            return Json(NotFound(new
                            {
                                success = false,
                                //data = ex
                            }));
                        }
                    }
                    else
                    {
                        var errorsTab = updatePasswordResult.Errors.ToArray();
                        foreach (var error in errorsTab)
                            NotifyError("Error_" + error.Code.ToUpper(), error.Code);
                        //AddIdentityErrors(identityResult);
                        //View(model);
                        return Response();
                        //AddIdentityErrors(updatePasswordResult);
                        //return Json(NotFound(new
                        //{
                        //    success = false,
                        //    //data = ex
                        //}));
                    }
                }
            }
            else
            {
                currentUser.PhoneNumber = model.PhoneNumber;
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                currentUser.UserName = model.Email;
                currentUser.Address = model.Address;
                currentUser.CountryId = model.CountryId>0? model.CountryId:null;
                currentUser.IdFiscal = model.IdFiscal;
                currentUser.RaisonSocial = model.RaisonSocial;

                if (model.PhotoFile != null)
                {
                    // UplodePhoto
                    if (model.PhotoFile != null)
                    {
                        try
                        {
                            string fileName = Guid.NewGuid().ToString();
                            currentUser.PhotoPath = $"{fileName}{Path.GetExtension(model.PhotoFile.FileName)}";
                            await _fileManagerLogic.Upload(model.PhotoFile, "bookscover", currentUser.PhotoPath);
                        }
                        catch (Exception exp)
                        {
                            return new JsonResult(new { message = "Cannot upload image file !" })
                            {
                                StatusCode = (int)HttpStatusCode.InternalServerError
                            };
                        }
                    }
                }

                // UpdateUser
                var updateUser = await _userManager.UpdateAsync(currentUser);

                if (updateUser.Succeeded)
                {
                    return Json(Ok(new
                    {
                        success = true,
                        //data = currentUser
                    }));
                }
                else
                {
                    AddIdentityErrors(updateUser);
                    return Json(NotFound(new
                    {
                        success = false,
                        //data = ex
                    }));
                }
            }
            //currentUser.PhoneNumber = model.PhoneNumber;
            //currentUser.FirstName = model.FirstName;
            //currentUser.LastName = model.LastName;
            //currentUser.UserName = model.Email;
            //currentUser.Address = model.Address;
            //currentUser.CountryId = model.CountryId;
            //currentUser.IdFiscal = model.IdFiscal;
            //currentUser.RaisonSocial = model.RaisonSocial;

            //if (model.PhotoFile != null)
            //{
            //    // UplodePhoto
            //    if (model.PhotoFile != null)
            //    {
            //        try
            //        {
            //            string fileName = Guid.NewGuid().ToString();
            //            currentUser.PhotoPath = $"{fileName}{Path.GetExtension(model.PhotoFile.FileName)}";
            //            await _fileManagerLogic.Upload(model.PhotoFile, "bookscover", currentUser.PhotoPath);
            //        }
            //        catch (Exception exp)
            //        {
            //            return new JsonResult(new { message = "Cannot upload image file !" })
            //            {
            //                StatusCode = (int)HttpStatusCode.InternalServerError
            //            };
            //        }
            //    }
            //}

            //// UpdateUser
            //var updateUser = await _userManager.UpdateAsync(currentUser);

            ////if (model.NewPassword != null)
            ////{
            ////    if (model.NewPassword != model.ConfirmPassword)
            ////    {
            ////        NotifyModelStateErrors();
            ////        var errors = ModelState.Select(x => x.Value.Errors)
            ////                   .Where(y => y.Count > 0)
            ////                   .ToList();
            ////        return Response();
            ////    }

            ////    //UpdatePassword
            ////    var updatePasswordResult = await _userManager.ChangePasswordAsync(currentUser, model.Password, model.NewPassword);
            ////    if (updatePasswordResult.Succeeded)
            ////    {
            ////        return Json(Ok(new
            ////        {
            ////            success = true,
            ////            // data = ""
            ////        }));
            ////    }
            ////    else
            ////    {
            ////        AddIdentityErrors(updatePasswordResult);
            ////        return Json(NotFound(new
            ////        {
            ////            success = false,
            ////            //data = ex
            ////        }));
            ////    }
            ////}
            ////else
            ////{
            //    if (updateUser.Succeeded)
            //    {
            //        return Json(Ok(new
            //        {
            //            success = true,
            //            //  data = 
            //        }));
            //    }
            //    else
            //    {
            //        AddIdentityErrors(updateUser);
            //        return Json(NotFound(new
            //        {
            //            success = false,
            //            //data = ex
            //        }));
            //    }
            //}

            return new JsonResult(new { message = "Erreur inconnu !" });
            //            {
            //                StatusCode = (int)HttpStatusCode.InternalServerError
            //            };
        }

        #region Editeur

        [HttpGet]
        public IActionResult Create()
        {
            var categories = base.GetAllCategories().ToList();
            if (categories != null)
            {
                ViewBag.categories = categories;
            }

            var language = base.GetAllLanguages().ToList();
            if (language != null)
            {
                ViewBag.language = language;
            }

            var country = base.GetAllCountries().ToList();
            if (country != null)
            {
                ViewBag.country = country;
            }

            var allAuthors = base.GetAllAuthorVM().ToList();
            //var allAuthors = base.GetAllAuthors().ToList();
            // var appUser = _userManager.GetUsersInRoleAsync(Roles.Author).Result.ToList();
            if (allAuthors != null)
            {
                ViewBag.allAuthors = allAuthors;
            }
            return PartialView();
        }




        [HttpPost]
        public async Task<IActionResult> AddBook(BookViewModel bookViewModel)
        {

            bookViewModel.Status = BookState.Created;
            var appUser = _userManager.FindByNameAsync(User.Identity.Name);
            bookViewModel.PublisherId = appUser.Result.Id;
            try
            {
                string fileName = Guid.NewGuid().ToString();
                bookViewModel.PDFPath = $"{fileName}{Path.GetExtension(bookViewModel.PDFFile.FileName)}";
                bookViewModel.CoverPath = $"{fileName}{Path.GetExtension(bookViewModel.CoverFile.FileName)}";

                _bookService.AddBook(bookViewModel);

                await _fileManagerLogic.Upload(bookViewModel.PDFFile, "bookspdf", bookViewModel.PDFPath);
                await _fileManagerLogic.Upload(bookViewModel.CoverFile, "bookscover", bookViewModel.CoverPath);
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
        public IActionResult CreatePrize()
        {
            return PartialView();
        }


        [HttpPost]
        public IActionResult AddAuthor(AuthorVM authorVM)
        {
            string authorId;
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(authorVM);

            }
            try
            {
                authorId = _authorService.AddAuthors(authorVM);
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
                data = authorId
            }));
        }




        [HttpGet]
        public IActionResult BookList()
        {

            var allEditors = base.GetAllEditors().ToList();
            var allAuthors = base.GetAllAuthorVM().ToList();
            //var allAuthors = base.GetAllAuthors().ToList();
            var allCategories = base.GetAllCategories().ToList();
            var allLanguages = base.GetAllLanguages().ToList();
            var allCountries = base.GetAllCountries().ToList();
            var appUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var role = _userManager.GetRolesAsync(appUser).Result[0];
            var Booklist = new List<BookViewModel>();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
            var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
            var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
            var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();

            switch (role)
            {
                case var value when value == UserRoleVM.Editor.ToString():
                    {
                        SearchResultVM searchResultVM = new SearchResultVM();

                        Booklist = _bookService.GetBooksByEditorId(appUser.Id, 0, PAGINATION_TAKE).ToList();
                        //SetLibraryCapacity(Booklist, FreeBooksList, AllFreePromotions, userId);
                        Booklist = FillBookVMAdditionalInfos.FillBookVMList(Booklist, allAuthors, allEditors, allCategories, allCountries, allLanguages);

                        //checkFavoriteAndCart(Booklist);
                        searchResultVM.BooksList = Booklist;
                        //ViewBag.bookList = Booklist;
                        double pageCount = (double)((decimal)PageCount(appUser.Id/*$"{ appUser.FirstName + " " + appUser.LastName}"*/) / Convert.ToDecimal(PAGINATION_TAKE));
                        searchResultVM.PageCount = (int)Math.Ceiling(pageCount);
                        searchResultVM.currentPageIndex = 0;
                        return PartialView(searchResultVM);
                    }
                case var value when value == UserRoleVM.Author.ToString():
                    {
                        Booklist = _bookService.GetBookByAuthor(appUser.FirstName + " " + appUser.LastName, 0, PAGINATION_TAKE).ToList();
                        //SetLibraryCapacity(Booklist, FreeBooksList, AllFreePromotions, userId);
                        Booklist = FillBookVMAdditionalInfos.FillBookVMList(Booklist, allAuthors, allEditors, allCategories, allCountries, allLanguages);
                        // checkFavoriteAndCart(Booklist);
                        ViewBag.bookList = Booklist;
                        return PartialView("BookList2", Booklist);
                    }
                default:
                    {
                        var libraryList = _libraryService.GetLibrariesByUserId(appUser.Id).ToList();
                        foreach (var libraryVM in libraryList)
                        {
                            var bookVM = _bookService.GetBookById(libraryVM.BookId);
                            bookVM.LibraryId = libraryVM.Id.ToString();
                            bookVM.inLibrary = true;
                            bookVM.CurrentPage = libraryVM.CurrentPage;
                            Booklist.Add(bookVM);
                        }
                        //Booklist = _bookService.GetAll().ToList();
                        SetLibraryCapacity(Booklist, FreeBooksList, AllFreePromotions, userId);
                        Booklist = FillBookVMAdditionalInfos.FillBookVMList(Booklist, allAuthors, allEditors, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
                        //checkFavoriteAndCart(Booklist);
                        ViewBag.bookList = Booklist;
                        return PartialView("BookList2", Booklist);
                    }
            }
        }
        #endregion 

        [HttpGet]
        public async Task<IActionResult> LoadEditorBookList(string pageIndex)
        {
            SearchResultVM searchResultVM = new SearchResultVM();

            int PageIndex = int.Parse(pageIndex);
            var tableRows = "";
            //var allEditors = base.GetAllEditors().ToList();
            var allAuthors = base.GetAllAuthorVM().ToList();
            //var allAuthors = base.GetAllAuthors().ToList();
            var allCategories = base.GetAllCategories().ToList();
            //var allLanguages = base.GetAllLanguages().ToList();
            //var allCountries = base.GetAllCountries().ToList();
            var appUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            double pageCount = (double)((decimal)PageCount(appUser.Id/*$"{ appUser.FirstName + " " + appUser.LastName}"*/) / Convert.ToDecimal(PAGINATION_TAKE));
            searchResultVM.PageCount = (int)Math.Ceiling(pageCount);
            searchResultVM.currentPageIndex = PageIndex;
            //var role = _userManager.GetRolesAsync(appUser).Result[0];
            var Booklist = new List<BookViewModel>();
            Booklist = _bookService.GetBooksByEditorId(appUser.Id, PAGINATION_TAKE * PageIndex, PAGINATION_TAKE).ToList();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
            var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
            var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
            var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
            SetLibraryCapacity(Booklist, FreeBooksList, AllFreePromotions, userId);

            FillBookVMAdditionalInfos.FillBookVMList(Booklist, allAuthors, null, allCategories, null, null, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
            //checkFavoriteAndCart(Booklist);
            searchResultVM.BooksList = Booklist;
            //return PartialView("BookList", searchResultVM);

            if (Booklist.Count() > 0)
            {
                foreach (var item in Booklist)
                {
                    tableRows = tableRows + await RenderPartialViewToString("BooksTablePartialView", item);
                }
                return Json(new
                {
                    Status = 1,
                    TableRows = tableRows,
                    PageCount = searchResultVM.PageCount,
                    CurrentPageIndex = PageIndex,
                    NewModel = searchResultVM
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

        [HttpPost]
        public IActionResult LoadEditorBookList2(int PageIndex)
        {
            SearchResultVM searchResultVM = new SearchResultVM();
            //int PageIndex = int.Parse(_pageIndex); 
            //int PageIndex = int.Parse(pageIndex);
            //var tableRows = "";
            //var allEditors = base.GetAllEditors().ToList();
            var allAuthors = base.GetAllAuthorVM().ToList();
            //var allAuthors = base.GetAllAuthors().ToList();
            var allCategories = base.GetAllCategories().ToList();
            //var allLanguages = base.GetAllLanguages().ToList();
            //var allCountries = base.GetAllCountries().ToList();
            var appUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            //double pageCount = (double)((decimal)PageCount($"{ appUser.FirstName + " " + appUser.LastName}") / Convert.ToDecimal(PAGINATION_TAKE));
            //ViewBag.PageCount = (int)Math.Ceiling(pageCount);
            searchResultVM.currentPageIndex = PageIndex;
            //var role = _userManager.GetRolesAsync(appUser).Result[0];
            var Booklist = new List<BookViewModel>();
            Booklist = _bookService.GetBooksByEditorId(appUser.Id, PAGINATION_TAKE * PageIndex, PAGINATION_TAKE).ToList();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
            var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
            var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
            var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
            var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
            var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
            SetLibraryCapacity(Booklist, FreeBooksList, AllFreePromotions, userId);

            FillBookVMAdditionalInfos.FillBookVMList(Booklist, allAuthors, null, allCategories, null, null, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
            //checkFavoriteAndCart(Booklist);

            //foreach (var item in Booklist)
            //{
            //    tableRows = tableRows + await RenderPartialViewToString("BooksTablePartialView", item);
            //} 
            //return Json(new
            //{
            //    Status = 1,
            //    TableRows = tableRows,
            //    PageCount = searchResultVM.PageCount,
            //    CurrentPageIndex = PageIndex,
            //    NewModel = searchResultVM
            //});
            return View(searchResultVM);
        }

        private int PageCount(string editorId)
        {
            var ConnString = _configuration.GetConnectionString("DefaultConnection");
            string SqlString = "";
            string parameterName = "";
            var nn = 0;

            SqlString = "Select count(Books.Id) as Numb From Books WITH (NOLOCK) INNER JOIN AspNetUsers WITH (NOLOCK) " +
                "on Books.PublisherId = AspNetUsers.Id where AspNetUsers.Id = @editorId";/* AspNetUsers.FirstName LIKE '%' +" +
                " @editorName + '%' or AspNetUsers.LastName LIKE '%' + @editorName +'%' " +
                            "or AspNetUsers.FirstName+' '+AspNetUsers.LastName LIKE '%' + @editorName +'%'";*/
            parameterName = "editorId";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(SqlString, conn))
                {
                    cmd.Parameters.AddWithValue(parameterName, editorId);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        nn = (int)reader["Numb"];
                        return nn;
                    }
                }
            }
            return 1;
        }

        [NonAction]
        public void checkFavoriteAndCart(List<BookViewModel> Booklist)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var bookVM in Booklist)
            {
                var fav = _IFavoriteService.GetFavoriteBookByUserIdAndBookId(userId, bookVM.Id).FirstOrDefault();
                if (fav != null) bookVM.isFavorite = !(fav.Id == Guid.Empty);
                var cb = _cartService.GetCartByUserIdAndBookId(userId, bookVM.Id).FirstOrDefault();
                if (cb != null) bookVM.inCart = !(cb.Id == Guid.Empty);
            }
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> EditorDashboard()
        {
            //get the list of selled books published by this editor
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            List<InvoiceVM> list = _InvoiceService.GetByEditorId(0, 14, currentUser.Id).ToList();
            var allCountries = base.GetAllCountries().ToList();
            foreach (var l in list)
            {
                l.User.Country = new Country();
                l.User.Country.Name = allCountries.Where(x => x.Id == l.User.CountryId).FirstOrDefault().Name;
            }
            return PartialView(list);
        }

        [HttpGet]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }

        [HttpPost]
        // [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                var errorsTab = ModelState.Keys.ToArray();
                foreach (var error in errorsTab) NotifyError("Error_" + error.ToUpper(), "Error_" + error);
                return Response();
            }
            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email || r.UserName == model.Email);

            if (appUser == null)
            {
                NotifyError("User_Not_Found", "UserNotFound");
                return Response();
            }
            var role = _userManager.GetRolesAsync(appUser).Result[0];

            if (role == UserRoleVM.Admin.ToString())
            {
                NotifyError("Admin_Not_Authorized", "AdminNotAuthorized");
                return Response();
            }

            // Sign In
            var signInResult = await _signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, false);
            if (!signInResult.Succeeded)
            {
                bool emailStatus = await _userManager.IsEmailConfirmedAsync(appUser);
                if (!emailStatus)
                {
                    ModelState.AddModelError(nameof(model.Email), "EmailNotConfirmed");
                    NotifyModelStateErrors();
                    return Response();
                }
                NotifyError(signInResult.ToString(), "WrongPassword");
                return Response();
            }
            _logger.LogInformation(1, "User logged in.");

            if (role == UserRoleVM.Editor.ToString())
                return Json(new { result = "Redirect", url = Url.Action(nameof(MyAccountController.Index), "MyAccount"), isEditor = 1 });
            else
                return Json(new { result = "Redirect", url = Url.Action(nameof(HomeController.Index), "Home"), isEditor = 0 });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorsTab = ModelState.Keys.ToArray();
                foreach (var error in errorsTab) NotifyError("Error_" + error, "Error_" + error);
                //NotifyModelStateErrors();
                return Response();
            }
            if (model.Password != model.ConfirmPassword)
            {
                NotifyError("Error_ConfirmPassword", "Error_ConfirmPassword");
                return Response();
            }
            // Add User
            var appUser = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                Birthdate = model.Birthdate,
                PhotoPath = model.PhotoPath,
                Address = model.Address,
                CountryId = model.CountryId,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                var identityResult = await _userManager.CreateAsync(appUser, model.Password);
           
            if (!identityResult.Succeeded)
            {
                var errorsTab = identityResult.Errors.ToArray();
                foreach (var error in errorsTab) NotifyError("Error_" + error.Code.ToUpper(), error.Code);
                //AddIdentityErrors(identityResult);
                //View(model);
                return Response();
            }

            // Add UserRoles
            identityResult = await _userManager.AddToRoleAsync(appUser, UserRoleVM.Subscriber.ToString());
            if (!identityResult.Succeeded)
            {
                var errorsTab = identityResult.Errors.ToArray();
                foreach (var error in errorsTab) NotifyError("Error_" + error.Code.ToUpper(), error.Code);
                //AddIdentityErrors(identityResult);
                View(model);
            }

            //Send Email confirmation
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var callbackUrl = Url.MyEmailConfirmationLink(appUser.Id, code, Request.Scheme);
            await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToAction(nameof(MyAccountController.RegisterConfirmation), "MyAccount");
                //return PartialView("RegisterConfirmation");
            }
            else
            {
                await _signInManager.SignInAsync(appUser, isPersistent: false);
            }

            _logger.LogInformation(3, "User created a new account with password.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        [HttpGet]
        public IActionResult RegisterConfirmation()
        {

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError($"Unable to load user with ID '{userId}'.");
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmedEmail" : "Error");
        }

        [HttpGet]
        public IActionResult ConfirmedEmail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailNotConfirmedError()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswordMail(string Email)
        {
            Email = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(Email);
            string FilePath = Directory.GetCurrentDirectory() + "\\wwwroot\\Template\\Html\\ChangePasswordEmailTemplate.html";

            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == Email);
            if (appUser == null)
            {
                NotifyError("User_Not_Found", "UserNotFound");
                return Response();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            var callbackUrl = Url.MyResetPasswordCallbackLink(appUser.Id, Email, code, Request.Scheme);

            MailText = MailText.Replace("[email]", Email).Replace("[link]", callbackUrl);

            await _emailSender.SendChangePasswordviaEmailAsync(Email, MailText);
            return Response();
        }

        [HttpPost]
        [AllowAnonymous]
        //[Route("SendContactMail")]
        public async Task SendContactMail(JoinRequestVM model)
        {
            //string FromEmail, string subject, string BodyContent
            await _emailSender.SendContactEmailAsync(model.Email, model.FirstName, model.LastName, model.Subject, model.Description);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email = null, string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code, Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [Route("UsersByRole")]
        public IActionResult GetUsersByRole(string role)
        {
            return base.GetUsersByRole(role);
        }

        [HttpGet]
        public IActionResult GetWishlistBooks()
        {
            var allEditors = base.GetAllEditors().ToList();
            var allAuthors = base.GetAllAuthorVM().ToList();
            //var allAuthors = base.GetAllAuthors().ToList();
            var allCategories = base.GetAllCategories().ToList();
            var allLanguages = base.GetAllLanguages().ToList();
            var allCountries = base.GetAllCountries().ToList();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var listFB = _IFavoriteService.GetFavoriteBooksByUserId(userId).Distinct().ToList();
            var listBooks = new List<BookViewModel>();
            foreach (var fb in listFB)
                listBooks.Add(_bookService.GetBookById(fb.BookId));

            if (listBooks.Count > 0)
            {
                var _favoriteBooks = _IFavoriteService.GetFavoriteBooksByUserId(userId).ToList();
                var _cartItems = _cartService.GetCartsByUserId(userId).ToList();
                var _libraryItems = _libraryService.GetLibrariesByUserId(userId).ToList();
                var DiscountedBooksList = _promotionService.GetDiscountBooks(0, 30);
                var FreeBooksList = _promotionService.GetFreeBooks(0, 30);
                var AllFreePromotions = _promotionService.GetAllFreePromotions().ToList();
                SetLibraryCapacity(listBooks, FreeBooksList, AllFreePromotions, userId);

                FillBookVMAdditionalInfos.FillBookVMList(listBooks, allAuthors, allEditors, allCategories, allCountries, allLanguages, null, _favoriteBooks, _cartItems, _libraryItems, DiscountedBooksList, FreeBooksList);
            }

            return PartialView("FavoriteBooksList", listBooks);
        }

        [HttpGet]
        public IActionResult GetCartListDashboard()
        {
            var list = base.GetCartItems();
            setBookPrice(list.Select(x => x.CartVM.Book).ToList());
            return PartialView("CartList", list);
        }


        #region ClickToPay

        [Route("ClickToPayRegister")]
        [HttpGet]
        public async Task<IActionResult> ClickToPayRegister()
        {
            var list = base.GetCartItems();
            setBookPrice(list.Select(x => x.CartVM.Book).ToList());
            double amount = list.Sum(x => x.CartVM.Book.BusinessPrice) * 1000 + 1000;
            string invoiceCode = DateTime.Now.ToString("yyyyMMddHHmmss");
            string returnPath = $"{Request.Scheme}://{Request.Host}/MyAccount/PaymentFinish";

            var userName = _configuration.GetSection("PaymentInfos").GetSection("userName").Value;
            var password = _configuration.GetSection("PaymentInfos").GetSection("password").Value;


            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://ipay.clictopay.com/payment/rest/register.do?amount={amount}&currency=788&language=fr&orderNumber={invoiceCode}&password={password}&returnUrl={returnPath}&userName={userName}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                //if success : save values in cache memory
                PaymentDetailsVM _paymentDetailsVM = System.Text.Json.JsonSerializer.Deserialize<PaymentDetailsVM>(responseBody);
                if (!string.IsNullOrWhiteSpace(_paymentDetailsVM.formUrl))
                {
                    base.AddPaymentInfosCacheableItems(new KeyValuePair<string, string>(invoiceCode, _paymentDetailsVM.orderId));
                    return Response(new { url = _paymentDetailsVM.formUrl });
                }

                return new JsonResult(new { message = "error !" })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };

            }
            catch (HttpRequestException exp)
            {
                return new JsonResult(new { message = exp.Message })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpGet]
        public async Task<IActionResult> MPGSCreateSession()
        {
            var list = base.GetCartItems();
            setBookPrice(list.Select(x => x.CartVM.Book).ToList());
            double amount = list.Sum(x => x.CartVM.Book.BusinessPrice)+1;
            string invoiceCode = DateTime.Now.ToString("yyyyMMddHHmmss");
            string returnPath = $"{Request.Scheme}://{Request.Host}/MyAccount/PostalPaymentFinish";
            var myData = new paiementObj
            {
                apiOperation = "INITIATE_CHECKOUT",
                order = new Order
                {
                    id = invoiceCode,
                    currency = "TND",
                    amount = (decimal)amount,
                    description = "Commande Clik2Read N° " + invoiceCode
                },
                interaction = new Interaction
                {
                    operation = "PURCHASE",
                    returnUrl = returnPath,
                    displayControl = new DisplayControl
                    {
                        billingAddress = "HIDE",
                        customerEmail = "OPTIONAL"
                    },
                    merchant = new Merchant
                    {
                        name = "Clik2Read"
                    }
                }
            };

            //Tranform it to Json object
            string jsonData = JsonConvert.SerializeObject(myData);

            var merchantId = _configuration.GetSection("MPGSCredentials").GetSection("merchantId").Value;
            var userName = _configuration.GetSection("MPGSCredentials").GetSection("userName").Value;
            var password = _configuration.GetSection("MPGSCredentials").GetSection("password").Value;

            var url = _configuration.GetSection("MPGSCredentials").GetSection("url").Value;
            using var client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            var stringContent = new StringContent(JsonConvert.SerializeObject(myData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url + "/session", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            var jsonObject = JObject.Parse(result);
            var resultIndicator = (string)jsonObject["successIndicator"];
            base.SetResultIndicatorMPGS(resultIndicator);
            base.SetOrderNumberMPGS(invoiceCode);
            base.SetOrderAmountMPGS(amount.ToString());

            return new JsonResult(new
            {
                result,
                orderId = invoiceCode,
                orderAmount = amount,
                returnUrl = returnPath,
            })
            ;
        }
        [HttpGet]
        public async Task<IActionResult> PostalPaymentFinish()
        {
            string resultIndicator = HttpContext.Request.Query["resultIndicator"];
            var sessionResultIndicator = base.GetResultIndicatorMPGS();

            if (resultIndicator == sessionResultIndicator)
            {
                var orderNumber = base.GetOrderNumberMPGS();
                var merchantId = _configuration.GetSection("MPGSCredentials").GetSection("merchantId").Value;
                var userName = _configuration.GetSection("MPGSCredentials").GetSection("userName").Value;
                var password = _configuration.GetSection("MPGSCredentials").GetSection("password").Value;

                var url = _configuration.GetSection("MPGSCredentials").GetSection("url").Value;
                using var client = new HttpClient();
                var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var response = await client.GetAsync(url + "/order/" + orderNumber);
                var result = await response.Content.ReadAsStringAsync();
                var keys = result.Split(',');
                var authorizationString = Array.FindAll(keys, s => s.Contains("authorizationCode"));

                PaymentDetailsVM _paymentDetailsVM = new PaymentDetailsVM();
                var _currentUser = await _userManager.GetUserAsync(HttpContext.User);

                //remove from cart table
                var listCart = base.GetCartItems().Where(x => x.CartVM.UserId == _currentUser.Id).ToList();
                foreach (var c in listCart)
                {
                    //add to table Invoice
                    InvoiceVM _invoiceVM = new InvoiceVM();
                    _invoiceVM.Id = Guid.NewGuid();
                    _invoiceVM.UserId = _currentUser.Id;
                    _invoiceVM.BookId = c.CartVM.BookId;
                    _invoiceVM.Date = DateTime.Now;
                    _invoiceVM.Price = c.CartVM.Book.BusinessPrice;
                    _invoiceVM.OrderNumber = base.GetOrderNumberMPGS();
                    _invoiceVM.PaymentType = GlobalConstant.PaymentType.PostOffice;
                    if (authorizationString.Any())
                    {
                        char[] MyChar = { '"' };
                        _invoiceVM.AuthorizationCode = authorizationString[0].Split(':')[1].Trim(MyChar);
                    }
                    _InvoiceService.AddInvoice(_invoiceVM);

                    //add to table Library
                    LibraryVM libraryVM = new LibraryVM();
                    libraryVM.Id = Guid.NewGuid();
                    libraryVM.BookId = c.CartVM.BookId;
                    libraryVM.UserId = _currentUser.Id;
                    _libraryService.AddBookToLibrary(libraryVM);

                    //remove row from table Cart
                    _cartService.DeleteBookFromCart(c.CartVM.Id);
                }
                _paymentDetailsVM.OrderNumber = base.GetOrderNumberMPGS();
                _paymentDetailsVM.UserName = $"{_currentUser.FirstName} {_currentUser.LastName}";
                _paymentDetailsVM.Date = DateTime.Now;
                if (authorizationString.Any())
                {
                    char[] MyChar = { '"' };
                    _paymentDetailsVM.AuthorizationCode = authorizationString[0].Split(':')[1].Trim(MyChar);
                }
                double amount = 0;
                Double.TryParse(base.GetOrderAmountMPGS(), out amount);
                _paymentDetailsVM.Amount = amount+1;
                //remove from cache memory
                _memoryCache.Remove(Models.Constants.MPGSPaymentCacheKey);
                _memoryCache.Remove(Models.Constants.MPGSOrderAmountCacheKey);
                _memoryCache.Remove(Models.Constants.MPGSOrderNumberCacheKey);

                return View("PaymentFinish", _paymentDetailsVM);
            }
            else return View("Error");


        }
        [HttpGet]
        public async Task<IActionResult> PaymentFinish()
        {
            //add bloc of get order status
            HttpClient client = new HttpClient();
            try
            {
                var OrderId = base.GetPaymentInfos().Value;
                HttpResponseMessage response = await client.GetAsync($"https://ipay.clictopay.com/payment/rest/getOrderStatus.do?orderId={OrderId}&language=fr&password=w55aMc2H&userName=0391076510");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                //deserialize data of response, put it in a VM, send the VM with the partial view
                PaymentDetailsVM _paymentDetailsVM = System.Text.Json.JsonSerializer.Deserialize<PaymentDetailsVM>(responseBody);
                var _currentUser = await _userManager.GetUserAsync(HttpContext.User);

                if (_paymentDetailsVM.ErrorCode == "0")
                {
                    //remove from cart table
                    var listCart = base.GetCartItems().Where(x => x.CartVM.UserId == _currentUser.Id).ToList();
                    foreach (var c in listCart)
                    {
                        //add to table Invoice
                        InvoiceVM _invoiceVM = new InvoiceVM();
                        _invoiceVM.Id = Guid.NewGuid();
                        _invoiceVM.UserId = _currentUser.Id;
                        _invoiceVM.BookId = c.CartVM.BookId;
                        _invoiceVM.Date = DateTime.Now;
                        _invoiceVM.Price = c.CartVM.Book.BusinessPrice;
                        _invoiceVM.OrderNumber = base.GetPaymentInfos().Key;
                        _invoiceVM.PaymentType = GlobalConstant.PaymentType.Bank;
                        _InvoiceService.AddInvoice(_invoiceVM);

                        //add to table Library
                        LibraryVM libraryVM = new LibraryVM();
                        libraryVM.Id = Guid.NewGuid();
                        libraryVM.BookId = c.CartVM.BookId;
                        libraryVM.UserId = _currentUser.Id;
                        _libraryService.AddBookToLibrary(libraryVM);

                        //remove row from table Cart
                        _cartService.DeleteBookFromCart(c.CartVM.Id);
                    }
                    _paymentDetailsVM.OrderNumber = base.GetPaymentInfos().Key;
                    _paymentDetailsVM.UserName = $"{_currentUser.FirstName} {_currentUser.LastName}";
                    _paymentDetailsVM.Date = DateTime.Now;
                    _paymentDetailsVM.Amount /= 1000;
                    //remove from cache memory
                    _memoryCache.Remove(Models.Constants.PaymentCacheKey);

                    return View(_paymentDetailsVM);
                }
                else return View("Error");

            }
            catch (HttpRequestException e)
            {
                return PartialView("Error");
            }
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
        private void setBookPrice(List<BookViewModel> bookList)
        {
            foreach (var Model in bookList)
            {

                if (Model.PromotionsPercentage != 0 && Model.PromotionsPercentage != 100)
                    Model.BusinessPrice = Model.BusinessPrice - (Model.BusinessPrice * Model.PromotionsPercentage / 100);
            }
        }
        #endregion

        /*
        public IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public IActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginurl = fb.GetLoginUrl(new
            {
                client_id = FB_AppID,
                client_secret = FB_AppSecret,
                redirect_uri = RedirectUri.AbsoluteUri,
                Response_type = "code",
                scope = "email"
            }); ;
            return Redirect(loginurl.AbsoluteUri);
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Headers["Referer"].ToString());
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallBack");

                return uriBuilder.Uri;
            }
        }

        public IActionResult FacebookCallBack(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = FB_AppID,
                client_secret = FB_AppSecret,
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            fb.AccessToken = accessToken;
            dynamic data = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
            TempData["email"] = data.email;
            TempData["name"] = data.first_name + " " + data.last_name;
            TempData["picture"] = data.picture.data.url;

            return RedirectToAction("Index", "Home");
        }
        */

    }
}
class Order
{
    public string id { get; set; }
    public string currency { get; set; }
    public decimal amount { get; set; }
    public string description { get; set; }

}
class Interaction
{
    public string operation { get; set; }
    public string returnUrl { get; set; }
    public DisplayControl displayControl { get; set; }
    public Merchant merchant { get; set; }

}
class Merchant
{
    public string name { get; set; }
    //public string returnUrl { get; set; }
    //public DisplayControl displayControl { get; set; }

}
class DisplayControl
{
    public string billingAddress { get; set; }
    public string customerEmail { get; set; }

}
class Customer
{
    public string customerEmail { get; set; }

}
class paiementObj
{
    public string apiOperation { get; set; }
    public Order order { get; set; }
    public Interaction interaction { get; set; }
    public Customer customer { get; set; }

}
