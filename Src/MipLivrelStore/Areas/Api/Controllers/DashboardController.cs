using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Application.ILogics;
using DDD.Application.Interfaces;
using DDD.Application.Specifications;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DDD.Application.Enum.Constants;

namespace MipLivrelStore.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
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

        public DashboardController(
            IBookService bookService, UserManager<ApplicationUser> userManager, ICategoryService categoryService,
            ICountryService countryService, ILanguageService languageService, IFavoriteService IFavoriteService,
            IFileManagerLogic fileManagerLogic, ILibraryService libraryService,
            ICartService cartService, IPromotionService promotionService, IAuthorService authorService, IInvoiceService InvoiceService) : base()
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
        [Route("getTotalPublishedBooksCount")]
        public async Task<double> getTotalPublishedBooksCount()
        {
            var count = await _bookService.GetPublishedBookCount(new DDD.Application.Specifications.PagedBooks());
            return count;
        }

        [HttpGet]
        [Route("GetEditorsCount")]
        public async Task<IActionResult> GetEditorsCount()
        {
            var count = _userManager.GetUsersInRoleAsync(UserRoleVM.Editor.ToString()).Result.Count;
            return Ok(count);
        }

        [HttpGet]
        [Route("GetFiltredSelledBooksCount")]
        public async Task<double> GetSelledBookByFilter([FromQuery] DashboardFilter dashboardFilter)
        {
            var count = await _bookService.GetTotalSelledBooksCountByFilter(dashboardFilter);
            return count;
        }

        [HttpGet]
        [Route("GetFiltredSelledBooksByDay")]
        public async Task<IActionResult> GetDaySelledBookByFilter([FromQuery] DashboardFilter dashboardFilter)
        {
            var count = await _bookService.GetDaySelledBooksByFilter(dashboardFilter);
            return Ok(count);
        }

        [HttpGet]
        [Route("GetPagedTopSelledBooks")]
        public async Task<IActionResult> GetSelledBookByFilter(int index, int size)
        {
            var count = await _bookService.GetTopSelledBooks(index, size);
            return Ok(count);
        }

        [HttpGet]
        [Route("GetPagedTopPublishers")]
        public async Task<IActionResult> GetPagedTopPublishers(int index, int size)
        {
            var count = await _bookService.GetTopPublishers(index, size);
            return Ok(count);
        }

        [HttpGet]
        [Route("GetPagedTopSubscribers")]
        public async Task<IActionResult> GetPagedTopSubscribers(int index, int size)
        {
            var count = await _bookService.GetTopSubscribers(index, size);
            return Ok(count);
        }

        [HttpGet]
        [Route("GetPagedSubscriberPurchase")]
        public async Task<IActionResult> GetSubscriberPurchaseDetails(int index, int size, string userId)
        {
            var count = await _bookService.GetSubscriberPurchaseDetails(index, size, userId);
            return Ok(count);
        }
    }
}
