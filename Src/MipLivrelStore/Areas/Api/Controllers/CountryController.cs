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
    public class CountryController : ApiController
    {
        private readonly ICountryService _countryService;

        public CountryController(
            ICountryService countryService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _countryService = countryService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getAll")]
        public IActionResult Get()
        {
            return Response(_countryService.GetAll());
        }

        [HttpGet]
        //[AllowAnonymous]
        //[Route("book-managment/{id:guid}")]
        [Route("getById")]
        public IActionResult Get(int id)
        {
            var countryViewModel = _countryService.GetCountryById(id);

            return Response(countryViewModel);
        }
    }
}
