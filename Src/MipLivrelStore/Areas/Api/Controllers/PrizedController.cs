using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DDD.Application.ILogics;
using DDD.Application.Interfaces;
using DDD.Application.ViewModels;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Notifications;
using DDD.Domain.Interfaces;
using DDD.Domain.Models;
using DDD.Infra.CrossCutting.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MipLivrelStore.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class PrizedController : ApiController
    {
        private readonly IPrizeService _prizeService;
        private readonly IFileManagerLogic _fileManagerLogic;


        public PrizedController(IPrizeService prizeService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IFileManagerLogic fileManagerLogic) : base(notifications, mediator)
        {
            _prizeService = prizeService;
            _fileManagerLogic = fileManagerLogic;
        }


        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("AddPrize")]

        public async Task<IActionResult> AddPrizeAsync([FromBody] PrizeVM prizeVM)
        {

            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(prizeVM);
            }

            if (prizeVM.Logo != null)
            {
                // UplodePhoto
                string fileName = Guid.NewGuid().ToString();
                prizeVM.LogoPath = $"{fileName}{Path.GetExtension(prizeVM.Logo.FileName)}";
                await _fileManagerLogic.Upload(prizeVM.Logo, "bookscover", prizeVM.LogoPath);
            }

            _ = _prizeService.AddPrize(prizeVM);

          

            return Response(prizeVM);
        }

        [HttpPost]
        //[Authorize(Roles = Roles.Admin)]
        [Route("AddPrizeBook")]

        public IActionResult AddPrizebooks([FromBody] PrizeBookVM prizeBookVM)
        {

            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(prizeBookVM);
            }
            var AddResult = _prizeService.AddPrizeBook(prizeBookVM);
            return Response(prizeBookVM);
        }



        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [Route("getAllPrizeBook")]
        public IActionResult GetPrizeBooks()
        {
            return Response(_prizeService.GetAllPrizeBook());

        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        [Route("getAll")]
        public IActionResult Get()
        {
            return Response(_prizeService.GetAll());
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [Route("getBook")]
        public IActionResult GetBooks(PrizePostVM prizePostVM)
        {
            return Response(_prizeService.GetBookByTitle(prizePostVM));

            // return Response(new { bookId = prizePostVM.BookId, Title = prizePostVM.Title });

        }







    }
}
