using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
using DDD.Application.Services;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Models;
using DDD.Infra.CrossCutting.Identity.Authorization;
using DDD.Infra.CrossCutting.Identity.Extensions;
using DDD.Infra.CrossCutting.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MipLivrelStore.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WalletTransactionController : ApiController
    {
        private readonly IWalletTransactionService _walletTransactionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public WalletTransactionController(
            IWalletTransactionService walletTransactionService,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
        INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _walletTransactionService = walletTransactionService;
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<CommunityController>();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetHistoryByUserId")]
        public IActionResult GetHistoryByUserId(Guid userId)
        {
            return Response(_walletTransactionService.GetHistoryByUserId(userId));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetHistoryByUserIdWithPagination")]
        public IActionResult GetHistoryByUserIdWithPagination(Guid userId, int skip, int take)
        {
            return Response(_walletTransactionService.GetHistoryByUserIdWithPagination(userId, skip, take));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetHistoryByCommunityId")]
        public IActionResult GetHistoryByCommunityId(Guid communityId)
        {
            return Response(_walletTransactionService.GetHistoryByCommunityId(communityId));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetHistoryByCommunityIdWithPagination")]
        public IActionResult GetHistoryByCommunityIdWithPagination(Guid communityId,int skip, int take)
        {
            return Response(_walletTransactionService.GetHistoryByCommunityIdWithPagination(communityId, skip, take));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetUserBalance")]
        public IActionResult GetUserBalance(Guid userId)
        {
            var communityViewModel = _walletTransactionService.GetUserBalance(userId);

            return Response(communityViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetCommunityBalance")]
        public IActionResult GetCommunityBalance(Guid communityId)
        {
            var communityViewModel = _walletTransactionService.GetCommunityBalance(communityId);

            return Response(communityViewModel);
        }
                
        [HttpPost]
        [Authorize(Roles = Roles.Subscriber + "," + Roles.CommunityAdmin + "," + Roles.Admin)]
        //[AllowAnonymous]
        [Route("Refill")]
        public async Task<IActionResult> Refill([FromBody] WalletDispatchTransactionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(viewModel);
            }

            var addResult = await _walletTransactionService.Refill(viewModel);
            if (!addResult)
            {
                ModelState.AddModelError("RefillError", "Could not refill wallet");
                NotifyModelStateErrors();
            }

            return Response(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Subscriber + "," + Roles.CommunityAdmin + "," + Roles.Admin)]
        //[AllowAnonymous]
        [Route("Withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WalletDispatchTransactionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(viewModel);
            }

            var addResult = await _walletTransactionService.Withdraw(viewModel);
            if (!addResult)
            {
                ModelState.AddModelError("WithdrawError", "Could not withdraw wallet");
                NotifyModelStateErrors();
            }

            return Response(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = Roles.CommunityAdmin)]
        //[AllowAnonymous]
        [Route("Dispatch")]
        public async Task<IActionResult> DispatchToMember([FromBody] WalletDispatchTransactionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(viewModel);
            }

            var addResult = await _walletTransactionService.DispatchToMember(viewModel);
            if (!addResult)
            {
                ModelState.AddModelError("RefillError", "Could not dispatch balance");
                NotifyModelStateErrors();
            }

            return Response(viewModel);
        }

    }
}
