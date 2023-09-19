using System.IO;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using DDD.Infra.CrossCutting.Identity.Authorization;
using DDD.Infra.CrossCutting.Identity.Extensions;
using DDD.Infra.CrossCutting.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MipLivrelStore.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class JoinRequestController : ApiController
    {
        private readonly IJoinRequestService _joinRequestService;
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private ICompositeViewEngine _viewEngine;

        public JoinRequestController(IJoinRequestService joinRequestService, INotificationHandler<DomainNotification> notifications,
            UserManager<ApplicationUser> userManager, IMediatorHandler mediator, IEmailSender emailSender, ICompositeViewEngine viewEngine)
            : base(notifications, mediator)

        {
            _joinRequestService = joinRequestService;
            _mediator = mediator;
            _notifications = (DomainNotificationHandler)notifications;
            _userManager = userManager;
            _emailSender = emailSender;
            _viewEngine = viewEngine;

        }
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [Route("GetById")]
        public IActionResult GetAllByIntreval(string requestId)
        {
            return Response(_joinRequestService.GetById(requestId));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("interval")]
        public IActionResult GetAllByIntreval(JoinRequestPostVM joinRequestPostVM)
        {
            return Response(_joinRequestService.GetAll(joinRequestPostVM));
        }

        //send the contract
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("SendContractToRequester")]
        public async Task<IActionResult> SendContractToRequester([FromBody] JoinRequestVM joinRequestVM)
        {
            await sendContractToRequester(joinRequestVM);
            return Response();
        }

        private async Task sendContractToRequester(JoinRequestVM joinRequestVM)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\wwwroot\\Template\\Html\\ContractEmailTemplate.html";
            //string LogoPath = Directory.GetCurrentDirectory() + "\\wwwroot\\img\\logo.png";

            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", $"{joinRequestVM.FirstName} {joinRequestVM.LastName}")
                .Replace("[email]", joinRequestVM.Email);
            await _emailSender.SendContractByEmailAsync(joinRequestVM.Email, MailText);
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [Route("Update")]
        public async Task<IActionResult> PutJointRequest([FromBody] JoinRequestVM joinRequestVM)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(joinRequestVM);
            }

            if(joinRequestVM.Status == DDD.Domain.Common.Constants.GlobalConstant.JoinRequestState.Accepted)
            {
                 await sendContractToRequester(joinRequestVM);
            }

            _joinRequestService.UpdateJoinRequest(joinRequestVM);
            return Response(joinRequestVM);
        }

        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        [Route("deleteCompetitionBook")]
        public IActionResult DeleteCompetitionBook(string requestId, string bookId)
        {
            return Response(_joinRequestService.DeleteCompetitionBook(requestId,bookId));
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [Route("AddCompetitionBook")]
        public IActionResult AddCompetitionBook(string requestId, string bookId)
        {
            return Response(_joinRequestService.AddCompetitionBook(requestId, bookId));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("GivewayBook")]
        public async Task<IActionResult> GivewayBook(string requestId, string userEmail)
        {
            var user=await _userManager.FindByEmailAsync(userEmail);
            if(user == null) { return BadRequest("User not found");}
            return Response(_joinRequestService.GivewayBook(requestId, user.Id));
        }
    }
}
