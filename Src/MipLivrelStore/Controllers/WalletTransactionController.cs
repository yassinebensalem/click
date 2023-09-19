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
using DDD.Application.Services;
using Microsoft.AspNetCore.Authorization;
using iTextSharp.text;
using DDD.Domain.Common.Constants;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MipLivrelStore.Controllers
{
    public class WalletTransactionController : BaseController
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
        private readonly IWalletTransactionService _walletTransactionService;
        private readonly IInvoiceService _invoiceService;

        public WalletTransactionController(
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
             ILanguageService languageService, IAuthorService authorService, ICartService cartService, IConfiguration configuration, IPrizeService prizeService,
             IWalletTransactionService walletTransactionService,
             IInvoiceService invoiceService)

            : base(joinRequestService, fileManagerLogic, memoryCache, userManager, loggerFactory, notifications,
              mediator, bookService, categoryService, mapper, promotionService, countryService, languageService, cartService, authorService, prizeService, libraryService)
        {
            _joinRequestService = joinRequestService;
            _fileManagerLogic = fileManagerLogic;
            _memoryCache = memoryCache;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<WalletTransactionController>();
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
            _walletTransactionService = walletTransactionService;
            _invoiceService = invoiceService;
        }


        [HttpGet]
        //[Route("GetCurrentUserBalance")]
        public JsonResult GetCurrentUserBalance()
        {
            ApplicationUser currentUser = null;
            if (User.Identity.Name != null)
            {
                currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                //currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            if (currentUser == null)
            {
                return Json(null);
            }
            var balance = _walletTransactionService.GetUserBalance(Guid.Parse(currentUser.Id));

            return Json(new
            {
                balance = balance
            });
        }

        [HttpGet]
        public IActionResult GetWalletTransactions()
        {
            ApplicationUser currentUser = null;
            if (User.Identity.Name != null)
            {
                currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                //currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            if (currentUser == null)
            {
                return Json("_WalletTransactions");
            }
            var list = _walletTransactionService.GetHistoryByUserId(Guid.Parse(currentUser.Id));
           
            return PartialView("_WalletTransactions", list.ToList());
        }

        #region ClickToPay

        [Route("ClickToPayRegister")]
        [HttpGet]
        public async Task<IActionResult> ClickToPayRegister(int amount)
        {
            string invoiceCode = DateTime.Now.ToString("yyyyMMddHHmmss");
            string returnPath = $"{Request.Scheme}://{Request.Host}/WalletTransaction/PaymentFinish";

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
        public async Task<IActionResult> MPGSCreateSession(int amount)
        {
            string invoiceCode = DateTime.Now.ToString("yyyyMMddHHmmss");
            string returnPath = $"{Request.Scheme}://{Request.Host}/WalletTransaction/PostalPaymentFinish";
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

                //add to table Invoice
                InvoiceVM _invoiceVM = new InvoiceVM();
                _invoiceVM.Id = Guid.NewGuid();
                _invoiceVM.UserId = _currentUser.Id;
                _invoiceVM.Date = DateTime.Now;
                _invoiceVM.Price = _paymentDetailsVM.Amount;
                _invoiceVM.OrderNumber = base.GetOrderNumberMPGS();
                _invoiceVM.PaymentType = GlobalConstant.PaymentType.PostOffice;
                _invoiceVM.PaymentReason = GlobalConstant.PaymentReason.WalletRefill;
                if (authorizationString.Any())
                {
                    char[] MyChar = { '"' };
                    _invoiceVM.AuthorizationCode = authorizationString[0].Split(':')[1].Trim(MyChar);
                }
                _invoiceService.AddInvoice(_invoiceVM);

                await AddWalletTransaction(_currentUser.Id, _paymentDetailsVM.Amount);

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
                _paymentDetailsVM.Amount = amount + 1;
                //remove from cache memory
                _memoryCache.Remove(Models.Constants.MPGSPaymentCacheKey);
                _memoryCache.Remove(Models.Constants.MPGSOrderAmountCacheKey);
                _memoryCache.Remove(Models.Constants.MPGSOrderNumberCacheKey);

                return View("/MyAccount/PaymentFinish", _paymentDetailsVM);
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

                    //add to table Invoice
                    InvoiceVM _invoiceVM = new InvoiceVM();
                    _invoiceVM.Id = Guid.NewGuid();
                    _invoiceVM.UserId = _currentUser.Id;
                    _invoiceVM.Date = DateTime.Now;
                    _invoiceVM.Price = _paymentDetailsVM.Amount;
                    _invoiceVM.OrderNumber = base.GetPaymentInfos().Key;
                    _invoiceVM.PaymentType = GlobalConstant.PaymentType.Bank;
                    _invoiceVM.PaymentReason = GlobalConstant.PaymentReason.WalletRefill;
                    _invoiceService.AddInvoice(_invoiceVM);

                    await AddWalletTransaction(_currentUser.Id, _paymentDetailsVM.Amount/1000);

                    _paymentDetailsVM.OrderNumber = base.GetPaymentInfos().Key;
                    _paymentDetailsVM.UserName = $"{_currentUser.FirstName} {_currentUser.LastName}";
                    _paymentDetailsVM.Date = DateTime.Now;
                    _paymentDetailsVM.Amount /= 1000;
                    //remove from cache memory
                    _memoryCache.Remove(Models.Constants.PaymentCacheKey);

                    return View("/MyAccount/PaymentFinish", _paymentDetailsVM);
                }
                else return View("Error");

            }
            catch (HttpRequestException e)
            {
                return PartialView("Error");
            }
        }

        private async Task<bool> AddWalletTransaction(string userId, double amount)
        {
            var userIds = new List<Guid>();
            userIds.Add(Guid.Parse(userId));
            var walletTransactionVM = new WalletDispatchTransactionViewModel
            {
                UserIds = userIds,
                Amount = (float)amount,
                Status = true,
                Type = DDD.Domain.Models.Enums.WalletTransactionTypeEnum.Refill
            };
            return await _walletTransactionService.Refill(walletTransactionVM);
        }


        #endregion
    }
}
