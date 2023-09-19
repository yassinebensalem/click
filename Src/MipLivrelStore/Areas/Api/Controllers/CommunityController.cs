using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.Application.Interfaces;
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
    public class CommunityController : ApiController
    {
        private readonly ICommunityService _communityService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CommunityController(
            ICommunityService communityService,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
        INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _communityService = communityService;
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<CommunityController>();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getAll")]
        public IActionResult Get()
        {
            return Response(_communityService.GetAll());
        }

        [HttpGet]
        [AllowAnonymous]
        //[Route("book-managment/{id:guid}")]
        [Route("getById")]
        public IActionResult Get(Guid id)
        {
            var communityViewModel = _communityService.GetCommunityById(id);

            return Response(communityViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("pagination")]
        public IActionResult GetByPageIndex(int skip, int take)
        {
            return Response(_communityService.GetAll(skip, take));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        //[AllowAnonymous]
        [Route("Add")]
        public async Task<IActionResult> Post([FromBody] CommunityEditViewModel communityViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(communityViewModel);
            }

            var addResult = await _communityService.AddCommunity(communityViewModel);
            if (addResult)
            {
                return Response(communityViewModel);
            }
            return Response(null, false);
        }



        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        [Route("Update")]
        //[AllowAnonymous]
        public async Task<IActionResult> Put([FromBody] CommunityEditViewModel communityViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(communityViewModel);
            }

            var updateResult = await _communityService.UpdateCommunity(communityViewModel);

            if (updateResult)
            {
                return Response(communityViewModel);
            }
            return Response(null, false);
        }

        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        [Route("Delete")]
        //[AllowAnonymous]
        public IActionResult Delete(Guid id)
        {
            var DeleteResult = _communityService.DeleteCommunity(id);

            return Response();
        }
                 
        [HttpGet]
        [AllowAnonymous]
        [Route("SearchMembers")]
        public IActionResult SearchMembers(Guid communityId, string keywords, int skip, int take)
        {
            return Response(_communityService.GetMembers(communityId, keywords, skip, take));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("SearchUsers")]
        public IActionResult SearchUsers(string keywords, int skip, int take)
        {
            return Response(_communityService.GetUsers(keywords, skip, take));
        }
                
        [HttpPut]
        [Authorize(Roles = Roles.CommunityAdmin)]
        //[AllowAnonymous]
        [Route("AssociateMember")]
        public IActionResult AssociateMember([FromBody] CommunityMemberViewModel communityMemberViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(communityMemberViewModel);
            }

            var AddResult = _communityService.AssociateMember(communityMemberViewModel);

            return Response(communityMemberViewModel);
        }

        [HttpPut]
        [Authorize(Roles = Roles.CommunityAdmin)]
        //[AllowAnonymous]
        [Route("DissociateMember")]
        public IActionResult DissociateMember([FromBody] CommunityMemberViewModel communityMemberViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(communityMemberViewModel);
            }

            var AddResult = _communityService.DissociateMember(communityMemberViewModel);

            return Response(communityMemberViewModel);
        }

        [HttpPut]
        [Authorize(Roles = Roles.CommunityAdmin)]
        //[AllowAnonymous]
        [Route("InviteWithMembership")]
        public async Task<IActionResult> InviteWithMembership([FromBody] CommunityInvitationalViewModel invitationalViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(invitationalViewModel);
            }

            var communityViewModel = _communityService.GetCommunityById(invitationalViewModel.CommunityId);
            string password = GeneratePassword();
            //_configuration.GetSection("DefaultPass") != null ? _configuration.GetSection("DefaultPass").Value : _DEFAULTPASSWORD;

            if (communityViewModel == null)
            {
                ModelState.AddModelError("Not found", "Community not found");
                NotifyModelStateErrors();
                return Response(invitationalViewModel);
            }

            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == invitationalViewModel.Email);
            if (appUser == null)
            {
                appUser = new ApplicationUser
                {
                    Email = invitationalViewModel.Email,
                    UserName = invitationalViewModel.Email,
                };
                var identityResult = await _userManager.CreateAsync(appUser, password);
                if (!identityResult.Succeeded)
                {
                    AddIdentityErrors(identityResult);
                    return Response();
                }

                // Add UserRoles
                identityResult = await _userManager.AddToRoleAsync(appUser, Roles.Subscriber);
                if (!identityResult.Succeeded)
                {
                    AddIdentityErrors(identityResult);
                    return Response();
                }
            }

            if (appUser != null)
            {
                var AddResult = await _communityService.InviteWithMembership(invitationalViewModel);
                if (AddResult)
                {
                    //send an email notification to the added user (subscriber added to community)
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    var callbackUrl = Url.CompleteUserInformationLink(appUser.Email, Request.Scheme);
                    var _currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
                    var communityManagerName = $"{_currentUser.FirstName} {_currentUser.LastName}";
                    await _emailSender.SendEmailConfirmationAndJoinCommunityAsync(communityManagerName, invitationalViewModel.Email, communityViewModel.CommunityName, callbackUrl, password);

                    _logger.LogInformation(3, "Invited user was created and will join community once he complete his registration.");
                    return Response(invitationalViewModel);
                }
            }

            ModelState.AddModelError("Not added", "Member was not added");
            NotifyModelStateErrors();
            return Response(invitationalViewModel);
        }

        [NonAction]
        private string GeneratePassword()
        {
            var options = _userManager.Options.Password;

            int length = options.RequiredLength;

            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }

    }
}
