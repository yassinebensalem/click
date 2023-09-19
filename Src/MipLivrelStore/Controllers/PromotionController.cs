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
using DDD.Application.Services;

namespace MipLivrelStore.Controllers
{
    public class PromotionController : BaseController
    {
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
        private readonly ILibraryService _libraryService;

        public PromotionController(IConfiguration configuration, IJoinRequestService joinRequestService, IFileManagerLogic fileManagerLogic,
            UserManager<ApplicationUser> userManager, ILogger logger, IBookService bookService, ICategoryService categoryService,
            ICountryService countryService, ILanguageService languageService, IMapper mapper, IMemoryCache memoryCache, ILoggerFactory loggerFactory,
            DomainNotificationHandler notifications, IMediatorHandler mediator, IAuthorService authorService,
            IFavoriteService iFavoriteService, ILibraryService  libraryService, IPromotionService promotionService,
            ICartService cartService, IPrizeService prizeService, ICompositeViewEngine viewEngine)
        : base(joinRequestService, fileManagerLogic, memoryCache, userManager, loggerFactory, notifications,
              mediator, bookService, categoryService, mapper, promotionService, countryService, languageService, cartService, authorService, prizeService, libraryService)
        {
            _configuration = configuration;
            _joinRequestService = joinRequestService;
            _fileManagerLogic = fileManagerLogic;
            _userManager = userManager;
            _logger = logger;
            _bookService = bookService;
            _categoryService = categoryService;
            _countryService = countryService;
            _languageService = languageService;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _notifications = notifications;
            _mediator = mediator;
            _authorService = authorService;
            _IFavoriteService = iFavoriteService;
            _cartService = cartService;
            _prizeService = prizeService;
            _viewEngine = viewEngine;
            _libraryService = libraryService;
        }


    }
}
