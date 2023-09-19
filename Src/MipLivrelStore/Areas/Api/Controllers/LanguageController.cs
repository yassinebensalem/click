using System;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Infra.CrossCutting.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MipLivrelStore.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LanguageController : ApiController
    {
        private readonly ILanguageService _languageService;

        public LanguageController(
            ILanguageService languageService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _languageService = languageService;
        }

        [HttpGet]

        [HttpGet]
        [AllowAnonymous]
        [Route("getAll")]
        public IActionResult Get()
        {
            return Response(_languageService.GetAll());
        }

        [HttpGet]
        [Route("getById")]
        public IActionResult Get(int id)
        {
            var languageViewModel = _languageService.GetLanguageById(id);

            return Response(languageViewModel);
        }



    }
}
